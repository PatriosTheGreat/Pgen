using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace GenerationCore.ServicesManager
{
    public sealed class FileServiceManager : IServicesManager
    {
        public FileServiceManager(string servicesFileName)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(servicesFileName));

            _servicesFileName = servicesFileName;
            _serializer = new DataContractSerializer(typeof(List<ServiceInformation>));

            _services = LoadServices().ToList();
        }

        public void SaveService(ServiceInformation service)
        {
            Contract.Assert(service != null);

            _services.Add(service);
            SaveServicesToFile();
        }

        public void DeleteService(string serviceToken)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceToken));
            
            _services.RemoveAt(_services.FindIndex(information => information.UniqueToken == serviceToken));
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
            File.WriteAllText(_servicesFileName, string.Empty);
            using (var fileStream = new FileStream(_servicesFileName, FileMode.Create))
            {
                _serializer.WriteObject(fileStream, _services.ToArray());
            }
        }

        private List<ServiceInformation> _services;
        private readonly DataContractSerializer _serializer;
        private readonly string _servicesFileName;
    }
}
