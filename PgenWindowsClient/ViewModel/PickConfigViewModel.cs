using System.IO;
using GenerationCore;

namespace PgenWindowsClient.ViewModel
{
    public sealed class PickConfigViewModel : ViewModelBase
    {
        public PickConfigViewModel(
            IFileConfigurableService fileConfigurableService,
            PersistenceSettingManager persistenceSettingManager)
        {
            _fileConfigurableService = fileConfigurableService;
            PathToConfig = Path.GetFullPath(persistenceSettingManager.PathToConfig);
        }

        public string PathToConfig
        {
            get { return _pathToConfig; }

            set
            {
                _pathToConfig = value;
                _fileConfigurableService.SetFile(_pathToConfig);
                OnPropertyChanged();
            }
        }

        public string FileFilter => "Config Files (.config) | *.config";

        private readonly IFileConfigurableService _fileConfigurableService;
        private string _pathToConfig;
    }
}
