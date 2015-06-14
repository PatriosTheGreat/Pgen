using System;
using System.Windows.Input;

namespace PgenWindowsClient
{
    public class LambdaCommand : ICommand
    {
        public LambdaCommand(Action<object> lambda)
        {
            _lambda = lambda;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _lambda(parameter);
        }

        public event EventHandler CanExecuteChanged;

        private readonly Action<object> _lambda;
    }
}
