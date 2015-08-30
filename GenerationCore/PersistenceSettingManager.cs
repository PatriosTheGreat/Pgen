using System.IO;

namespace GenerationCore
{
    public sealed class PersistenceSettingManager
    {
        public string PathToConfig
        {
            get
            {
                string pathToConfig;
                return TryReadPathToConfig(out pathToConfig) ? pathToConfig : DefaultPath;
            }

            set
            {
                TryWritePathToConfig(value);
            }
        }

        private bool TryReadPathToConfig(out string pathToConfig)
        {
            pathToConfig = string.Empty;
            lock (_syncRoot)
            {
                try
                {
                    pathToConfig = File.ReadAllText(PathToPersistenceSettingsFile);
                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
        }

        private void TryWritePathToConfig(string pathToConfig)
        {
            lock (_syncRoot)
            {
                try
                {
                    File.WriteAllText(PathToPersistenceSettingsFile, pathToConfig);
                }
                catch (IOException)
                {
                }
            }
        }

        private readonly object _syncRoot = new object();
        private const string DefaultPath = "Services.config";
        private const string PathToPersistenceSettingsFile = "Settings.config";
    }
}
