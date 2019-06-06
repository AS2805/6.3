using System;
using System.IO;
using Hik.Communication.ScsServices.Client;
using NUnit.Framework;
using Rhino.Mocks;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.TritonNode.Communication.Messages;
using SwitchLink.TritonNode.Communication.Services;
using UnitTest.Helper;

namespace UnitTest
{
    [TestFixture]
    public class UnitTest_TritonNode
    {
        readonly AtmRequestHelper _atmRequestHelper = new AtmRequestHelper();

        [Test]
        public void TestOnAtmConnect()
        {
            var mockCoreComm = MockRepository.GenerateMock<IScsServiceClient<ICoreNodeRequestService>>();
            mockCoreComm.Expect(x => x.ServiceProxy.RequestTransaction(null)).Return(new TranCoreNodeResponseDto());
            
            IAtmCoreService atmSvc = new AtmCoreService(new CoreCommunicationService(mockCoreComm));

            Assert.IsFalse(atmSvc.IsConnected);
            atmSvc.Connect(DateTime.Now, "", "");
            Assert.IsFalse(!atmSvc.IsConnected);
            Assert.IsFalse(atmSvc.RetryResponse);
            var res = atmSvc.GetAtmResponse();
            Assert.AreEqual(res.RawBytes, new byte[] { _atmRequestHelper.Enq });
            atmSvc.ResponseSent();
            Assert.IsFalse(!atmSvc.IsConnected);
        }

        [Test]
        public void TestOnAtmSendRequestWithWrongFormat()
        {
            var mockCoreComm = MockRepository.GenerateMock<IScsServiceClient<ICoreNodeRequestService>>();
            mockCoreComm.Expect(x => x.ServiceProxy.RequestTransaction(null)).Return(new TranCoreNodeResponseDto());

            IAtmCoreService atmSvc = new AtmCoreService(new CoreCommunicationService(mockCoreComm));

            Assert.IsFalse(atmSvc.IsConnected);
            atmSvc.Connect(DateTime.Now, "", "");
            Assert.IsFalse(!atmSvc.IsConnected);
            Assert.IsFalse(atmSvc.RetryResponse);
            var res = atmSvc.GetAtmResponse();
            Assert.AreEqual(res.RawBytes, new [] { _atmRequestHelper.Enq });
            atmSvc.ResponseSent();
            Assert.IsFalse(!atmSvc.IsConnected);
            Assert.Throws<InvalidDataException>(() => atmSvc.HandleIncomingMessage(new AtmRawMessage(new byte[] { _atmRequestHelper.Enq })));

        }

        [Test]
        public void TestOnAtmSendRequest()
        {
            var mockCoreComm = MockRepository.GenerateMock<IScsServiceClient<ICoreNodeRequestService>>();
            mockCoreComm.Expect(x => x.ServiceProxy.RequestTransaction(Arg<TranTritonNodeRequestDto>.Is.Anything)).Return(new TranCoreNodeResponseDto { ResponseCode = "00", Stan = 1 });

            IAtmCoreService atmSvc = new AtmCoreService(new CoreCommunicationService(mockCoreComm));
            const string str = "<STX>00000000td2W0       <FS>S9111111       <FS>11<FS>4892<FS>4089670000392726=17112011000017980000<FS>00008900<FS>00000200<FS>76728398F76ED27D<FS><FS><FS>VA6.00.12WV02.70.10 V06.01.12 0  0T  00 100     00000302K0288000000002K028800000000000000000000000000000000000000<FS><FS><FS><ETX>"; // transaction

            byte[] bytes = _atmRequestHelper.BuildMessageBytesForAtmRequest(str); // Bulid byte

            atmSvc.Connect(DateTime.Now, "", "");
            Assert.IsFalse(!atmSvc.IsConnected);
            Assert.IsFalse(atmSvc.RetryResponse);
            var res = atmSvc.GetAtmResponse();
            Assert.AreEqual(res.RawBytes, new byte[] { _atmRequestHelper.Enq });
            atmSvc.ResponseSent();
            Assert.IsFalse(!atmSvc.IsConnected);
            atmSvc.HandleIncomingMessage(new AtmRawMessage(bytes));
            res = atmSvc.GetAtmResponse();
            Assert.IsNotNull(res.RawBytes);
            atmSvc.ResponseSent();
            Assert.IsFalse(!atmSvc.IsConnected);
            atmSvc.HandleIncomingMessage(new AtmRawMessage(new byte[] { _atmRequestHelper.Ack }));
            res = atmSvc.GetAtmResponse();
            Assert.IsNotNull(res.RawBytes);
            Assert.AreEqual(res.RawBytes, new byte[] { _atmRequestHelper.Eot });
            atmSvc.ResponseSent();
            Assert.IsFalse(atmSvc.IsConnected);
        }

        [Test]
        public void TestOnAtmRequestTwice()
        {
            var mockCoreComm = MockRepository.GenerateMock<IScsServiceClient<ICoreNodeRequestService>>();
            mockCoreComm.Expect(x => x.ServiceProxy.RequestTransaction(Arg<TranTritonNodeRequestDto>.Is.Anything)).Return(new TranCoreNodeResponseDto { ResponseCode = "00", Stan = 1 });

            IAtmCoreService atmSvc = new AtmCoreService(new CoreCommunicationService(mockCoreComm));
            const string str = "<STX>00000000td2W0       <FS>S9111111       <FS>11<FS>4892<FS>4089670000392726=17112011000017980000<FS>00008900<FS>00000200<FS>76728398F76ED27D<FS><FS><FS>VA6.00.12WV02.70.10 V06.01.12 0  0T  00 100     00000302K0288000000002K028800000000000000000000000000000000000000<FS><FS><FS><ETX>"; // transaction

            byte[] bytes = _atmRequestHelper.BuildMessageBytesForAtmRequest(str); // Bulid byte

            atmSvc.Connect(DateTime.Now, "","");
            Assert.IsFalse(!atmSvc.IsConnected);
            Assert.IsFalse(atmSvc.RetryResponse);
            var res = atmSvc.GetAtmResponse();
            Assert.AreEqual(res.RawBytes, new byte[] { _atmRequestHelper.Enq });
            atmSvc.ResponseSent();
            Assert.IsFalse(!atmSvc.IsConnected);
            atmSvc.HandleIncomingMessage(new AtmRawMessage(bytes));
            Assert.Throws<InvalidDataException>(() => atmSvc.HandleIncomingMessage(new AtmRawMessage(bytes)));
        }
    }
}
