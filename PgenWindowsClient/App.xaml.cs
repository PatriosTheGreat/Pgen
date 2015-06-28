using System.Windows;
using GenerationCore.ServicesManager;
using Microsoft.Practices.Unity;

namespace PgenWindowsClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = new UnityContainer();

            /* container.RegisterInstance(
                 typeof (IServicesManager), 
                 new FileServiceManager("Services.config"));*/

            container.RegisterInstance(
                 typeof (IServicesManager), 
                 new TestServiceManager());

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
