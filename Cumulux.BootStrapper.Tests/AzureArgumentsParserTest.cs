namespace Cumulux.BootStrapper.Tests
{
    using System;
    using Cumulux.BootStrapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureArgumentsParserTest
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
        public void ParseLocalResource()
        {
            AzureArgumentsParser parser = new AzureArgumentsParser();
            var parsed = parser.ParseArguments(@"$lr(temp)\somefile.txt");

            Assert.AreEqual<string>(@"C:\Resources\Temp\somefile.txt", parsed);
        }

        [TestMethod]
        public void ParseStorageConnection()
        {
            AzureArgumentsParser parser = new AzureArgumentsParser();
            var parsed = parser.ParseArguments(@"$sc(DefaultConnectionString)");

            Assert.AreEqual<string>(@"DefaultEndpointsProtocol=https;AccountName=poopypants;AccountKey=SomeKey", parsed);
        }

        [TestMethod]
        public void ParseMultiplePrefixes()
        {
            var parser = new AzureArgumentsParser();
            var actual = parser.ParseArguments(@"-lr $lr(temp) -run $lr(temp)\file.exe");

            Assert.AreEqual<string>(@"-lr C:\Resources\Temp -run C:\Resources\Temp\file.exe", actual);
        }

        [TestMethod]
        public void DoNotExpandUnknownPrefixes()
        {
            var parser = new AzureArgumentsParser();
            var actual = parser.ParseArguments(@"-switch $tdd(cumulux)");

            Assert.AreEqual<string>(@"-switch $tdd(cumulux)", actual);
        }

        [TestMethod]
        public void ParseConfig()
        {
            var parser = new AzureArgumentsParser();
            var actual = parser.ParseArguments(@"$config(foo.installContainer)");
            var expected = "container";

            Assert.AreEqual<string>(expected, actual);
        }

    }
}
