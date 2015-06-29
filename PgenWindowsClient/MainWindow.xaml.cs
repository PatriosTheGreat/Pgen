using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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

            _container.RegisterInstance(
                 typeof (IServicesManager), 
                 new FileServiceManager("Services.config"));

            // _container.RegisterInstance(typeof(IServicesManager), new TestServiceManager());
            _container.RegisterInstance(typeof(IPageNavigator), this);

            NavigateToServicesPage();
        }
        
        private UnityContainer _container;
    }
}
