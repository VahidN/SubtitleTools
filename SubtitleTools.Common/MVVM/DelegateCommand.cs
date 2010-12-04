using System;
using System.Windows.Input;

namespace SubtitleTools.Common.MVVM
{
    // From http://johnpapa.net/silverlight/5-simple-steps-to-commanding-in-silverlight/
    public class DelegateCommand<T> : ICommand
    {
        #region Fields (3)

        readonly Func<T, bool> _canExecute;
        bool _canExecuteCache;
        readonly Action<T> _executeAction;

        #endregion Fields

        #region Constructors (1)

        public DelegateCommand(Action<T> executeAction,
            Func<T, bool> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region Delegates and Events (1)

        // Events (1) 

        public event EventHandler CanExecuteChanged;

        #endregion Delegates and Events

        #region Methods (2)

        // Public Methods (2) 

        public bool CanExecute(object parameter)
        {
            bool temp = _canExecute((T)parameter);

            if (_canExecuteCache != temp)
            {
                _canExecuteCache = temp;

                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return _canExecuteCache;
        }

        public void Execute(object parameter)
        {
            _executeAction((T)parameter);
        }

        #endregion Methods
    }
}
