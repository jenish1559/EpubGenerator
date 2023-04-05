using System;
using System.Windows.Input;

namespace EPubGenerator.Command
{
	public class RelayCommand : ICommand
	{
		#region Field

		Action<object> executeAction;
        Func<object, bool> canExecute;
        bool canExecuteCache;

		#endregion

		#region Method

		public RelayCommand(Action<object> executeAction, Func<object, bool> canExecute = null, bool canExecuteCache = true)
        {
            this.canExecute = canExecute;
            this.executeAction = executeAction;
            this.canExecuteCache = canExecuteCache;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            else
            {
                return canExecute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }

        #endregion

        #region Event

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

		#endregion
    }
}
