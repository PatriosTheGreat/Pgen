using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GenerationCore;
using GenerationCore.ServicesManager;

namespace PgenWindowsClient.ViewModel
{
    public sealed class ServicesViewModel : ViewModelBase
    {
        public ServicesViewModel(
            IServicesManager servicesManager, 
            IPageNavigator pageNavigator, 
            PickConfigViewModel pickConfigViewModel)
        {
            Contract.Assert(servicesManager != null);
            Contract.Assert(pageNavigator != null);
            Contract.Assert(pickConfigViewModel != null);

            _pageNavigator = pageNavigator;
            PickConfigViewModel = pickConfigViewModel;

            ResetServices(servicesManager.LoadServices());

            _servicesListView.Filter = FilterByNameFilter;

            _services.CollectionChanged += (sender, args) =>
            {
                _servicesListView.Refresh();
                OnPropertyChanged();
            };

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

            NavigateToAddService = new LambdaCommand(_ => { _pageNavigator.NavigateToAddServicePage(); });

            // ToDo: Вынести действие в background поток
            DeleteSelectedService = new LambdaCommand(_ =>
            {
                servicesManager.DeleteService(SelectedService.UniqueToken);
                _services.Remove(SelectedService);
                SelectedService = null;
                OnPropertyChanged();
            });

            // ToDo: Вынести действие в background поток
            servicesManager.ServicesUpdated += () =>
            {
                ResetServices(servicesManager.LoadServices());

                if (SelectedService != null && 
                    _services.SingleOrDefault(
                        service => 
                            service.UniqueToken == SelectedService.UniqueToken) == null)
                {
                    SelectedService = null;
                }

                OnPropertyChanged();
            };
        }

        public ICommand NavigateToAddService
        {
            get { return _navigateToAddService; }

            set
            {
                _navigateToAddService = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteSelectedService
        {
            get { return _deleteSelectedService; }

            set
            {
                _deleteSelectedService = value;
                OnPropertyChanged();
            }
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

        public ICollectionView FilteredServices
        {
            get { return _servicesListView; }

            set
            {
                _servicesListView = value;
                OnPropertyChanged();
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
                _servicesListView.Refresh();
                OnPropertyChanged();
            }
        }

        public PickConfigViewModel PickConfigViewModel { get; }

        private void ResetServices(IEnumerable<ServiceInformation> newServices)
        {
            _services = new ObservableCollection<ServiceInformation>(newServices);

            FilteredServices = CollectionViewSource.GetDefaultView(_services);
        }

        private bool FilterByNameFilter(object serviceObject)
        {
            if (string.IsNullOrEmpty(NameFilter))
            {
                return true;
            }

            var service = serviceObject as ServiceInformation;
            return service.ServiceName.ToLower().Contains(NameFilter);
        }

        private ICollectionView _servicesListView;
        private ObservableCollection<ServiceInformation> _services;
        private ServiceInformation _selectedService;
        private string _selectedServicePassword;
        private string _nameFilter;
        private ICommand _generateServicePassword;
        private ICommand _copyServicePassword;
        private readonly IPageNavigator _pageNavigator;
        private ICommand _navigateToAddService;
        private ICommand _deleteSelectedService;
    }
}
