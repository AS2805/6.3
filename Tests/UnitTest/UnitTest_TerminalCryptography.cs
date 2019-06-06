using System;
using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.Cryptography.TritonCryptography;
namespace UnitTest
{
    [TestClass]
    public class UnitTest_TerminalCryptography
    {
        log4net.ILog logger = LogManager.GetLogger(typeof(UnitTest_TerminalCryptography));


        [TestMethod]
        public void Test_Sessionkeys()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.GenerateTerminalSessionKeys("UF0C85E08A34581A140FB2744BD6F98FF");  // TMK in db

            if (response.ContainsKey("ErrorCode"))
              {
                Assert.AreEqual((response["ErrorCode"]), "00");
              }
        }

        [TestMethod]
        public void Test_GenerateKeys()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.GenerateKeys("002");

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_GenerateKeys_TMK()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.GenerateKeys_TMK();

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_GenerateKeys_TAK()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.GenerateKeys_TAK();

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_GenerateMACKeys()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.GenerateMACKeys("UF0C85E08A34581A140FB2744BD6F98FF");   // tmk from db

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_TranslateKeyScheme()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.TranslateKeyScheme("002", "U6A4074D2BACA9E894A37C9CBA2B30F6B", "U"); // tpk_lmk from db or terminal session key test

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_TranslatePIN_TDES()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();

            //D4
            //U61C0EA6E706F5B2DAF57B37C7D825D04
            //U88EE45E510C981FC928F95EAB01EAA28
            //0D12F42095EF12A2
            //237000000234
            string accNumber = "4902370000002348=121210111234123";
            string bin = accNumber.Substring(0, 6);

            accNumber = accNumber.Substring(0,accNumber.IndexOf("=", StringComparison.Ordinal));
            accNumber = accNumber.Substring(accNumber.Length - 13, 12);
           
            Dictionary<String, String> response = terminalCrypto.TranslatePIN_TDES("U61C0EA6E706F5B2DAF57B37C7D825D04", "U88EE45E510C981FC928F95EAB01EAA28", "0D12F42095EF12A2", accNumber);

            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_TranslatePIN_TDES_CA()
        {
            TritonCryptography terminalCrypto = new TritonCryptography();
            Dictionary<String, String> response = terminalCrypto.TranslatePIN_TDES_CA("X940E5629A11369666B5EDDC0F3CF9271", "U88EE45E510C981FC928F95EAB01EAA28", "C84DD86CB463DF35", "237000000234");

            if (response.ContainsKey("Status"))
            {
                Assert.AreEqual((response["Status"]), "00");
            }
        }
    }
}
