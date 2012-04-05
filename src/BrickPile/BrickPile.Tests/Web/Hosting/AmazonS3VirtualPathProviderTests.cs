using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using BrickPile.FileSystem.AmazonS3.Hosting;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Hosting {
    public class AmazonS3VirtualPathProviderTests {
        // Instance property for the HostingEnvironment-enabled AppDomain.
        private AppDomain _hostingEnvironmentDomain = null;

        //[TestFixtureSetUp]
        public void Setup() {
            // Create the AppDomain that will support the VPP.
            this._hostingEnvironmentDomain =
              AppDomain.CreateDomain("HostingEnvironmentDomain",
              AppDomain.CurrentDomain.Evidence,
              AppDomain.CurrentDomain.SetupInformation,
              AppDomain.CurrentDomain.PermissionSet);

            // Set some required data that the runtime needs.
            this._hostingEnvironmentDomain.SetData(".appDomain", "HostingEnvironmentDomain");
            this._hostingEnvironmentDomain.SetData(".appId", "HostingEnvironmentTests");
            this._hostingEnvironmentDomain.SetData(".appPath", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            this._hostingEnvironmentDomain.SetData(".appVPath", "/");
            this._hostingEnvironmentDomain.SetData(".domainId", "HostingEnvironmentTests");

            // Initialize the hosting environment.
            var environment = this._hostingEnvironmentDomain.CreateInstanceAndUnwrap(typeof(HostingEnvironment).Assembly.FullName, typeof(HostingEnvironment).FullName) as HostingEnvironment;

            // Finally, register your VPP instance so you can test.
            this.Execute(() => HostingEnvironment.RegisterVirtualPathProvider(new AmazonS3VirtualPathProvider()));

        }

        //[TestFixtureTearDown]
        public void TestFixtureTearDown() {
            // When the fixture is done, tear down the special AppDomain.
            AppDomain.Unload(this._hostingEnvironmentDomain);
        }

        // This method allows you to execute code in the
        // special HostingEnvironment-enabled AppDomain.
        private void Execute(CrossAppDomainDelegate testMethod) {
            this._hostingEnvironmentDomain.DoCallBack(testMethod);
        }
        //[Test]
        public void Can_Load_File_From_S3_Root() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var file = HostingEnvironment.VirtualPathProvider.GetFile("/s3/main_area_7.jpg");
                var stream = file.Open();
                Assert.NotNull(stream);
                Assert.IsFalse(file.IsDirectory);
                Assert.NotNull(file);
                Console.WriteLine("Office.jpg virtual path" + file.VirtualPath);
            });
        }
        //[Test]
        public void Can_Load_File_From_S3_SubFolder() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var file = HostingEnvironment.VirtualPathProvider.GetFile("/s3/images/iMac.jpg");
                var stream = file.Open();
                Assert.NotNull(stream);
                Assert.IsFalse(file.IsDirectory);
                Assert.NotNull(file);
                Console.WriteLine("Office.jpg virtual path" + file.VirtualPath);
            });
        }
        //[Test]
        public void Can_Load_Files_From_S3() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/s3/images/");
                Assert.NotNull(directory);
                Assert.IsTrue(directory.IsDirectory);
                Assert.NotNull(directory.Directories);
                Assert.NotNull(directory.Files);
                Console.WriteLine("Images directory:" + directory.Name);
            });
        }

        //[Test]
        public void Can_Load_Root_Directory() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/s3/");
                Assert.NotNull(directory);
                Assert.IsTrue(directory.IsDirectory);
                Assert.NotNull(directory.Directories);
                Assert.NotNull(directory.Files);
                Assert.AreEqual(1, directory.Directories.Count());
                Assert.AreEqual("/s3/",directory.VirtualPath);
            });
        }
    }
    static class EnumerableExtensions {
        public static int Count(this IEnumerable source) {
            return Enumerable.Count(source.Cast<object>());
        }
    }

}