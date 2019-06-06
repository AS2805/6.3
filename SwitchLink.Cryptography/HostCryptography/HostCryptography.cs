using System;
using System.Collections.Generic;
using log4net;


namespace SwitchLink.Cryptography.HostCryptography
{
    public class HostCryptography:Cryptography
    {
        log4net.ILog logger = LogManager.GetLogger(typeof(HostCryptography));

        public Dictionary<string, string> Generate_KEKr_Validation_Response(string kekr, string krs)
        {
            Dictionary<String, String> kekr_validation_response = new Dictionary<string, string>();
            String response = BuildKekrValidationResponse(kekr, krs);
            String errorCode = response.Substring(8, 2);

            kekr_validation_response.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            kekr_validation_response.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            kekr_validation_response.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                kekr_validation_response.Add("KRr", response.Substring(10));
                logger.Debug("KRr: " + response.Substring(10));


            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }
            return kekr_validation_response;
        }
        private string BuildKekrValidationResponse(string _kekr, string _krs)
        {
            logger.Info("Generating message to build KEKr validation response");
            String commandCode = "E2", kekr = _kekr, krs = _krs;

            String message = commandCode;
            message += kekr;
            message += krs;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> Generate_KEKs_Validation_Request(string keks)
        {
            Dictionary<String, String> keks_validation_request= new Dictionary<string, string>();
            String response = BuildKeksValidationRequest(keks);
            String errorCode = response.Substring(8, 2);

            keks_validation_request.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            keks_validation_request.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            keks_validation_request.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                keks_validation_request.Add("KRs", response.Substring(10,16));
                logger.Debug("KRs: " + response.Substring(10,16));

                keks_validation_request.Add("KRr", response.Substring(26,16));
                logger.Debug("KRr: " + response.Substring(26,16));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }

            return keks_validation_request;
        }
        private string BuildKeksValidationRequest(string _keks)
        {
            logger.Info("Generating message to build KEKs validation request");
            String commandCode = "E0", keks = _keks;

            String message = commandCode;
            message += keks;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public string VerifyMAC(string mac, string message, string length, string key)
        {
            //Dictionary<String, String> keks_validation_request = new Dictionary<string, string>();
            String response = GetMacInfo(mac, message, length, key);
            return response;
       }
        private string GetMacInfo(string mac, string msg, string length, string key)
        {
            logger.Info("Generating message to get MAC Info");

            String message = "C40320";
            message += key;
            message += mac;
            message += length;
            message += msg;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> GenerateRandomNumber()
        {
            
            Dictionary<String, String> randomNumber = new Dictionary<string, string>();
            String response = GetRandomNumber();
            String errorCode = response.Substring(8, 2);

            randomNumber.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            randomNumber.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            randomNumber.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                randomNumber.Add("Random Number:", response.Substring(10,16));
                logger.Debug("Random Number: " + response.Substring(10,16));
             }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }

            return randomNumber;
        }
        private string GetRandomNumber()
        {
            logger.Info("Generating message to get random number");

            string commandCode = "C6";
            String message = commandCode;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> GenerateSetOfZoneKeys(string keks)
        {
            Dictionary<String, String> zoneKeys = new Dictionary<string, string>();
            String response = GetSetOfZoneKeys(keks);
            String errorCode = response.Substring(8, 2);

            zoneKeys.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            zoneKeys.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            zoneKeys.Add("ErrorCode", errorCode);
             logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                zoneKeys.Add("ZPK(LMK)", response.Substring(10, 33));
                logger.Debug("ZPK(LMK) :" + response.Substring(10, 33));

                zoneKeys.Add("ZPK(ZMK)", response.Substring(43,33));
                logger.Debug("ZPK(ZMK) :" + response.Substring(43,33));

                zoneKeys.Add("ZPK Check Value", response.Substring(76,6));
                logger.Debug("ZPK Check Value :" + response.Substring(76,6));

                zoneKeys.Add("ZAK(LMK)", response.Substring(82,33));
                logger.Debug("ZAK(LMK) :" + response.Substring(82,33));

                zoneKeys.Add("ZAK(ZMK)", response.Substring(115,33));
                logger.Debug("ZAK(ZMK) :" + response.Substring(115,33));

                zoneKeys.Add("ZAK Check Value", response.Substring(148,6));
                logger.Debug("ZAK Check Value :" + response.Substring(148,6));

                zoneKeys.Add("ZEK(LMK)", response.Substring(154,33));
                logger.Debug("ZEK(LMK) :" + response.Substring(154,33));

                zoneKeys.Add("ZEK(ZMK)", response.Substring(187,33));
                logger.Debug("ZEK(ZMK) :" + response.Substring(187,33));

                zoneKeys.Add("ZEK Check Value", response.Substring(220, 6));
                logger.Debug("ZEK Check Value :" + response.Substring(220, 6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }

            return zoneKeys;
        }
        private string GetSetOfZoneKeys(string _keks)
        {
            logger.Info("Generating message to get set of zone keys");
            String commandCode = "OI", keks = _keks;

            String message = commandCode;
            message += keks;
            message += ";HU1;1";

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> TranslateSetOfZoneKeys(string kekr, string zpk, string zak, string zek)
        {
            Dictionary<String, String> translatedZoneKeys = new Dictionary<string, string>();
            String response = GetTranslatedSetOfZoneKeys(kekr, zpk, zak, zek);
            String errorCode = response.Substring(8, 2);

            translatedZoneKeys.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            translatedZoneKeys.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            translatedZoneKeys.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                translatedZoneKeys.Add("KCV Processing Flag", response.Substring(10, 1));
                logger.Debug("KCV Processing Flag :" + response.Substring(10, 1));

                translatedZoneKeys.Add("ZPK(LMK)", response.Substring(11,33));
                logger.Debug("ZPK(LMK) :" + response.Substring(11,33));

                translatedZoneKeys.Add("ZPK Check Value", response.Substring(44,6));
                logger.Debug("ZPK Check Value :" + response.Substring(44,6));

                translatedZoneKeys.Add("ZAK(LMK)", response.Substring(50,33));
                logger.Debug("ZAK(LMK) :" + response.Substring(50,33));
               
                translatedZoneKeys.Add("ZAK Check Value", response.Substring(83,6));
                logger.Debug("ZAK Check Value :" + response.Substring(83,6));

                translatedZoneKeys.Add("ZEK(LMK)", response.Substring(89,33));
                logger.Debug("ZEK(LMK) :" + response.Substring(89,33));
              
                translatedZoneKeys.Add("ZEK Check Value", response.Substring(122,6));
                logger.Debug("ZEK Check Value :" + response.Substring(122,6));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }

            return translatedZoneKeys;
        }
        private string GetTranslatedSetOfZoneKeys(string _kekr, string _zpk, string _zak, string _zek)
        {
            logger.Info("Generating message to get translated set of zone keys");
            String commandCode = "OK", kekr = _kekr, kvcProcessingFlag = "2", zpkFlag = "1", zpk = "H" + _zpk, zakFlag = "1", zak = "H" + _zak, zekFlag = "0", zek = "H" + "11111111111111111111111111111111";

            String message = commandCode;
            message += kekr;
            message += kvcProcessingFlag;
            message += zpkFlag;
            message += zpk;
            message += zakFlag;
            message += zak;
            message += zekFlag;
            message += zek;
            message += ";HU1";

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }

        public Dictionary<string, string> CalculateMAC_ZAK(string message, string macKey)
        {
            Dictionary<String, String> responseMAC = new Dictionary<string, string>();

            String response = GenerateMAC(message,macKey);

            String errorCode = response.Substring(8, 2);

            responseMAC.Add("Header", response.Substring(2, 4));
            logger.Debug("Header: " + response.Substring(2, 4));

            responseMAC.Add("ResponseCode", response.Substring(6, 2));
            logger.Debug("ResponseCode: " + response.Substring(6, 2));

            responseMAC.Add("ErrorCode", errorCode);
            logger.Debug("ErrorCode: " + errorCode);

            if (errorCode == "00")
            {
                responseMAC.Add("MAC :", response.Substring(10));
                logger.Debug("MAC :" + response.Substring(10));
            }
            else
            {
                logger.Error("ERROR CODE: " + errorCode);
            }

            return responseMAC;
        }
        private string GenerateMAC(string msg , string macKey)
        {
            logger.Info("Generating message to get MAC");

            string len = msg.Length.ToString("X4");

            String messageBlock=msg, commandCode = "C2", blockNo = "0", macKeyType = "3", macGenerationMode = "3", messageType = "0", key = macKey, messageLength = len;

            String message = commandCode;
            message += blockNo;
            message += macKeyType;
            message += macGenerationMode;
            message += messageType;
            message += key;
            message += messageLength;
            message += messageBlock;

            logger.Info("Message to HSM : " + message);

            return SendMessage(message);
        }
    }
}
