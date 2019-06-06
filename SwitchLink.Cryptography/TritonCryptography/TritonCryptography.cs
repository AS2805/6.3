using System;
using System.Collections.Generic;
using log4net;

namespace SwitchLink.Cryptography.TritonCryptography
{
    public class TritonCryptography : Cryptography
    {
        log4net.ILog logger = LogManager.GetLogger(typeof(TritonCryptography));

        public Dictionary<string, string> GenerateKeys(string keyType)
        {
            Dictionary<String, String> Response = new Dictionary<string, string>();
            String response = BuildCommandKey(keyType);
            String errorCode = response.Substring(8, 2);

            Response.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            Response.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            Response.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                Response.Add("TMK", response.Substring(10, 33));
                logger.Debug("TMK: " + response.Substring(10, 33));

                Response.Add("TMK_Check", response.Substring(43, 6));
                logger.Debug("TMK_Check: " + response.Substring(43, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return Response;
        }
        private string BuildCommandKey(String keyTyp)
        {
            logger.Info("Generating Command Key");
            String mode = "0", commandCode = "A0", keyType = keyTyp, keyScheme = "U";

            String message = commandCode;
            message += mode;
            message += keyType;
            message += keyScheme;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> GenerateKeys_TMK()
        {
            Dictionary<String, String> responseTMK = new Dictionary<string, string>();
            String response = BuildCommandTMK();
            String errorCode = response.Substring(8, 2);

            responseTMK.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            responseTMK.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            responseTMK.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                responseTMK.Add("TMK", response.Substring(10, 33));
                logger.Debug("TMK: " + response.Substring(10, 33));

                responseTMK.Add("TMK_Check", response.Substring(43, 6));
                logger.Debug("TMK_Check: " + response.Substring(43, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return responseTMK;
        }
        private string BuildCommandTMK()
        {
            logger.Info("Generating TMK");
            String mode = "0", commandCode = "A0", keyType = "002", keyScheme = "U";

            String message = commandCode;
            message += mode;
            message += keyType;
            message += keyScheme;

            logger.Info("Message to HSM :" + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> GenerateKeys_TAK()
        {
            Dictionary<String, String> responseTAK = new Dictionary<string, string>();
            String response = BuildCommandTAK();
            String errorCode = response.Substring(8, 2);

            responseTAK.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            responseTAK.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            responseTAK.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                responseTAK.Add("TAK", response.Substring(10, 33));
                logger.Debug("TAK: " + response.Substring(10, 33));

                responseTAK.Add("TAK_Check", response.Substring(43, 6));
                logger.Debug("TAK_Check: " + response.Substring(43, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return responseTAK;
        }   
        private string BuildCommandTAK()
        {
            logger.Info("Generating TAK");
            String mode = "0", commandCode = "A0", keyType = "003", keyScheme = "U";

            String message = commandCode;
            message += mode;
            message += keyType;
            message += keyScheme;

            logger.Info("Message to HSM :" + message);

            return SendMessage(message);
        }

        public Dictionary<String, String> GenerateTerminalSessionKeys(String terminalMasterKey)
        {
            Dictionary<String, String> response = new Dictionary<string, string>();
            String hsmResponse = BuildTerminalPinKey(terminalMasterKey);
           
            String errorCode = hsmResponse.Substring(8, 2);
           
            response.Add("Header", hsmResponse.Substring(2, 4));
            logger.Debug("Header: " + hsmResponse.Substring(2, 4));

            response.Add("ResponseCode", hsmResponse.Substring(6, 2));
            logger.Debug("ResponseCode: " + hsmResponse.Substring(6, 2));

            response.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                response.Add("TPK_LMK", hsmResponse.Substring(10, 33));
                logger.Debug("TPK_LMK: " + hsmResponse.Substring(10, 33));

                response.Add("TPK", hsmResponse.Substring(43, 33));
                logger.Debug("TPK: " + hsmResponse.Substring(43, 33));

                response.Add("TPK_Check", hsmResponse.Substring(76, 6));
                logger.Debug("TPK_Check: " + hsmResponse.Substring(76, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return response;
        }   
        private string BuildTerminalPinKey(String terminalMasterKey)
        {
            logger.Info("Generating a Terminal Pin Key from a Terminal Master Key");
            String commandCode = "A0", mode = "1", keyType = "002", tmk_zmk_flag = "1", keyScheme = "U", tmk = terminalMasterKey, exportingKeyScheme = "X";

            String message = commandCode;
            message += mode;
            message += keyType;
            message += keyScheme + ';';
            message += tmk_zmk_flag;
            message += tmk;
            message += exportingKeyScheme;

            logger.Info("Message to HSM :" + message);
            return SendMessage(message);
          
        }

        public Dictionary<string, string> GenerateMACKeys(string tmk)
        {
            Dictionary<String, String> responseTPK = new Dictionary<string, string>();
            String response = BuildCommandTAK_MAC(tmk);

            String errorCode = response.Substring(8, 2);

            responseTPK.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            responseTPK.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            responseTPK.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                responseTPK.Add("TAK", response.Substring(10, 33));
                logger.Debug("TAK: " + response.Substring(10, 33));

                responseTPK.Add("TAK_LMK", response.Substring(43, 33));
                logger.Debug("TAK_LMK: " + response.Substring(43, 33));

                responseTPK.Add("TAK_Check", response.Substring(76, 6));
                logger.Debug("TAK_Check: " + response.Substring(76, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return responseTPK;
        }
        private string BuildCommandTAK_MAC(string terminalMasterKey)
        {
            logger.Info("Generating TAK_MAC");
            String commandCode = "A0", mode = "1",  keyType = "003", tmk_zmk_flag="1", keyScheme = "U", tmk=terminalMasterKey, exportingKeyScheme="X";

            String message = commandCode;
            message += mode;
            message += keyType;
            message += keyScheme + ";";
            message += tmk_zmk_flag;
            message += tmk;
            message += exportingKeyScheme;

            logger.Info("Message to HSM :" + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> TranslateKeyScheme(string keyType, string key, string toScheme )
        {
            Dictionary<String, String> translateKeyScheme = new Dictionary<string, string>();
            String response = BuildCommandTranslateKey(keyType, key, toScheme);

            String errorCode = response.Substring(8, 2);

            translateKeyScheme.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            translateKeyScheme.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            translateKeyScheme.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                translateKeyScheme.Add("Key", response.Substring(10, 33));
                logger.Debug("Key: " + response.Substring(10, 33));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return translateKeyScheme;
        }
        private string BuildCommandTranslateKey(string key_type, string _key, string toScheme)
        {
            logger.Info("Generating Translate Key");
            String commandCode = "B0", keyType = key_type, key = _key, keyScheme = toScheme;

            String message = commandCode;
            message += keyType;
            message += key;
            message += keyScheme;
            logger.Info("Message to HSM :" + message);
            return SendMessage(message);
        }

        public Dictionary<string, string> TranslatePIN_TDES(string terminalPinKey, string pinEncryptionKey, string pinBlock, string accountNumber)
        {
            Dictionary<String, String> Translate_pin_tdes_response = new Dictionary<string, string>();
            String response = BuildCommandTPKPinBlock(terminalPinKey, pinEncryptionKey, pinBlock, accountNumber);

            String errorCode = response.Substring(8, 2);

            Translate_pin_tdes_response.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            Translate_pin_tdes_response.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            Translate_pin_tdes_response.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                Translate_pin_tdes_response.Add("DestPIN", response.Substring(10, 16));
                logger.Debug("DestPIN: " + response.Substring(10, 16));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return Translate_pin_tdes_response;
        }
        private string BuildCommandTPKPinBlock(string terminalPinKey, string pinEncryptionKey, string pin_block, string accountNumber)
        {
            logger.Info("Generating TPK Pin Block");
            String commandCode = "D4", ktp = terminalPinKey, kpe = pinEncryptionKey, pinBlock = pin_block, pan = accountNumber;

            String message = commandCode;
            message += ktp;
            message += kpe;
            message += pinBlock;
            message += pan;
            logger.Info("Message to HSM :" + message);
            return SendMessage(message);
        }

        public Dictionary<string, string> TranslatePIN_TDES_CA(string terminalPinKey, string pinEncryptionKey, string pinBlock, string accountNumber)
        {
            Dictionary<String, String> Translate_pin_teds__ca_response = new Dictionary<string, string>();
            String response = BuildCommandTPKPinBlock_CA(terminalPinKey, pinEncryptionKey, pinBlock, accountNumber);

            Translate_pin_teds__ca_response.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            Translate_pin_teds__ca_response.Add("Status", response.Substring(6, 2));
            logger.Debug("Status: " + response.Substring(6, 2));
            
            if (response.Substring(6, 2) == "00")
            {
                Translate_pin_teds__ca_response.Add("DestPIN", response.Substring(8, 16));
                logger.Debug("DestPIN: " + response.Substring(8, 16));
            }
            else
            {
                logger.Error("STATUS: " + response.Substring(6, 2));
            }
            return Translate_pin_teds__ca_response;
        }
        private string BuildCommandTPKPinBlock_CA(string terminalPinKey, string pinEncryptionKey, string pin_block, string accountNumber)
        {
            logger.Info("Generating TPK Pin Block CA");
            String commandCode = "CA", ktp = terminalPinKey, kpe = pinEncryptionKey, pinLength = "12", pinBlock = pin_block, pinBlockFormat = "0101", pan = accountNumber;

            String message = commandCode;
            message += ktp;
            message += kpe;
            message += pinLength;
            message += pinBlock;
            message += pinBlockFormat;
            message += pan;

            logger.Info("Message to HSM :" + message);
            return SendMessage(message);
        }

    }
}
