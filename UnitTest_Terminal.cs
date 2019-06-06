
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwitchLink.Cryptography
{
    [TestClass]
    public class UnitTest_Terminal
    {
        [TestMethod]
        public void TestSessionkeys()
        {
            TritonNode.TritonCryptography terminalCrypto = new TritonNode.TritonCryptography();
            Dictionary<String, String> Respone = terminalCrypto.GenerateTerminalSessionKeys("UF0C85E08A34581A140FB2744BD6F98FF");

            if(Respone.ContainsKey("ErrorCode"))
           {
                Assert.AreEqual((Respone["ErrorCode"]), "00");
           }
        }
    }
}
