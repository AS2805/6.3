using System;
using SwitchLink.Data;
using SwitchLink.Data.Models;
using SwitchLink.ProtocalFactory.TritonNode.Models;

namespace SwitchLink.TritonNode.Services
{
    interface ILoggerService
    {
        void LogTritonAuthorizationRequest(TransactionModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonConfigRequest(ConfigModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonHostTotalRequest(HostTotalModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonReversalRequest(ReversalModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);

        void LogTritonAuthorizationResponse(TransactionModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonConfigResponse(ConfigModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonHostTotalResponse(HostTotalModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);
        void LogTritonReversalResponse(ReversalModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp);

        void LogTritonDayTotal(HostTotalModel hostTotalModel, string atmId);
    }

    class LoggerService : ILoggerService
    {
        private decimal _transactionAmount;

        public void LogTritonAuthorizationRequest(TransactionModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            _transactionAmount = atm.Amount1 / 100m;

            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Authorization",
                tran_type = "Request",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                transaction_amount = _transactionAmount,
                surcharge_amount = atm.Amount2 / 100m,
                miscellaneous_1 = atm.Miscellaneous1,
                miscellaneous_2 = atm.Miscellaneous2,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                track2 = ReplaceTrack2(atm.Track2),
                pin_block = atm.PinBlock,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text,
                terminal_tran_seq = atm.AtmSeqNo
            };

            Log(logModel);
        }

        public void LogTritonConfigRequest(ConfigModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Config",
                tran_type = "Request",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                surcharge_amount = atm.Amount2,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text
            };

            Log(logModel);
        }

        public void LogTritonHostTotalRequest(HostTotalModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "HostTotal",
                tran_type = "Request",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text
            };

            Log(logModel);
        }

        public void LogTritonReversalRequest(ReversalModel atm, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            _transactionAmount = ((atm.AuthorizationTranAmount + atm.AuthorizationSurAmount) - atm.AuthorizationDispensedAmount)/ 100m;

            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Reversal",
                tran_type = "Request",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                transaction_amount = _transactionAmount,
                miscellaneous_1 = atm.Miscellaneous1,
                miscellaneous_2 = atm.Miscellaneous2,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                track2 = ReplaceTrack2(atm.Track2),
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text,
                terminal_tran_seq = atm.AtmSeqNo
            };

            Log(logModel);
        }

        public void LogTritonAuthorizationResponse(TransactionModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Authorization",
                tran_type = "Response",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                transaction_amount = _transactionAmount,
                surcharge_amount = atm.Amount2 / 100m,
                account_balance = atm.Balance / 100m,
                miscellaneous_1 = atm.Miscellaneous1,
                miscellaneous_2 = atm.Miscellaneous2,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                response_code = atm.AuthorizationCode,
                dtResponseSent = responseSent.ToString("yyyy-MM-dd HH:mm:ss"),
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                dtResponseFromCore = responseFromCoreNode.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                track2 = ReplaceTrack2(atm.Track2),
                pin_block = atm.PinBlock,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                auth_no = atm.AuthorizationNum,
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text,
                terminal_tran_seq = atm.AtmSeqNo
            };
            Log(logModel);

            //if (atm.AuthorizationCode == "00")
            {
                Transaction_Completed logCompleted = new Transaction_Completed
                {
                    tran_gid = atmId,
                    tran_ip = atmIp,
                    switch_no = 1,
                    request_timestamp = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                    request_type = "Authorization",
                    terminal_id = atm.TerminalId,
                    transaction_seq_no = atm.TranSeqNo,
                    card_no = ReplaceCardNo(atm.Track2),
                    transaction_amount = _transactionAmount,
                    surcharge_amount = atm.Amount2 / 100m,
                    account_balance = atm.Balance / 100m,
                    response_code = atm.AuthorizationCode,
                    auth_no = atm.AuthorizationNum,
                    tran_date = takenByCore.ToString("yyyy-MM-dd"),
                    tran_time = takenByCore.ToString("HH:mm:ss"),
                    transaction_code = atm.TransactionCode,
                    transaction_speed = (responseFromCoreNode - takenByCore).ToString("g"),
                    response_desc = atm.AuthorizationDesc,
                    response_action = atm.AuthorizationAction,
                    status_monitoring = atm.StatusMonitoringField,
                    terminal_tran_seq = atm.AtmSeqNo
                };

                Log(logCompleted);
            }
        }

        public void LogTritonConfigResponse(ConfigModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Config",
                tran_type = "Response",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                dtResponseSent = responseSent.ToString("yyyy-MM-dd HH:mm:ss"),
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                dtResponseFromCore = responseFromCoreNode.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text
            };

            Log(logModel);
        }

        public void LogTritonHostTotalResponse(HostTotalModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "HostTotal",
                tran_type = "Response",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                dtResponseSent = responseSent.ToString("yyyy-MM-dd HH:mm:ss"),
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                dtResponseFromCore = responseFromCoreNode.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                text = atm.Text
            };

            Log(logModel);
        }

        public void LogTritonReversalResponse(ReversalModel atm, DateTime responseSent, DateTime responseFromCoreNode, DateTime atmConnected, DateTime takenByCore, string atmId, string atmIp)
        {
            TritonNode_Transaction logModel = new TritonNode_Transaction
            {
                req_type = "Reversal",
                tran_type = "Response",
                communications_identifier = atm.CommunicationIdentifier,
                terminal_identifier = atm.TerminalIdentifier,
                software_version_number = atm.SoftwareVerionNo,
                encryption_mode_flag = atm.EncryptionModeFlag,
                information_header = atm.InformationHeader,
                terminal_id = atm.TerminalId,
                transaction_code = atm.TransactionCode,
                transaction_seq_no = atm.TranSeqNo,
                transaction_amount = _transactionAmount,
                //dispense_amount = _actualDispenseAmount,
                //surcharge_amount = _surchargeAmount,
                miscellaneous_1 = atm.Miscellaneous1,
                miscellaneous_2 = atm.Miscellaneous2,
                status_monitoring = atm.StatusMonitoringField,
                miscellaneous_X = atm.MiscellaneousX,
                response_code = atm.AuthorizationCode,
                dtResponseSent = responseSent.ToString("yyyy-MM-dd HH:mm:ss"),
                dtTakenByCore = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                dtconnected = atmConnected.ToString("yyyy-MM-dd HH:mm:ss"),
                dtResponseFromCore = responseFromCoreNode.ToString("yyyy-MM-dd HH:mm:ss"),
                tran_gid = atmId,
                track2 = ReplaceTrack2(atm.Track2),
                tran_date = takenByCore.ToString("yyyy-MM-dd"),
                tran_time = takenByCore.ToString("HH:mm:ss"),
                switch_no = 1,
                tran_ip = atmIp,
                auth_no = atm.AuthorizationNum,
                text = atm.Text,
                terminal_tran_seq = atm.AtmSeqNo
            };

            Log(logModel);

            //if (atm.AuthorizationCode == "00")
            {
                Transaction_Completed logCompleted = new Transaction_Completed
                {
                    tran_gid = atmId,
                    tran_ip = atmIp,
                    switch_no = 1,
                    request_timestamp = takenByCore.ToString("yyyy-MM-dd HH:mm:ss"),
                    request_type = "Reversal",
                    terminal_id = atm.TerminalId,
                    transaction_seq_no = atm.TranSeqNo,                    
                    card_no = ReplaceCardNo(atm.Track2),
                    transaction_amount = _transactionAmount,
                    //surcharge_amount = _surchargeAmount,
                    //dispense_amount = _actualDispenseAmount,
                    response_code = atm.AuthorizationCode,
                    tran_date = takenByCore.ToString("yyyy-MM-dd"),
                    tran_time = takenByCore.ToString("HH:mm:ss"),
                    transaction_code = atm.TransactionCode,
                    transaction_speed = (responseFromCoreNode - takenByCore).ToString("g"),
                    auth_no = atm.AuthorizationNum,
                    response_desc = atm.AuthorizationDesc,
                    response_action = atm.AuthorizationAction,
                    status_monitoring = atm.StatusMonitoringField,
                    terminal_tran_seq = atm.AtmSeqNo
                };

                Log(logCompleted);
            }
        }

        public void LogTritonDayTotal(HostTotalModel atm, string atmId)
        {
            Terminals_Day_Totals dayTotals = new Terminals_Day_Totals
            {
                tran_gid = atmId,
                terminal_id = atm.TerminalId,
                business_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                settlement_code = atm.TransactionCode,
                processed_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                num_of_cw = atm.NoOfWithdrawals,
                num_of_tf = atm.NoOfTransfers,
                num_of_bi = atm.NoOfInquiries,
                total_dispensed = atm.Settlement / 100m
            };

            Log(dayTotals);
        }

        private void Log(Transaction_Completed model)
        {
            using (TransactionData context = new TransactionData())
            {
                context.InsertTransactionCompletedRecord(model);
            }
        }

        private void Log(TritonNode_Transaction model)
        {
            using (TerminalData context = new TerminalData())
            {
                context.InsertTritonLogRecord(model);
            }
        }

        private void Log(Terminals_Day_Totals model)
        {
            using (TerminalData context = new TerminalData())
            {
                context.InsertTritonDayTotals(model);
            }
        }

        private string ReplaceCardNo(string str)
        {
            string accNumber = str.Substring(0, str.IndexOf("=", StringComparison.Ordinal));
            return accNumber.Substring(0, 4) + "********" + accNumber.Substring(accNumber.Length - 4, 4);
        }

        private string ReplaceTrack2(string str)
        {
            string accNumber = str.Substring(0, str.IndexOf("=", StringComparison.Ordinal));
            return accNumber.Substring(0, 4) + "********" + accNumber.Substring(accNumber.Length - 4, 4) + str.Substring(str.IndexOf("=", StringComparison.Ordinal), 16);
        }

        //private string RandomCardNo()
        //{
        //    string result = "4";
        //    if (new Random().Next(3) == 2)
        //        result = "5";
        //    result += new Random().Next(0, 99999999);
        //    result += new Random().Next(0, 9999999);
        //    return result;
        //}
    }
}