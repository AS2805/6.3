using System;
using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Switchlink.Diagnostics.Switchlink.Diagnostics;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_Diagnostics
    {
        //log4net.ILog logger = LogManager.GetLogger(typeof(UnitTest_Diagnostics));
        ILog logger = LogManager.GetLogger("Diagnosticlogger");

        [TestMethod]
        public void Test_PerformDiagnostics()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.PerformDiagnostics();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }

        [TestMethod]
        public void Test_NetworkInformation()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.NetworkInformation();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }

        [TestMethod]
        public void Test_HostCommandVolumes()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.HostCommandVolumes();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }

        [TestMethod]
        public void Test_HealthCheckAccumulatedCounts()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.HealthCheckAccumulatedCounts();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }

        [TestMethod]
        public void Test_ResetHealthCheckAccumulatedCounts()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.ResetHealthCheckAccumulatedCounts();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }


        [TestMethod]
        public void Test_HealthCheckStatus()
        {
            Diagnostics diag = new Diagnostics();
            Dictionary<String, String> response = diag.HealthCheckStatus();
            if (response.ContainsKey("ErrorCode"))
            {
                Assert.AreEqual("00", response["ErrorCode"]);
            }
        }

    }
}
