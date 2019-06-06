using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SwitchLink.Cryptography;

namespace Switchlink.Diagnostics
{

    namespace Switchlink.Diagnostics
    {
        public class Diagnostics : Cryptography
        {
            //private log4net.ILog logger = LogManager.GetLogger(typeof (Diagnostics));

            log4net.ILog logger = LogManager.GetLogger("DiagnosticLogger");

            public Dictionary<string, string> PerformDiagnostics()
            {
                Dictionary<String, String> repsonseDiagnostics = new Dictionary<string, string>();
                String response = GetDiagnosticsResponse();
                String errorCode = response.Substring(8, 2);

                repsonseDiagnostics.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                repsonseDiagnostics.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));

                repsonseDiagnostics.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    repsonseDiagnostics.Add("LMK_CHECK", response.Substring(10, 16));
                    logger.Debug("LMK_CHECK: " + response.Substring(10, 16));

                    repsonseDiagnostics.Add("FIRMWARE", response.Substring(26, 9));
                    logger.Debug("FIRMWARE: " + response.Substring(26, 9));
                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return repsonseDiagnostics;
            }
            private string GetDiagnosticsResponse()
            {
                logger.Info("Generating diagnostic message to send");
                String commandCode = "NC"; //, lmkIdentifier = "00";

                String message = commandCode;
                // message +=lmkIdentifier;


                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

            public Dictionary<string, string> NetworkInformation()
            {
                Dictionary<String, String> responseNetworkInformation = new Dictionary<string, string>();
                String response = GetNetworkInformation();
                String errorCode = response.Substring(8, 2);

                responseNetworkInformation.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                responseNetworkInformation.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));


                responseNetworkInformation.Add("NumberOfRecords", response.Substring(10, 4));
                logger.Debug("NumberOfRecords : " + response.Substring(10, 4));

                responseNetworkInformation.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    int start = 14;

                    for (int i = 1; i <= Convert.ToInt32(response.Substring(10, 4)); i++)
                    {
                        responseNetworkInformation.Add("RECORD PROTOCOL " + i, response.Substring(start, 1));
                        logger.Debug("RECORD PROTOCOL " + i + ": " + response.Substring(start, 1));
                        start = start++;

                        responseNetworkInformation.Add("RECORD LOCAL PORT " + i, response.Substring(start, 4));
                        logger.Debug("RECORD LOCAL PORT " + i + ": " + response.Substring(start, 4));
                        start = start + 4;

                        responseNetworkInformation.Add("RECORD REMOTE ADDRESS " + i, response.Substring(start, 8));
                        logger.Debug("RECORD REMOTE ADDRESS " + i + ": " + response.Substring(start, 8));
                        start = start + 8;

                        responseNetworkInformation.Add("RECORD REMOTE PORT " + i, response.Substring(start, 4));
                        logger.Debug("RECORD REMOTE PORT " + i + ": " + response.Substring(start, 4));
                        start = start + 4;

                        responseNetworkInformation.Add("RECORD STATE " + i, response.Substring(start, 1));
                        logger.Debug("RECORD STATE " + i + ": " + response.Substring(start, 1));
                        start = start++;

                        responseNetworkInformation.Add("RECORD DURATION " + i, response.Substring(start, 8));
                        logger.Debug("RECORD DURATION " + i + ": " + response.Substring(start, 8));
                        start = start + 8;
                    }

                    responseNetworkInformation.Add("TOTAL BYTES SENT", response.Substring(start, 16));
                    logger.Debug("TOTAL BYTES SENT : " + response.Substring(start, 16));

                    responseNetworkInformation.Add("TOTAL BYTES RECEIVED", response.Substring(start, 16));
                    logger.Debug("TOTAL BYTES RECEIVED : " + response.Substring(start, 16));

                    responseNetworkInformation.Add("TOTAL UNICAST PACKET SENT", response.Substring(start, 8));
                    logger.Debug("TOTAL UNICAST PACKET SENT : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL UNICAST PACKETS RECEIVED", response.Substring(start, 8));
                    logger.Debug("TOTAL UNICAST PACKETS RECEIVED : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL NON-UNICAST PACKET SENT", response.Substring(start, 8));
                    logger.Debug("TOTAL NON-UNICAST PACKET SENT : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL NON-UNICAST PACKETS RECEIVED", response.Substring(start, 8));
                    logger.Debug("TOTAL NON-UNICAST PACKETS RECEIVED : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL PACKETS DISCARDED DURING SEND", response.Substring(start, 8));
                    logger.Debug("TOTAL PACKETS DISCARDED DURING SEND : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL PACKETS DISCARDED DURING RECEIEVE", response.Substring(start, 8));
                    logger.Debug("TOTAL PACKETS DISCARDED DURING RECEIEVE : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL ERRORS DURING SEND", response.Substring(start, 8));
                    logger.Debug("TOTAL ERRORS DURING SEND : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL ERRORS DURING RECEIEVE", response.Substring(start, 8));
                    logger.Debug("TOTAL ERRORS DURING RECEIEVE : " + response.Substring(start, 8));

                    responseNetworkInformation.Add("TOTAL UNKNOWN PACKETS", response.Substring(start, 8));
                    logger.Debug("TOTAL UNKNOWN PACKETS : " + response.Substring(start, 8));

                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return responseNetworkInformation;
            }
            private string GetNetworkInformation()
            {
                logger.Info("Generating message to send to get network information");
                String commandCode = "NI", networkInterface = "X", ethernetStastics = "1";

                String message = commandCode;
                message += networkInterface;
                message += ethernetStastics;


                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

            public Dictionary<string, string> HostCommandVolumes()
            {
                Dictionary<String, String> responseHostCommandVolume = new Dictionary<string, string>();
                String response = GetHostCommandVolumes();

                String errorCode = response.Substring(8, 2);

                responseHostCommandVolume.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                responseHostCommandVolume.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));

                responseHostCommandVolume.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    responseHostCommandVolume.Add("SERIAL NUMBER", response.Substring(10, 12));
                    logger.Debug("SERIAL NUMBER: " + response.Substring(10, 12));

                    responseHostCommandVolume.Add("START DATE", response.Substring(22, 6));
                    logger.Debug("START DATE: " + response.Substring(22, 6));

                    responseHostCommandVolume.Add("START TIME", response.Substring(28, 6));
                    logger.Debug("START TIME: " + response.Substring(28, 6));

                    responseHostCommandVolume.Add("END DATE", response.Substring(34, 6));
                    logger.Debug("END DATE: " + response.Substring(34, 6));

                    responseHostCommandVolume.Add("END TIME", response.Substring(40, 6));
                    logger.Debug("END TIME: " + response.Substring(40, 6));

                    responseHostCommandVolume.Add("CURRENT DATE", response.Substring(46, 6));
                    logger.Debug("CURRENT DATE: " + response.Substring(46, 6));

                    responseHostCommandVolume.Add("CURRENT TIME", response.Substring(52, 6));
                    logger.Debug("CURRENT TIME: " + response.Substring(52, 6));

                    responseHostCommandVolume.Add("SECONDS", response.Substring(58, 10));
                    logger.Debug("SECONDS: " + response.Substring(58, 10));

                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return responseHostCommandVolume;
            }
            private string GetHostCommandVolumes()
            {
                logger.Info("Generating message to send to get host command volumes");
                String commandCode = "J4";

                String message = commandCode;

                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

            public Dictionary<string, string> HealthCheckAccumulatedCounts()
            {
                Dictionary<String, String> responseHealthCheckAccumulatedCounts = new Dictionary<string, string>();
                String response = GetHealthCheckAccumulatedCounts();

                String errorCode = response.Substring(8, 2);

                responseHealthCheckAccumulatedCounts.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                responseHealthCheckAccumulatedCounts.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));

                responseHealthCheckAccumulatedCounts.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    responseHealthCheckAccumulatedCounts.Add("SERIAL NUMBER", response.Substring(10, 12));
                    logger.Debug("SERIAL NUMBER: " + response.Substring(10, 12));

                    responseHealthCheckAccumulatedCounts.Add("START DATE", response.Substring(22, 6));
                    logger.Debug("START DATE: " + response.Substring(22, 6));

                    responseHealthCheckAccumulatedCounts.Add("START TIME", response.Substring(28, 6));
                    logger.Debug("START TIME: " + response.Substring(28, 6));

                    responseHealthCheckAccumulatedCounts.Add("END DATE", response.Substring(34, 6));
                    logger.Debug("END DATE: " + response.Substring(34, 6));

                    responseHealthCheckAccumulatedCounts.Add("END TIME", response.Substring(40, 6));
                    logger.Debug("END TIME: " + response.Substring(40, 6));

                    responseHealthCheckAccumulatedCounts.Add("CURRENT DATE", response.Substring(46, 6));
                    logger.Debug("CURRENT DATE: " + response.Substring(46, 6));

                    responseHealthCheckAccumulatedCounts.Add("CURRENT TIME", response.Substring(52, 6));
                    logger.Debug("CURRENT TIME: " + response.Substring(52, 6));

                    responseHealthCheckAccumulatedCounts.Add("REBOOTS", response.Substring(58, 10));
                    logger.Debug("REBOOTS: " + response.Substring(58, 10));

                    responseHealthCheckAccumulatedCounts.Add("TAMPERS", response.Substring(68, 10));
                    logger.Debug("TAMPERS: " + response.Substring(68, 10));

                    responseHealthCheckAccumulatedCounts.Add("PIN_VERIFIES/MIN", response.Substring(78, 7));
                    logger.Debug("PIN_VERIFIES/MIN: " + response.Substring(78, 7));

                    responseHealthCheckAccumulatedCounts.Add("PIN_VERIFIES/HOUR", response.Substring(85, 5));
                    logger.Debug("PIN_VERIFIES/HOUR: " + response.Substring(85, 5));

                    responseHealthCheckAccumulatedCounts.Add("PIN ATTACKS", response.Substring(90, 8));
                    logger.Debug("PIN ATTACKS: " + response.Substring(90, 8));
                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return responseHealthCheckAccumulatedCounts;
            }
            private string GetHealthCheckAccumulatedCounts()
            {
                logger.Info("Generating message to send to get health check accumulated counts");
                String commandCode = "J8";

                String message = commandCode;

                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

            public Dictionary<string, string> ResetHealthCheckAccumulatedCounts()
            {
                Dictionary<String, String> responseResetHealthCheckAccumulatedCounts = new Dictionary<string, string>();
                String response = GetResetHealthResponse();

                String errorCode = response.Substring(8, 2);

                responseResetHealthCheckAccumulatedCounts.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                responseResetHealthCheckAccumulatedCounts.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));

                responseResetHealthCheckAccumulatedCounts.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    responseResetHealthCheckAccumulatedCounts.Add("HEALTH CHECK COUNT SUCCESSFULLY RESET", errorCode);
                    logger.Debug("HEALTH CHECK COUNT SUCCESSFULLY RESET: " + errorCode);
                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return responseResetHealthCheckAccumulatedCounts;
            }
            private string GetResetHealthResponse()
            {
                logger.Info("Generating message to send to reset health check accumulated counts");
                String commandCode = "JI";

                String message = commandCode;

                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

            public Dictionary<string, string> HealthCheckStatus()
            {
                Dictionary<String, String> responseHealthCheckStatus = new Dictionary<string, string>();
                String response = GetHealthCheckStatus();

                String errorCode = response.Substring(8, 2);

                responseHealthCheckStatus.Add("Header", response.Substring(2, 4));
                logger.Debug("Header: " + response.Substring(2, 4));

                responseHealthCheckStatus.Add("ResponseCode", response.Substring(6, 2));
                logger.Debug("ResponseCode: " + response.Substring(6, 2));

                responseHealthCheckStatus.Add("ErrorCode", errorCode);
                logger.Debug("ErrorCode: " + errorCode);

                if (errorCode == "00")
                {
                    responseHealthCheckStatus.Add("SERIAL NUMBER", response.Substring(10, 12));
                    logger.Debug("SERIAL NUMBER : " + response.Substring(10, 12));

                    responseHealthCheckStatus.Add("SYSTEM DATE", response.Substring(22, 6));
                    logger.Debug("SYSTEM DATE : " + response.Substring(22, 6));

                    responseHealthCheckStatus.Add("SYSTEM TIME", response.Substring(28, 6));
                    logger.Debug("SYSTEM TIME : " + response.Substring(28, 6));

                    responseHealthCheckStatus.Add("CONSOLE STATE", response.Substring(34, 1));
                    logger.Debug("CONSOLE STATE : " + response.Substring(34, 1));

                    responseHealthCheckStatus.Add("HSM MANAGER STATE", response.Substring(35, 1));
                    logger.Debug("HSM MANAGER STATE : " + response.Substring(35, 1));

                    responseHealthCheckStatus.Add("ETHERNET HOST LINK 1 STATE", response.Substring(36, 1));
                    logger.Debug("ETHERNET HOST LINK 1 STATE : " + response.Substring(36, 1));

                    responseHealthCheckStatus.Add("ETHERNET HOST LINK 2 STATE", response.Substring(37, 1));
                    logger.Debug("ETHERNET HOST LINK 2 STATE : " + response.Substring(37, 1));

                    responseHealthCheckStatus.Add("ASYNC LINK STATE", response.Substring(38, 1));
                    logger.Debug("ASYNC LINK STATE : " + response.Substring(38, 1));

                    responseHealthCheckStatus.Add("FICON LINK STATE", response.Substring(39, 1));
                    logger.Debug("FICON LINK STATE : " + response.Substring(39, 1));

                    responseHealthCheckStatus.Add("TAMPER STATE", response.Substring(40, 1));
                    logger.Debug("TAMPER STATE : " + response.Substring(40, 1));

                    responseHealthCheckStatus.Add("TOTAL LMK'S LOADED", response.Substring(41, 2));
                    logger.Debug("TOTAL LMK'S LOADED : " + response.Substring(41, 2));

                    responseHealthCheckStatus.Add("TOTAL TEST LMK'S LAODED", response.Substring(43, 2));
                    logger.Debug("TOTAL TEST LMK'S LOADED : " + response.Substring(43, 2));

                    responseHealthCheckStatus.Add("TOTAL OLD TEST LMK'S LAODED", response.Substring(45, 2));
                    logger.Debug("TOTAL OLD TEST LMK'S LAODED : " + response.Substring(45, 2));

                    
                    responseHealthCheckStatus.Add("LMK ID", response.Substring(47, 2));
                    logger.Debug("LMK ID : " + response.Substring(47, 2));

                    responseHealthCheckStatus.Add("AUTHORIZED ", response.Substring(49, 1));
                    logger.Debug("AUTHORIZED : " + response.Substring(49, 1));

                    responseHealthCheckStatus.Add("NUM OF AUTHORIZED ACTIVITIES", response.Substring(50, 2));
                    logger.Debug("NUM OF AUTHORIZED ACTIVITIES : " + response.Substring(50, 2));

                    responseHealthCheckStatus.Add("SCHEME", response.Substring(52, 1));
                    logger.Debug("SCHEME : " + response.Substring(52, 1));

                    responseHealthCheckStatus.Add("ALGORITHM", response.Substring(53, 1));
                    logger.Debug("ALGORITHM " + ": " + response.Substring(53, 1));

                    responseHealthCheckStatus.Add("STATUS", response.Substring(54, 1));
                    logger.Debug("STATUS : " + response.Substring(54, 1));

                    responseHealthCheckStatus.Add("COMMENTS", response.Substring(55, 15));
                    logger.Debug("COMMENTS : " + response.Substring(55, 15));

                    responseHealthCheckStatus.Add("DELIMITER", response.Substring(70));
                    logger.Debug("DELIMITER : " + response.Substring(70, 1));

                    responseHealthCheckStatus.Add("END LMK LIST DELIMITER", response.Substring(71,1));
                    logger.Debug("END LMK LIST DELIMITER : " + response.Substring(71, 1));

                    responseHealthCheckStatus.Add("FRAUD DETECTION EXCEEDED", response.Substring(72, 1));
                    logger.Debug("FRAUD DETECTION EXCEEDED : " + response.Substring(72, 1));

                    responseHealthCheckStatus.Add("PIN ATTACKS EXCEEDED", response.Substring(73, 1));
                    logger.Debug("PIN ATTACKS EXCEEDED : " + response.Substring(73, 1));
                }
                else
                {
                    logger.Error("ERROR CODE: " + errorCode);
                }
                return responseHealthCheckStatus;
            }

            private string GetHealthCheckStatus()
            {
                logger.Info("Generating message to send to get instant health status");
                String commandCode = "JK";

                String message = commandCode;

                logger.Info("Message to HSM : " + message);

                return SendMessage(message);
            }

        }
    }
}