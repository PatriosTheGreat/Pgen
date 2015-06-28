using System.IO;
using System.Linq;
using GenerationCore;
using GenerationCore.ServicesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class FileServiceManagerTest
    {
        [TestCleanup]
        public void CleanupTest()
        {
            File.Delete(TestFileName);
        }

        [TestMethod]
        public void EmptySystemShouldLoadNoServices()
        {
            Assert.IsTrue(!_servicesManager.LoadServices().Any());
        }

        [TestMethod]
        public void ServicesShouldBeLoadedAfterSaving()
        {
            var service = new ServiceInformation(
                "TestService",
                new PasswordRestriction(SymbolsType.Digital));

            _servicesManager.SaveService(service);

            var loadedServices = _servicesManager.LoadServices().ToArray();

            Assert.AreEqual(1, loadedServices.Count());
            Assert.AreEqual(service.UniqueToken, loadedServices.First().UniqueToken);
        }

        [TestMethod]
        public void ManagerShouldLoadExistsServicesBeforeSave()
        {
            var service = new ServiceInformation(
                "TestService",
                new PasswordRestriction(SymbolsType.Digital));

            var serviceAfter = new ServiceInformation(
                "TestServiceAfter",
                new PasswordRestriction(SymbolsType.Digital));

            _servicesManager.SaveService(service);

            var anotherServiceManager = new FileServiceManager(TestFileName);
            anotherServiceManager.SaveService(serviceAfter);

            var loadedServices = anotherServiceManager.LoadServices().ToArray();

            Assert.AreEqual(loadedServices.Count(), 2);
            Assert.IsTrue(loadedServices.Any(loadedService => loadedService.UniqueToken == service.UniqueToken));
        }

        private const string TestFileName = "TestServicesFile";
        private readonly FileServiceManager _servicesManager = new FileServiceManager(TestFileName);
    }
}
