using System.Collections.Generic;

namespace GenerationCore.ServicesManager
{
    public interface IServicesManager
    {
        void SaveService(ServiceInformation service);

        IEnumerable<ServiceInformation> LoadServices();
    }
}
