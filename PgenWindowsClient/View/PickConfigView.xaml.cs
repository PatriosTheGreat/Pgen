using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PgenWindowsClient.ViewModel;

namespace PgenWindowsClient.View
{
    /// <summary>
    /// Interaction logic for PickConfigView.xaml
    /// </summary>
    public partial class PickConfigView : UserControl
    {
        public PickConfigView()
        {
            InitializeComponent();
        }

        private void ShowFileDialog(object sender, RoutedEventArgs e)
        {
            var configViewModel = (PickConfig.DataContext as PickConfigViewModel);

            var pickFileDialog = new OpenFileDialog
            {
                Filter = configViewModel.FileFilter,
                CheckFileExists = false
            };

            var userClickedOk = pickFileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                configViewModel.PathToConfig = pickFileDialog.FileName;
            }
        }
    }
}
