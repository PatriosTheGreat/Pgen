using System.Collections.ObjectModel;
using System.Linq;
using GenerationCore;

namespace PgenWindowsClient.ViewModel
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _services = new ObservableCollection<ServiceInformation>
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

            _services.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged("FilteredServices");
            };
        }

        public ServiceInformation[] FilteredServices
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NameFilter))
                {
                    return _services.ToArray();
                }

                return 
                    _services
                        .Where(service => service.ServiceName.ToLower().Contains(NameFilter))
                        .ToArray();
            }
        }

        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value.ToLower();
                OnPropertyChanged();
                OnPropertyChanged("FilteredServices");
            }
        }

        public bool NeedAddNewService
        {
            get { return _needAddNewService; }

            set
            {
                _needAddNewService = value;
                OnPropertyChanged();
            }
        }

        private readonly ObservableCollection<ServiceInformation> _services;

        private bool _needAddNewService;
        private string _nameFilter;
    }
}
