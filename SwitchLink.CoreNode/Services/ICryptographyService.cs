using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using SwitchLink.Cryptography.TritonCryptography;
using SwitchLink.Data;
using SwitchLink.Data.Models;

namespace SwitchLink.CoreNode.Services
{
    interface ICryptographyService
    {
        string GetTerminalLocation();
        CryptographyService.TranslateResult TranslatePin(string pinBlock, string track2);
    }

    class CryptographyService : ICryptographyService
    {
        private readonly ILog _log = LogManager.GetLogger<CryptographyService>();
        private readonly string _terminalId;

        public CryptographyService(string terminalId)
        {
            _terminalId = terminalId;
        }

        public string GetTerminalLocation()
        {
            using (var terData = new TerminalData())
            {
                _log.Debug("Geting Name location for Terminal");
                var results = terData.GetNameLocationByTerminalId(_terminalId);
                return results.name_location_name + ", " + results.name_location_city + ", " + results.name_location_state + ", " + results.name_location_country;
            }
        }

        public TranslateResult TranslatePin(string pinBlock, string track2)
        {
            Sessions_Core_ZPK_to_TPK tpkzpk;
            string pin;
            string errorCode;
            TritonCryptography crypt = new TritonCryptography();
            _log.Debug("Geting Keys for Pin Translation");
            using (var terData = new TerminalData())
            {
                tpkzpk = terData.GetTPKZPKByTerminalId(_terminalId);
            }
            _log.Debug("Formatting PAN");
            string accNumber = track2.Substring(0, track2.IndexOf("=", StringComparison.Ordinal));
            accNumber = accNumber.Substring(accNumber.Length - 13, 12);

            _log.Debug("Translating Pin Block");
            var translateResult = crypt.TranslatePIN_TDES(tpkzpk.TPK_LMK, tpkzpk.ZPK_LMK, pinBlock, accNumber);
            translateResult.TryGetValue("DestPIN", out pin);
            translateResult.TryGetValue("ErrorCode", out errorCode);

            var result = new List<TranslateResult> { new TranslateResult { PinBlock = pin, ErrorCode = errorCode } };
            return result.FirstOrDefault();
        }

        internal class TranslateResult
        {
            public string PinBlock { get; set; }
            public string ErrorCode { get; set; }
        }
    }
}