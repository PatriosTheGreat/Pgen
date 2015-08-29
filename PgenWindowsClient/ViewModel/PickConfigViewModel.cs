using System.IO;
using GenerationCore;

namespace PgenWindowsClient.ViewModel
{
    public sealed class PickConfigViewModel : ViewModelBase
    {
        public PickConfigViewModel(
            IFileConfigurableService fileConfigurableService,
            string defaultConfigPath = "")
        {
            _fileConfigurableService = fileConfigurableService;
            PathToConfig = Path.GetFullPath(defaultConfigPath);
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
