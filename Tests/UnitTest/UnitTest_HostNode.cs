using BankSimulator.SocketServer.Messaging;
using NUnit.Framework;
using SwitchLink.HostNode.Communication.Messages;
using SwitchLink.HostNode.Communication.Services;
using SwitchLink.ProtocalFactory.HostNode.Helper.Extension;
using UnitTest.AS2805.Helper;

namespace UnitTest
{
    /// <summary>
    /// Summary description for UnitTest_HostNode
    /// </summary>
    [TestFixture]
    public class UnitTest_HostNode
    {
        readonly BaseMessageHelper _baseMessageHelper = new BaseMessageHelper();

        [Test]
        public void TestSignedOn()
        {
            IBankingCoreService svc = new BankingCoreService();
            BaseMessageHandles msg = new BaseMessageHandles();

            string b = "0810822000008201000004000000100000000126105340000001086110001630303031363439444135323239434436424434334403010861100016";
            var result = _baseMessageHelper.StringToBytes(b);


            Assert.IsFalse(svc.IsBankConfig); 
            svc.Connect();
            Assert.IsFalse(svc.IsReady);
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementRequest()));
            Assert.IsFalse(!svc.IsBankConfig); 
            svc.SendRequest();
            svc.SendRequest();
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementResponse(new As2805(result))));
            Assert.IsFalse(!svc.IsReady);
        }

        [Test]
        public void TestKeyExchange()
        {
            IBankingCoreService svc = new BankingCoreService();
            BaseMessageHandles msg = new BaseMessageHandles();

            Assert.IsFalse(svc.IsBankConfig);
            svc.Connect();
            Assert.IsFalse(svc.IsReady);
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementRequest()));
            Assert.IsFalse(!svc.IsBankConfig);
            svc.SendRequest();
            svc.SendRequest();
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementResponse(new As2805(_baseMessageHelper.StringToBytes("0810822000008201000004000000100000000126105340000001086110001630303031363439444135323239434436424434334403010861100016")))));
            Assert.IsFalse(!svc.IsReady);
            svc.HandleResponse(new HostRawMessage(msg.NetworkManagementAdviceRequest()));
            Assert.IsFalse(!svc.IsBankConfig);
            svc.SendRequest();
            svc.SendRequest();
            svc.HandleResponse(new HostRawMessage(msg.NetworkManagementAdviceResponse(new As2805(_baseMessageHelper.StringToBytes("083082200000820108000400000010000000012610563200000308611000163030303132354537323846303334423230000000000000000201010861100016")))));
            Assert.IsFalse(!svc.IsReady);
        }

        [Test]
        public void TestTransaction()
        {
            IBankingCoreService svc = new BankingCoreService();
            BaseMessageHandles msg = new BaseMessageHandles();

            Assert.IsFalse(svc.IsBankConfig);
            svc.Connect();
            Assert.IsFalse(svc.IsReady);
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementRequest()));
            Assert.IsFalse(!svc.IsBankConfig);
            svc.SendRequest();
            svc.SendRequest();
            svc.HandleResponse(new HostRawMessage(msg.CreateNetworkManagementResponse(new As2805(_baseMessageHelper.StringToBytes("0810822000008201000004000000100000000126105340000001086110001630303031363439444135323239434436424434334403010861100016")))));
            Assert.IsFalse(!svc.IsReady);

            svc.HandleResponse(new HostRawMessage(_baseMessageHelper.BuildMessageWithHeaderLength("02103222001182C008810120000000000089000126111702000002012844000002000656025808611000163030533931313131313134333735383630303020202020202000000000000000010000000089009550FEC000000000")));
            var result = svc.GetCoreResponse();
            Assert.IsFalse(!svc.IsReady);
        }

    }
}
