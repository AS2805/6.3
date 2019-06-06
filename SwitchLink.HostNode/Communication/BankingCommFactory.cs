using SwitchLink.HostNode.Communication.Controller;

namespace SwitchLink.HostNode.Communication
{
    class BankingCommFactory
    {
        private static IBankingCommunicationController _commInstance =null;
        public static IBankingCommunicationController GetCommSvc()
        {
            if(_commInstance==null)
                _commInstance=new BankingCommunicationController();

            return _commInstance;
        }
    }
}
