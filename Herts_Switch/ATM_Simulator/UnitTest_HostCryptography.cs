using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.Cryptography.HostCryptography;

namespace UnitTest
{
    [TestClass]
   public class UnitTest_HostCryptography
    {
        //log4net.ILog logger = LogManager.GetLogger(typeof(UnitTest_HostCryptography));
        log4net.ILog logger = LogManager.GetLogger("CryptographyLogger");

        [TestMethod]
        public void Test_KekrValidationResponse()
        {
            HostCryptography hostCrypto = new HostCryptography();
            Dictionary<String, String> response = hostCrypto.Generate_KEKr_Validation_Response("U6991809FE5FD03B5CE28F671CB1DCB32", "U6991809FE5FD03B5CE28F671CB1DCB32"); // kekr from db,
            if (response.ContainsKey("ErrorCode"))
                if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }    

        [TestMethod]
        public void Test_KeksValidationRequest()
        {
            HostCryptography hostCrypto = new HostCryptography();
            Dictionary<String, String> response = hostCrypto.Generate_KEKs_Validation_Request("U186C568AD0CA98BA70150B0787AED8A3"); // keks from database
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_VerifyMAC()
        {
            HostCryptography hostCrypto = new HostCryptography();
            var response = hostCrypto.VerifyMAC("68B65A3C", "01101100011101010110110001110101", "","UDBEFF18B28C6CCC659F17A2B642DE795");

            if (response.Length==0)
            {
               Assert.Fail("The method did not return any value");
            }
        }

        [TestMethod]
        public void Test_GenerateRandomNumber()
        {
            HostCryptography hostCrypto = new HostCryptography();
            var response = hostCrypto.GenerateRandomNumber();
            
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_GenerateSetOfZoneKeys()
        {
            HostCryptography hostCrypto = new HostCryptography();
            Dictionary<String, String> response = hostCrypto.GenerateSetOfZoneKeys("U186C568AD0CA98BA70150B0787AED8A3"); // keks from db
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_TranslatedSetOfZoneKeys()
        {
            HostCryptography hostCrypto = new HostCryptography();             //          kekr                                        zpk_zmk                             zak_zmk                                zek_zmk
            Dictionary<String, String> response = hostCrypto.TranslateSetOfZoneKeys("U6991809FE5FD03B5CE28F671CB1DCB32", "D65FC0A3A2DE8D08084E03D28EBD3287", "753EECC690F068E93DE3C1259F14D991", "H005E7BE78AB407278E48F66D32AD5214");
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }

        [TestMethod]
        public void Test_GenerateMAC()
        {
            HostCryptography hostCrypto = new HostCryptography();
            Dictionary<String, String> response = hostCrypto.CalculateMAC_ZAK("01101100011101010110110001110101", "UDBEFF18B28C6CCC659F17A2B642DE795");
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual((response["ErrorCode"]), "00");
            }
        }
    }
}
