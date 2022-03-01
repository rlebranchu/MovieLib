using System;
using System.Windows.Input;

namespace MovieLib.MVVM
{
    public class RelayCommand : ICommand
    {
        #region Fields
        /// <summary>
        /// Méthode à rappeler lorsque méthode RelayCommand.Execute(object) est appelé 
        /// </summary>
        Action<object> _Execute;

        /// <summary>
        /// Méthode à rappeler lorsque méthode RelayCommand.CanExecute(object) est appelé 
        /// </summary>
        Func<object, bool> _CanExecute;
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region Constructors
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _CanExecute = canExecute;
        }
        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            return _CanExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        #endregion
    }
}
