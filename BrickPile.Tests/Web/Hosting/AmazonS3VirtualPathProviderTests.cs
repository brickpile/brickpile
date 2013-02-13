using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using BrickPile.Core.Hosting;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Hosting {
    //public class AmazonS3VirtualPathProviderTests {
    //    // Instance property for the HostingEnvironment-enabled AppDomain.
    //    private AppDomain _hostingEnvironmentDomain = null;

    //    //[TestFixtureSetUp]
    //    public void Setup() {
    //        // Create the AppDomain that will support the VPP.
    //        this._hostingEnvironmentDomain =
    //          AppDomain.CreateDomain("HostingEnvironmentDomain",
    //          AppDomain.CurrentDomain.Evidence,
    //          AppDomain.CurrentDomain.SetupInformation,
    //          AppDomain.CurrentDomain.PermissionSet);

    //        // Set some required data that the runtime needs.
    //        this._hostingEnvironmentDomain.SetData(".appDomain", "HostingEnvironmentDomain");
    //        this._hostingEnvironmentDomain.SetData(".appId", "HostingEnvironmentTests");
    //        this._hostingEnvironmentDomain.SetData(".appPath", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    //        this._hostingEnvironmentDomain.SetData(".appVPath", "/");
    //        this._hostingEnvironmentDomain.SetData(".domainId", "HostingEnvironmentTests");

    //        // Initialize the hosting environment.
    //        var environment = this._hostingEnvironmentDomain.CreateInstanceAndUnwrap(typeof(HostingEnvironment).Assembly.FullName, typeof(HostingEnvironment).FullName) as HostingEnvironment;

    //        // Finally, register your VPP instance so you can test.
    //        this.Execute(() => HostingEnvironment.RegisterVirtualPathProvider(new AmazonS3VirtualPathProvider()));

    //    }

    //    //[TestFixtureTearDown]
    //    public void TestFixtureTearDown() {
    //        // When the fixture is done, tear down the special AppDomain.
    //        AppDomain.Unload(this._hostingEnvironmentDomain);
    //    }

    //    // This method allows you to execute code in the
    //    // special HostingEnvironment-enabled AppDomain.
    //    private void Execute(CrossAppDomainDelegate testMethod) {
    //        this._hostingEnvironmentDomain.DoCallBack(testMethod);
    //    }
    //    //[Test]
    //    public void Can_Load_File_From_S3_Root() {
    //        // Use the special "Execute" method to run code
    //        // in the special AppDomain.
    //        this.Execute(() =>
    //        {
    //            var file = HostingEnvironment.VirtualPathProvider.GetFile("/static/9723d032034644629505f37994d89074.jpg");
    //            var stream = file.Open();
    //            Assert.NotNull(stream);
    //            Assert.IsFalse(file.IsDirectory);
    //            Assert.NotNull(file);
    //            Console.WriteLine("Office.jpg virtual path" + file.VirtualPath);
    //        });
    //    }
    //    //[Test]
    //    public void Can_Load_File_From_S3_SubFolder() {
    //        // Use the special "Execute" method to run code
    //        // in the special AppDomain.
    //        this.Execute(() =>
    //        {
    //            var file = HostingEnvironment.VirtualPathProvider.GetFile("/s3/images/iMac.jpg");
    //            var stream = file.Open();
    //            Assert.NotNull(stream);
    //            Assert.IsFalse(file.IsDirectory);
    //            Assert.NotNull(file);
    //            Console.WriteLine("Office.jpg virtual path" + file.VirtualPath);
    //        });
    //    }
    //    //[Test]
    //    public void Can_Load_Files_From_S3() {
    //        // Use the special "Execute" method to run code
    //        // in the special AppDomain.
    //        this.Execute(() =>
    //        {
    //            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/s3/images/");
    //            Assert.NotNull(directory);
    //            Assert.IsTrue(directory.IsDirectory);
    //            Assert.NotNull(directory.Directories);
    //            Assert.NotNull(directory.Files);
    //            Console.WriteLine("Images directory:" + directory.Name);
    //        });
    //    }

    //    //[Test]
    //    public void Can_Load_Root_Directory() {
    //        // Use the special "Execute" method to run code
    //        // in the special AppDomain.
    //        this.Execute(() =>
    //        {
    //            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/s3/");
    //            Assert.NotNull(directory);
    //            Assert.IsTrue(directory.IsDirectory);
    //            Assert.NotNull(directory.Directories);
    //            Assert.NotNull(directory.Files);
    //            Assert.AreEqual(1, directory.Directories.Count());
    //            Assert.AreEqual("/s3/",directory.VirtualPath);
    //        });
    //    }
    //    //[Test]
    //    public void Can_Upload_File() {
    //        // Use the special "Execute" method to run code
    //        // in the special AppDomain.
    //        this.Execute(() =>
    //        {
    //            const string virtualPath = "/static/D1.jpg";
    //            //CommonVirtualFile file;
    //            var file = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath) as CommonVirtualFile;
    //            if (file == null) {
    //                var virtualDir = VirtualPathUtility.GetDirectory(virtualPath);
    //                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualDir) as CommonVirtualDirectory;
    //                file = directory.CreateFile(VirtualPathUtility.GetFileName(virtualPath));
    //            }
    //            // open the replacement file and read its content
    //            byte[] fileContent;
    //            using (FileStream fileStream = new FileStream(@"c:\temp\D1.jpg", FileMode.Open, FileAccess.Read)) {
    //                fileContent = new byte[fileStream.Length];
    //                fileStream.Read(fileContent, 0, fileContent.Length);
    //            }
    //            // write the content to the file (that exists in BrickPile's file system)
    //            using (Stream stream = file.Open(FileMode.Create)) {
    //                stream.Write(fileContent, 0, fileContent.Length);
    //            }
    //        });
            
    //    }
    //}
    //static class EnumerableExtensions {
    //    public static int Count(this IEnumerable source) {
    //        return Enumerable.Count(source.Cast<object>());
    //    }
    //}

}