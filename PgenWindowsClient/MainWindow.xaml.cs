using System.Windows;
using GenerationCore;
using GenerationCore.ServicesManager;
using Microsoft.Practices.Unity;
using PgenWindowsClient.View;
using PgenWindowsClient.ViewModel;

namespace PgenWindowsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPageNavigator
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void NavigateToServicesPage()
        {
            var servicesViewModel = _container.Resolve<ServicesViewModel>();
            MainFrame.Navigate(new ServicesView { DataContext = servicesViewModel });
        }

        public void NavigateToAddServicePage()
        {
            var addServiceViewModel = _container.Resolve<AddServiceViewModel>();
            MainFrame.Navigate(new AddServiceView { DataContext = addServiceViewModel });
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _container = new UnityContainer();

            // ToDo: Добавить возможность загружать путь выбранный при предыдущем запуске приложения
            const string pathToDefaultConfig = "Services.config";

            var fileServiceManager = new FileServiceManager(pathToDefaultConfig);
            _container.RegisterInstance(typeof (IServicesManager), fileServiceManager);
            _container.RegisterInstance(typeof(IFileConfigurableService), fileServiceManager);

            _container.RegisterInstance(
                typeof (PickConfigViewModel),
                new PickConfigViewModel(_container.Resolve<IFileConfigurableService>(), pathToDefaultConfig));

            // _container.RegisterInstance(typeof(IServicesManager), new TestServiceManager());
            _container.RegisterInstance(typeof(IPageNavigator), this);

            NavigateToServicesPage();
        }
        
        private UnityContainer _container;
    }
}
