using System;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response;
using SwitchLink.Comm.Interfaces.Services;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_RMI
    {
        private const string Ip = "127.0.0.1";
        private const int Port = 8002;

        [TestMethod]
        public void HostNode()
        {
            using (var client = ScsServiceClientBuilder.CreateClient<IHostNodeRequestService>(new ScsTcpEndPoint(Ip, Port)))
            {
                client.Connect();
                string nameLocation = "800 LANGDON ST,          MADISON      AU";
                string posEntryMode = "021";
                string posCondCode = "41"; // Cash Dispenser Machine (ATM)
                TranHostNodeRequestDto hostReq = new TranHostNodeRequestDto
                {
                    P3ProcessingCode = "11", // from triton
                    P4AmountTran = 12000, // from triton
                    P7TransmitDt = DateTime.Now,
                    //P11Stan = 1,
                    P12TimeLocalTran = DateTime.Now,
                    P13DateLocalTran = DateTime.Now,
                    P15DateSettlement = DateTime.Now.AddDays(+1),
                    P18MerchantType = "5811",
                    P25PosConditionCode = posCondCode,
                    P28AmtTranFee = 200, // from triton
                    P35Track2 = "5188680100002932=15122015076719950000", // from triton  v
                    P41TerminalId = "S9218163", // from triton
                    P43NameLocation = nameLocation,
                    P52PinBlock = "A8FB4E47EACB0FA1", // from triton
                    P57AmountCash = 0 // from triton
                };

                TranHostNodeResponseDto hostRes = client.ServiceProxy.RequestTransaction(hostReq); // Send to HostNode
            }
        }
    }
}
