using System.Collections.Generic;
using System.Runtime.InteropServices;
using Common.Logging;
using SwitchLink.Cryptography.TritonCryptography;
using SwitchLink.Data;
using SwitchLink.Data.Models;

namespace SwitchLink.TritonNode.Services
{
    interface ICryptographyService
    {
        void CreateSessionKeys();
        void UpdateSessionKeys();

        string GetResponseCode();
        string GetEncryptedPinKey1();
        string GetEncryptedPinKey2();
    }

    class CryptographyService : ICryptographyService
    {
        private readonly ILog _log = LogManager.GetLogger<CryptographyService>();
        private readonly string _terminalId;
        private readonly TritonCryptography _crypt;

        private Dictionary<string, string> _takInfo;
        private Dictionary<string, string> _sessionKeys;
        private string _tpkTMK;

        public CryptographyService(string terminalId)
        {
            _terminalId = terminalId;
            _crypt = new TritonCryptography();
        }

        public void CreateSessionKeys()
        {
            Sessions_Triton_TMK tmkInfo;
            using (TerminalData data = new TerminalData())
            {
                tmkInfo = data.GetTMKByTerminalId(_terminalId);
                _log.Debug(string.Format("Get TMK for Terminal Id: {0}", _terminalId));
            }
            _log.Debug(string.Format("Create session key and tak for Terminal Id: {0}", _terminalId));
            _sessionKeys = _crypt.GenerateTerminalSessionKeys(tmkInfo.TMK);
            _takInfo = _crypt.GenerateKeys_TAK();
        }

        public void UpdateSessionKeys()
        {
            if (_sessionKeys != null && _takInfo != null)
            {
                _sessionKeys.TryGetValue("TPK", out _tpkTMK);

                string tpkChk;
                _sessionKeys.TryGetValue("TPK_Check", out tpkChk);

                string tpkLmk;
                _sessionKeys.TryGetValue("TPK_LMK", out tpkLmk);

                string tak;
                _takInfo.TryGetValue("TAK", out tak);

                string takChk;
                _takInfo.TryGetValue("TAK_Check", out takChk);

                using (TerminalData data = new TerminalData())
                {
                    data.UpdateSessionKeysByTerminalId(_terminalId, _tpkTMK, tpkChk, tpkLmk, tak, takChk);
                    _log.Debug(string.Format("Save the keys in database for Terminal Id: {0}", _terminalId));
                }
            }
            else
            {
                _log.Error(string.Format("Error session key or tak Info for Terminal Id:{0} <NULL>", _terminalId));
                throw new ExternalException(string.Format("Error session key or tak Info for Terminal Id:{0} <NULL>", _terminalId));
            }
        }

        public string GetResponseCode()
        {
            return _sessionKeys["ErrorCode"];
        }

        public string GetEncryptedPinKey1()
        {
            return _tpkTMK.Substring(1, 16);
        }

        public string GetEncryptedPinKey2()
        {
            return _tpkTMK.Substring(17, 16);
        }
    }
}