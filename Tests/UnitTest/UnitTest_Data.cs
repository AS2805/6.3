using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Constraints;
using SwitchLink.Data;
using SwitchLink.Data.Models;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_Data
    {
        [TestMethod]
        public void GetTPKZPKByTerminalId()
        {
            using (TerminalData terData = new TerminalData())
            {
                Sessions_Core_ZPK_to_TPK TPKZPK = terData.GetTPKZPKByTerminalId("S9111111");
                Assert.AreNotEqual(TPKZPK, null);
            }
        }
    }
}
