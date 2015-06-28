using System.Security;
using System.Windows;
using Microsoft.Practices.Unity;
using PgenWindowsClient.ViewModel;

namespace PgenWindowsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPasswordProvider 
    {
        [Dependency]
        public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            UserPassword.SecurePassword.Dispose();
            UserPassword.Password = string.Empty;
        }

        public SecureString Password => UserPassword.SecurePassword;
    }
}
