using System;
using System.Collections.Generic;
using System.Linq;

namespace GenerationCore.ServicesManager
{
    public sealed class TestServiceManager : IServicesManager
    {
        public event Action ServicesUpdated;

        public TestServiceManager()
        {
            _services = new List<ServiceInformation>
            {
                new ServiceInformation(
                    "VK",
                    new PasswordRestriction(SymbolsType.Digital)),
                new ServiceInformation(
                    "Facebook",
                    new PasswordRestriction(SymbolsType.UpcaseLatin)),
                new ServiceInformation(
                    "Twitter",
                    new PasswordRestriction(SymbolsType.LowcaseLatin)),
                new ServiceInformation(
                    "Instagram",
                    new PasswordRestriction(SymbolsType.LowcaseLatin | SymbolsType.Digital)),
                new ServiceInformation(
                    "VK",
                    new PasswordRestriction(SymbolsType.Digital)),
                new ServiceInformation(
                    "Facebook",
                    new PasswordRestriction(SymbolsType.UpcaseLatin)),
                new ServiceInformation(
                    "Twitter",
                    new PasswordRestriction(SymbolsType.LowcaseLatin)),
                new ServiceInformation(
                    "Instagram",
                    new PasswordRestriction(SymbolsType.LowcaseLatin | SymbolsType.Digital))
            };
        }

        public void SaveService(ServiceInformation service)
        {
            _services.Add(service);
        }

        public void DeleteService(string serviceToken)
        {
            _services.Remove(_services.Single(x => x.UniqueToken == serviceToken));
        }

        public IEnumerable<ServiceInformation> LoadServices()
        {
            return _services;
        }

        private readonly IList<ServiceInformation> _services;
    }
}
