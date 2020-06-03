using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using inRiverCommunity.Connectivity.iPMC.Contract;
using inRiverCommunity.Connectivity.iPMC.Tests.Helpers;
using inRiverCommunity.Extensions.TestExtensions.iPMC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inRiverCommunity.Connectivity.iPMC.Tests
{
    [TestClass]
    public class ApiAdapterTests
    {

        private const ApiEnvironment TestApiEnvironment = ApiEnvironment.Partner;

        private static readonly string TestPackageFilename = $"{typeof(TestExtensionBase).Namespace}.zip";
        private static readonly string TestPackagePath = Path.Combine(@"..\..\..\Packages", TestPackageFilename);
        private static readonly string TestExtensionAssemblyName = $"{typeof(TestExtensionBase).Namespace}.dll";
        private string _restApiKey;

        [TestInitialize]
        public void Initialize()
        {
            // Reading REST API key from file $(SolutionDir)\rest_api.key. Create one.
            _restApiKey = RestApiKeyHelper.GetRestApiKeyFromFile();
        }

        [TestMethod]
        public async Task Get_All_Packages_Test()
        {
            using IApiClient adapter = new ApiClient(TestApiEnvironment, _restApiKey);

            List<PackageModel> packages = await adapter.PackageService.GetAllPackages();

            Assert.IsNotNull(packages);

            Console.WriteLine($"Total packages found: {packages.Count}");
        }
        
        [TestMethod]
        public async Task Get_All_Extensions_Test()
        {
            using IApiClient adapter = new ApiClient(TestApiEnvironment, _restApiKey);

            List<ExtensionModel> extensions = await adapter.ExtensionService.GetAllExtensions();

            Assert.IsNotNull(extensions);

            Console.WriteLine($"Total extensions found: {extensions.Count}");
        }

        [TestMethod]
        public async Task Upload_Or_Replace_CheckServerExtensions_Package_Test()
        {
            using IApiClient adapter = new ApiClient(TestApiEnvironment, _restApiKey);

            List<PackageModel> packages = await adapter.PackageService.GetAllPackages();

            PackageModel testPackage = packages.FirstOrDefault(p => p.FileName == TestPackageFilename);

            Assert.IsTrue(File.Exists(TestPackagePath));

            await using FileStream data = File.OpenRead(TestPackagePath);

            if (testPackage == null)
            {
                Console.WriteLine($@"No package exists with filename '{TestPackageFilename}', uploading new ...");

                testPackage = await adapter.PackageService.UploadPackageFile(TestPackageFilename, data);
            }
            else
            {
                Console.WriteLine($@"Package {testPackage.Id} exists with filename '{TestPackageFilename}', replacing ...");

                testPackage = await adapter.PackageService.ReplacePackageFile(testPackage.Id, TestPackageFilename, data);
            }

            Assert.IsNotNull(testPackage);

            Console.WriteLine($@"Package {testPackage.Id} created or updated with filename '{TestPackageFilename}'.");
        }

        [TestMethod]
        public async Task Add_TestServerExtension_When_Not_Exists_Test()
        {
            string testExtensionId = $"{nameof(TestServerExtension)}";

            using IApiClient adapter = new ApiClient(ApiEnvironment.Partner, _restApiKey);

            List<PackageModel> packages = await adapter.PackageService.GetAllPackages();

            PackageModel testPackage = packages.FirstOrDefault(p => p.FileName == TestPackageFilename);
            Assert.IsNotNull(testPackage);

            ExtensionModel testExtension = await adapter.ExtensionService.GetExtension(testExtensionId);

            if (testExtension == null)
            {
                Console.WriteLine($@"No extension {testExtensionId} exists, creating new ...");

                testExtension = await adapter.ExtensionService.AddExtension(new ExtensionCreationModel
                {
                    ExtensionId = testExtensionId,
                    AssemblyName = TestExtensionAssemblyName,
                    AssemblyType = $"{typeof(TestServerExtension).FullName}",
                    ExtensionType = $"ServerExtension",
                    PackageId = testPackage.Id,
                });
            }

            Assert.IsNotNull(testExtension);

            Console.WriteLine($@"Extension {testExtension.ExtensionId} created.");
        }

        [TestMethod]
        public async Task Apply_Default_Settings_And_Reload_For_TestServerExtension_Test()
        {
            string testExtensionId = $"{nameof(TestServerExtension)}";

            using IApiClient adapter = new ApiClient(TestApiEnvironment, _restApiKey);

            ExtensionModel testExtension = await adapter.ExtensionService.GetExtension(testExtensionId);

            Assert.IsNotNull(testExtension);

            List<ExtensionSettingModel> defaultSettings = await adapter.ExtensionService.ApplyDefaultExtensionSettings(testExtensionId);
            Assert.IsNotNull(defaultSettings);

            bool reloadResult = await adapter.ExtensionService.ReloadExtensionSettings(testExtensionId);
            Assert.IsTrue(reloadResult);
        }
    }
}
