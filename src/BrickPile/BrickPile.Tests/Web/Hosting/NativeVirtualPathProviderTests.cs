using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using BrickPile.Core.Hosting;
using BrickPile.UI.Web.Hosting;
using NUnit.Framework;

namespace BrickPile.Tests.Web.Hosting {
    class NativeVirtualPathProviderTests {
        private AppDomain _hostingEnvironmentDomain = null;
        private const string SubFolderPathToDelete = @"c:\temp\sub\subfolder\";
        private const string FolderToDelete = @"c:\temp\sub\foldertodelete\";
        private const string FileToLoad = @"c:\temp\test.txt";

        /// <summary>
        /// Setups this instance.
        /// </summary>
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
            this.Execute(() => HostingEnvironment.RegisterVirtualPathProvider(new NativeVirtualPathProvider()));

            // Create directory to delete
            Directory.CreateDirectory(FolderToDelete);

            //File.Create(FileToLoad);

        }
        /// <summary>
        /// Tests the fixture tear down.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown() {
            // Delete sub folder
            Directory.Delete(SubFolderPathToDelete);

            //File.Delete(FileToLoad);

            // When the fixture is done, tear down the special AppDomain.
            AppDomain.Unload(this._hostingEnvironmentDomain);
        }
        // This method allows you to execute code in the
        // special HostingEnvironment-enabled AppDomain.
        /// <summary>
        /// Executes the specified test method.
        /// </summary>
        /// <param name="testMethod">The test method.</param>
        private void Execute(CrossAppDomainDelegate testMethod) {
            this._hostingEnvironmentDomain.DoCallBack(testMethod);
        }
        /// <summary>
        /// Can_s the load_ file_ from_ disc_ root.
        /// </summary>
        //[Test]
        public void Can_Load_File_From_Disc_Root() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var file = HostingEnvironment.VirtualPathProvider.GetFile("/assets/test.txt");
                using(var s = file.Open()) {
                    Assert.NotNull(s);
                    Assert.IsFalse(file.IsDirectory);
                    Assert.NotNull(file);
                    Console.WriteLine("Test.txt virtual path" + file.VirtualPath);                    
                }
            });
        }
        /// <summary>
        /// Can_s the load_ file_ from_ sub folder.
        /// </summary>
        //[Test]
        public void Can_Load_File_From_SubFolder() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var file = HostingEnvironment.VirtualPathProvider.GetFile("/assets/sub/test.txt");
                var stream = file.Open();
                Assert.NotNull(stream);
                Assert.IsFalse(file.IsDirectory);
                Assert.NotNull(file);
                Console.WriteLine("test.txt virtual path" + file.VirtualPath);
            });
        }
        /// <summary>
        /// Exception_s the is_thrown_ when_ file_ not_ exists.
        /// </summary>
        [Test]
        public void Exception_Is_thrown_When_File_Not_Exists() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                try {
                    var file = HostingEnvironment.VirtualPathProvider.GetFile("/assets/filenotfound.txt");
                    file.Open();
                } catch(FileNotFoundException exception) {
                    Assert.IsNotNull(exception);
                }
            });
        }
        /// <summary>
        /// File_s the exists.
        /// </summary>
        [Test]
        public void File_Exists() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var fileExists = HostingEnvironment.VirtualPathProvider.FileExists("/assets/test.txt");
                Assert.IsTrue(fileExists);
            });
        }
        /// <summary>
        /// Directory_s the exists.
        /// </summary>
        [Test]
        public void Directory_Exists() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directoryExists = HostingEnvironment.VirtualPathProvider.DirectoryExists("/assets/sub/");
                Assert.IsTrue(directoryExists);
            });
        }
        /// <summary>
        /// Get_s the directory.
        /// </summary>
        [Test]
        public void Get_Directory() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/assets/sub/");
                Assert.IsNotNull(directory);
                Assert.IsTrue(directory.IsDirectory);
                Assert.AreEqual("Sub", directory.Name);
            });
        }
        [Test]
        public void Create_Directory() {
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/assets/sub/") as CommonVirtualDirectory;
                directory.CreateDirectory("SubFolder");
            });
        }
        /// <summary>
        /// Delete_s the directory.
        /// </summary>
        [Test]
        public void Delete_Directory() {
            
            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("/assets/sub/foldertodelete") as CommonVirtualDirectory;
                directory.Delete();
            });
        }
        /// <summary>
        /// Delete_s the directory.
        /// </summary>
        //[Test]
        public void Create_And_Save_New_File_In_Directory() {

            // Use the special "Execute" method to run code
            // in the special AppDomain.
            this.Execute(() =>
            {
                const string virtualPath = "/assets/newFile.txt";
                var file = HostingEnvironment.VirtualPathProvider.GetFile(virtualPath) as CommonVirtualFile;
                if (file == null) {
                    var virtualDir = VirtualPathUtility.GetDirectory(virtualPath);
                    var directory = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualDir) as CommonVirtualDirectory;
                    file = directory.CreateFile(VirtualPathUtility.GetFileName(virtualPath));
                }
                // open the replacement file and read its content
                byte[] fileContent;
                using (var fileStream = new FileStream(@"c:\temp\fileToCopy.txt", FileMode.Open, FileAccess.Read)) {
                    fileContent = new byte[fileStream.Length];
                    fileStream.Read(fileContent, 0, fileContent.Length);
                }
                // write the content to the file (that exists in BrickPile's file system)
                using (Stream stream = file.Open(FileMode.Create)) {
                    stream.Write(fileContent, 0, fileContent.Length);
                }
            });
        }
    }
}
