using System.IO;
using System.Linq;
using System.Threading;
using GenerationCore;
using GenerationCore.ServicesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class FileServiceManagerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            File.Delete(TestFileName);
            File.Delete(OtherFileName);
            _servicesManager = new FileServiceManager(
                new PersistenceSettingManager { PathToConfig = TestFileName });
        }

        [TestMethod]
        public void EmptySystemShouldLoadNoServices()
        {
            Assert.IsTrue(!_servicesManager.LoadServices().Any());
        }

        [TestMethod]
        public void ServicesShouldBeLoadedAfterSaving()
        {
            _servicesManager.SaveService(_defaultService);

            var loadedServices = _servicesManager.LoadServices().ToArray();

            Assert.AreEqual(1, loadedServices.Length);
            Assert.AreEqual(_defaultService.UniqueToken, loadedServices.First().UniqueToken);
        }

        [TestMethod]
        public void ManagerShouldLoadExistsServicesBeforeSave()
        {
            _servicesManager.SaveService(_defaultService);

            var anotherServiceManager = new FileServiceManager(
                new PersistenceSettingManager { PathToConfig = TestFileName });
            anotherServiceManager.SaveService(_otherService);

            var loadedServices = anotherServiceManager.LoadServices().ToArray();

            Assert.AreEqual(loadedServices.Length, 2);
            Assert.IsTrue(loadedServices.Any(
                loadedService => loadedService.UniqueToken == _defaultService.UniqueToken));
        }

        [TestMethod]
        public void ServiceShouldNotExistsAfterDelete()
        {
            _servicesManager.SaveService(_defaultService);
            _servicesManager.SaveService(_otherService);
            
            var loadedServices = _servicesManager.LoadServices().ToArray();
            Assert.AreEqual(loadedServices.Length, 2);

            _servicesManager.DeleteService(_otherService.UniqueToken);

            loadedServices = _servicesManager.LoadServices().ToArray();
            Assert.AreEqual(loadedServices.Length, 1);
            Assert.AreEqual(loadedServices.Single().UniqueToken, _defaultService.UniqueToken);
        }

        [TestMethod]
        public void FileManagerShouldBeEmptyAfterSetNewFile()
        {
            _servicesManager.SaveService(_defaultService);
            _servicesManager.SetFile(OtherFileName);

            var loadedServices = _servicesManager.LoadServices().ToArray();
            Assert.AreEqual(loadedServices.Length, 0);
        }

        [TestMethod]
        public void FileManagerShouldLoadExistsServicesOnNewFile()
        {
            _servicesManager.SaveService(_defaultService);
            _servicesManager.SetFile(OtherFileName);
            _servicesManager.SetFile(TestFileName);

            var loadedServices = _servicesManager.LoadServices().ToArray();
            Assert.AreEqual(loadedServices.Single().UniqueToken, _defaultService.UniqueToken);
        }

        [TestMethod]
        public void FileManagerShouldLoadNewServicesAfter()
        {
            var otherServiceManager = new FileServiceManager(
                new PersistenceSettingManager { PathToConfig = TestFileName });
            
            _servicesManager.SaveService(_otherService);
            otherServiceManager.SaveService(_defaultService);

            var loadedServices = otherServiceManager.LoadServices().ToArray();
            Assert.AreEqual(loadedServices.Length, 2);
            Assert.IsTrue(loadedServices.Any(service  => service .UniqueToken == _defaultService.UniqueToken));
        }

        [TestMethod]
        public void FileManagerShouldRiseChangeEventIfFileChanges()
        {
            var isEventRose = false;
            _servicesManager.SaveService(_otherService);

            _servicesManager.ServicesUpdated += () =>
            {
                isEventRose = true;
            };

            File.WriteAllText(TestFileName, string.Empty);

            Thread.Sleep(1000);

            Assert.IsTrue(isEventRose);
        }

        private readonly ServiceInformation _defaultService = new ServiceInformation(
            "TestService",
            new PasswordRestriction(SymbolsType.Digital));

        private readonly ServiceInformation _otherService = new ServiceInformation(
                "TestServiceAfter",
                new PasswordRestriction(SymbolsType.Digital));

        private const string TestFileName = "TestServicesFile";
        private const string OtherFileName = "TestServicesFile2";
        private FileServiceManager _servicesManager;
    }
}
