using System;
using System.Collections.Generic;

namespace GenerationCore.ServicesManager
{
    public interface IServicesManager
    {
        void SaveService(ServiceInformation service);

        void DeleteService(string serviceToken);

        IEnumerable<ServiceInformation> LoadServices();

        event Action ServicesUpdated;
    }
}
