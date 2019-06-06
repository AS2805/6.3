using System;
using SwitchLink.HostNode.Communication;
using SwitchLink.HostNode.Communication.Controller;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.HostNode.Controller
{
    class BankingController
    {
        private readonly IBankingCommunicationController _svcHostComm;
        private readonly Object _syncRoot = new object();

        public BankingController()
        {
            _svcHostComm = BankingCommFactory.GetCommSvc();
        }

        public BaseModel Send(byte[] bytes, int stan)
        {
            BaseModel result;
            lock (_syncRoot)
            {
                try
                {
                    result = _svcHostComm.SendCommand(bytes, stan).Result;
                }
                catch (Exception)
                {
                    //todo log exception
                    result = null;
                }
            }

            return result;
        }

        public void CreatePartnerConnection()
        {
            _svcHostComm.CreateConnetionToPartner();
        }

        public void CloseBankConnection()
        {
            _svcHostComm.Close();
        }
    }
}
