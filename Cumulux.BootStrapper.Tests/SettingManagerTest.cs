namespace Cumulux.BootStrapper.Tests
{
    using System;
    using System.Configuration;
    using Cumulux.BootStrapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    

    [TestClass]
    public class SettingManagerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void GetConfigurationSettingTest()
        {
            var expected = ConfigurationManager.AppSettings["temp"];
            var actual = SettingManager.GetConfigurationSetting("temp");
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetResourcePathTest()
        {
            var expected = ConfigurationManager.AppSettings["DefaultConnectionString"];
            var actual = SettingManager.GetResourcePath("DefaultConnectionString");

            Assert.AreEqual(expected, actual);
        }
    }
}
