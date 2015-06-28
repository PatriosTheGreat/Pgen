using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GenerationCore;
using GenerationCore.ServicesManager;

namespace PgenWindowsClient.ViewModel
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IServicesManager servicesManager)
        {
            _services = new ObservableCollection<ServiceInformation>(servicesManager.LoadServices());

            _services.CollectionChanged += (sender, args) => { OnPropertyChanged(); };

            GenerateServicePassword = new LambdaCommand(
                parameter =>
                {
                    var passwordProvider = parameter as IPasswordProvider;
                    if (passwordProvider == null || 
                        SelectedService == null ||
                        passwordProvider.Password.Length < 1)
                    {
                        return;
                    }

                    SelectedServicePassword = ServicePasswordGenerator.GeneratePassword(
                        SelectedService,
                        passwordProvider.Password);

                    passwordProvider.Clear();
                });

            CopyServicePassword = new LambdaCommand(_ => { Clipboard.SetText(SelectedServicePassword); });
        }

        public ICommand CopyServicePassword
        {
            get { return _copyServicePassword; }

            set
            {
                _copyServicePassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateServicePassword
        {
            get { return _generateServicePassword; }

            set
            {
                _generateServicePassword = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ServiceInformation> FilteredServices
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

        public ServiceInformation SelectedService
        {
            get { return _selectedService; }

            set
            {
                SelectedServicePassword = null;
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        public string SelectedServicePassword
        {
            get { return _selectedServicePassword; }

            set
            {
                _selectedServicePassword = value;
                OnPropertyChanged();
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
        
        private readonly ObservableCollection<ServiceInformation> _services;
        private ServiceInformation _selectedService;
        private string _selectedServicePassword;
        private string _nameFilter;
        private ICommand _generateServicePassword;
        private ICommand _copyServicePassword;
    }
}
