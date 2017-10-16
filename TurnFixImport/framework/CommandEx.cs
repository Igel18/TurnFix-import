namespace TurnFixImport.framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    public class CommandEx : ICommand
    {
        readonly Func<Boolean> _canExecute;
        readonly Action<object> _execute;

        public CommandEx(Action<object> execute) : this(execute, null)
        {

        }

        public CommandEx(Action<object> execute, Func<Boolean> canExecute)
        {

            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {

                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(Object parameter)
        {
            _execute(parameter);
        }
    }
}
