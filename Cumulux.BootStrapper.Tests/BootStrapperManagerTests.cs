namespace Cumulux.BootStrapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class BootStrapperManagerTests
    {
        [TestMethod]
        public void TestSimpleGet()
        {
            var uri = new Uri(@"http://contoso.com/installer.msi");

            var downloader = new Mock<IPackageDownloader>();
            downloader.Setup(m => m.DownloadPackageToDisk(It.Is<Uri>(u => u.Equals(uri)),
                                                          It.Is<bool>(b => b == false),
                                                          It.Is<string>(s => string.Equals(s, @"C:\Temp")))).Verifiable();

            var runner = new Mock<IPackageRunner>();
            var unzipper = new Mock<IPackageUnzipper>();
            var logger = new Mock<ILogger>();

            var args = new BootStrapperArgs()
            {
                Get = uri.ToString(),
                LocalResource = @"C:\Temp"
            };

            var manager = new BootStrapperManager(logger.Object, downloader.Object, runner.Object, unzipper.Object);
            manager.Start(args);

            downloader.VerifyAll();
        }

        [TestMethod]
        public void TestAzureGetWithSlash()
        {
            var downloader = new Mock<IPackageDownloader>();
            downloader.Setup(m => m.DownloadPackageToDisk(It.Is<Uri>(u => u.ToString().StartsWith(@"https://test.blob.core.windows.net/container/large.rar")),
                                                          It.Is<bool>(b => b == false),
                                                          It.Is<string>(s => string.Equals(s, @"C:\Temp")))).Verifiable();

            var runner = new Mock<IPackageRunner>();
            var unzipper = new Mock<IPackageUnzipper>();
            var logger = new Mock<ILogger>();

            var args = new BootStrapperArgs()
            {
                Get = "/container/large.rar",
                LocalResource = @"C:\Temp",
                StorageConnection = @"DefaultEndpointsProtocol=https;AccountName=test;AccountKey=SomeKey"
            };

            var manager = new BootStrapperManager(logger.Object, downloader.Object, runner.Object, unzipper.Object);
            manager.Start(args);

            downloader.VerifyAll();
        }

        [TestMethod]
        public void TestAzureGetWithoutSlash()
        {
            var downloader = new Mock<IPackageDownloader>();
            downloader.Setup(m => m.DownloadPackageToDisk(It.Is<Uri>(u => u.ToString().StartsWith(@"https://test.blob.core.windows.net/container/large.rar")),
                                                          It.Is<bool>(b => b == false),
                                                          It.Is<string>(s => string.Equals(s, @"C:\Temp")))).Verifiable();

            var runner = new Mock<IPackageRunner>();
            var unzipper = new Mock<IPackageUnzipper>();
            var logger = new Mock<ILogger>();

            var args = new BootStrapperArgs()
            {
                Get = "container/large.rar",
                LocalResource = @"C:\Temp",
                StorageConnection = @"DefaultEndpointsProtocol=https;AccountName=test;AccountKey=SomeKey"
            };

            var manager = new BootStrapperManager(logger.Object, downloader.Object, runner.Object, unzipper.Object);
            manager.Start(args);

            downloader.VerifyAll();
        }

        [TestMethod]
        public void TestUnzip()
        {
            var downloader = new Mock<IPackageDownloader>();
            downloader.Setup(m => m.DownloadPackageToDisk(It.IsAny<Uri>(), It.IsAny<bool>(), It.IsAny<string>())).Returns(@"C:\Temp\testing.zip").Verifiable();

            var runner = new Mock<IPackageRunner>();
            var unzipper = new Mock<IPackageUnzipper>();
            var logger = new Mock<ILogger>();

            unzipper.Setup(m => m.Unzip(It.Is<string>(s => string.Equals(s, @"C:\Temp\testing.zip")), It.Is<string>(s => s == @"C:\Temp"), It.Is<bool>(b => b == false))).Verifiable();

            var args = new BootStrapperArgs()
            {
                Get = "http://contoso.net/downloads/testing.zip",
                LocalResource = @"C:\Temp", // when run on bootstrapper.exe, this is assiged to whatever unziptarget is
                UnzipTarget = @"C:\Temp",
                Unzip = true
            };

            var manager = new BootStrapperManager(logger.Object, downloader.Object, runner.Object, unzipper.Object);
            manager.Start(args);

            unzipper.VerifyAll();
        }
    }
}
