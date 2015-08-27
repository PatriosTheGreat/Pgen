using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace GenerationCore.ServicesManager
{
    public sealed class FileServiceManager : IServicesManager, IFileConfigurableService
    {
        public event Action ServicesUpdated = Actions.DoNothing;

        public FileServiceManager(string defaultFilePath)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(defaultFilePath));
            _serializer = new DataContractSerializer(typeof(List<ServiceInformation>));
            
            _fileSystemWatcher = new FileSystemWatcher();

            _fileSystemWatcher.Changed += (sender, args) =>
            {
                ServicesUpdated();
            };
            
            SetFile(defaultFilePath);
        }

        public void SetFile(string filePath)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _servicesFilePath = filePath;
            _fileSystemWatcher.Path = Path.GetDirectoryName(Path.GetFullPath(filePath));
            _fileSystemWatcher.Filter = Path.GetFileName(filePath);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void SaveService(ServiceInformation service)
        {
            Contract.Assert(service != null);

            var services = LoadServices().Concat(new[] { service });
            SaveServicesToFile(services);
        }

        public void DeleteService(string serviceToken)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceToken));

            var services = LoadServices().Where(service => service.UniqueToken != serviceToken);
            SaveServicesToFile(services);
        }

        public IEnumerable<ServiceInformation> LoadServices()
        {
            return DoActionWithServicesFile(() =>
            {
                if (!File.Exists(_servicesFilePath))
                {
                    return Enumerable.Empty<ServiceInformation>();
                }

                using (var fileSteam = new FileStream(_servicesFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return _serializer.ReadObject(fileSteam) as List<ServiceInformation>;
                }
            });
        }

        private void SaveServicesToFile(IEnumerable<ServiceInformation> services)
        {
            DoActionWithServicesFile(() =>
            {
                File.WriteAllText(_servicesFilePath, string.Empty);
                using (var fileStream = new FileStream(_servicesFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    _serializer.WriteObject(fileStream, services.ToArray());
                }

                return 0;
            });
        }

        private T DoActionWithServicesFile<T>(Func<T> action)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;

            var result = default(T);
            lock (_syncRoot)
            {
                bool actionFailed;

                var triesCount = 0;
                do
                {
                    try
                    {
                        result = action();
                        actionFailed = false;
                    }
                    catch (IOException exception)
                    {
                        actionFailed = true;

                        if (triesCount > 30)
                        {
                            // ToDo: Выкидывать собственное исключение. 
                            // ToDo: Добавить перехват и вывод ошибки с возможностью повторить действие.
                            throw;
                        }

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    triesCount++;
                } while (actionFailed);
            }

            _fileSystemWatcher.EnableRaisingEvents = true;
            return result;
        }
        
        private readonly DataContractSerializer _serializer;
        private string _servicesFilePath;
        private readonly FileSystemWatcher _fileSystemWatcher;

        private readonly object _syncRoot = new object();
    }
}
