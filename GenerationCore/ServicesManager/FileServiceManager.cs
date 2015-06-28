using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GenerationCore.ServicesManager
{
    public sealed class FileServiceManager : IServicesManager
    {
        public FileServiceManager(string servicesFileName)
        {
            _servicesFileName = servicesFileName;
            _serializer = new DataContractSerializer(typeof(List<ServiceInformation>));

            _services = LoadServices().ToList();
        }

        public void SaveService(ServiceInformation service)
        {
            _services.Add(service);
            SaveServicesToFile();
        }
        
        public IEnumerable<ServiceInformation> LoadServices()
        {
            if (!File.Exists(_servicesFileName))
            {
                return Enumerable.Empty<ServiceInformation>();
            }

            using (var fileSteam = new FileStream(_servicesFileName, FileMode.Open))
            {
                var loadedServices = _serializer.ReadObject(fileSteam) as List<ServiceInformation>;
                _services = loadedServices;
                return _services;
            }
        }

        private void SaveServicesToFile()
        {
            using (var fileStream = new FileStream(_servicesFileName, FileMode.OpenOrCreate))
            {
                _serializer.WriteObject(fileStream, _services.ToArray());
            }
        }

        private List<ServiceInformation> _services;
        private readonly DataContractSerializer _serializer;
        private readonly string _servicesFileName;
    }
}
