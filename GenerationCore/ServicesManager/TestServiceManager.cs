using System.Collections.Generic;

namespace GenerationCore.ServicesManager
{
    public sealed class TestServiceManager : IServicesManager
    {
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

        public IEnumerable<ServiceInformation> LoadServices()
        {
            return _services;
        }

        private readonly IList<ServiceInformation> _services;
    }
}
