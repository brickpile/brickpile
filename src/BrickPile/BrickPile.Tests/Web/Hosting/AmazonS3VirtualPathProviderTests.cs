using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using BrickPile.UI.Web.Hosting;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Hosting {
    public class AmazonS3VirtualPathProviderTests {
        // Instance property for the HostingEnvironment-enabled AppDomain.
        private AppDomain _hostingEnvironmentDomain = null;

        [TestFixtureSetUp]
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

        [TestFixtureTearDown]
        public void TestFixtureTearDown() {
            // When the fixture is done, tear down the special AppDomain.
            AppDomain.Unload(this._hostingEnvironmentDomain);
        }

        // This method allows you to execute code in the
        // special HostingEnvironment-enabled AppDomain.
        private void Execute(CrossAppDomainDelegate testMethod) {
            this._hostingEnvironmentDomain.DoCallBack(testMethod);
        }

        [Test]
        public void Can_Load_File_From_S3() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() => {
                             var file = HostingEnvironment.VirtualPathProvider.GetFile("~/s3/Images/office.jpg");
                             Assert.IsFalse(file.IsDirectory);
                             Assert.NotNull(file);
                         }
                );
        }
        [Test]
        public void Can_Load_Files_From_S3() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() => {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("~/s3/Images/");
                Assert.NotNull(directory);
                Assert.IsTrue(directory.IsDirectory);
                Assert.NotNull(directory.Directories);
                Assert.NotNull(directory.Files);
            });
        }
    }
}