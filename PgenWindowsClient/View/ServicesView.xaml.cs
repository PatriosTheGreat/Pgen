using System.Security;
using System.Windows.Controls;

namespace PgenWindowsClient.View
{
    /// <summary>
    /// Interaction logic for ServicesView.xaml
    /// </summary>
    public partial class ServicesView : Page, IPasswordProvider
    {
        public ServicesView()
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
