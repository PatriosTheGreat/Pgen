using System.Windows.Input;
using GenerationCore;
using GenerationCore.ServicesManager;

namespace PgenWindowsClient.ViewModel
{
    public sealed class AddServiceViewModel : ViewModelBase
    {
        public AddServiceViewModel(
            IServicesManager servicesManager,
            IPageNavigator navigator)
        {
            _servicesManager = servicesManager;
            _navigator = navigator;

            Submit = new LambdaCommand(_ =>
            {
                _servicesManager.SaveService(
                    new ServiceInformation(
                        ServiceName, 
                        new PasswordRestriction(
                            CollectSymbolTypes(),
                            PasswordMinBounder,
                            PasswordMaxBounder)));

                _navigator.NavigateToServicesPage();
            });

            Cancel = new LambdaCommand(_ =>
            {
                _navigator.NavigateToServicesPage();
            });

            ServiceName = "DefaultName";
            PasswordMinBounder = PasswordRestriction.PasswordMinBounder;
            PasswordMaxBounder = PasswordRestriction.PasswordMaxBounder;
            AllowLowLatin = true;
            AllowUpperLatin = true;
        }

        public bool AllowLowLatin
        {
            get { return _allowLowLatin; }
            private set
            {
                _allowLowLatin = value; 
                OnPropertyChanged();
            }
        }

        public bool AllowUpperLatin
        {
            get { return _allowUpperLatin; }
            private set
            {
                _allowUpperLatin = value;
                OnPropertyChanged();
            }
        }

        public string ServiceName
        {
            get { return _serviceName; }

            set
            {
                _serviceName = value;
                OnPropertyChanged();
            }
        }

        public int PasswordMinBounder
        {
            get { return _passwordMinBounder; }

            set
            {
                _passwordMinBounder = value;
                OnPropertyChanged();
            }
        }

        public int PasswordMaxBounder
        {
            get { return _passwordMaxBounder; }

            set
            {
                _passwordMaxBounder = value;
                OnPropertyChanged();
            }
        }

        public ICommand Submit
        {
            get { return _submit; }

            set
            {
                _submit = value;
                OnPropertyChanged();
            }
        }

        public ICommand Cancel
        {
            get { return _cancel; }

            set
            {
                _cancel = value;
                OnPropertyChanged();
            }
        }

        private SymbolsType CollectSymbolTypes()
        {
            var allawedSymbols = SymbolsType.Digital;

            if (AllowLowLatin)
            {
                allawedSymbols |= SymbolsType.LowcaseLatin;
            }

            if (AllowUpperLatin)
            {
                allawedSymbols |= SymbolsType.UpcaseLatin;
            }

            return allawedSymbols;
        }

        private ICommand _submit;
        private ICommand _cancel;
        private readonly IServicesManager _servicesManager;
        private readonly IPageNavigator _navigator;
        private string _serviceName;
        private int _passwordMinBounder;
        private int _passwordMaxBounder;
        private bool _allowLowLatin;
        private bool _allowUpperLatin;
    }
}
