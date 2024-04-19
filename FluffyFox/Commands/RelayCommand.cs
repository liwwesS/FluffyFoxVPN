using System.Windows.Input;

namespace FluffyFox.Commands;

public class RelayCommand<T> : ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
        _execute = execute;
    }

    private RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

public class RelayCommand : ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute)
        : this(execute, null)
    {
        _execute = execute;
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

	public bool CanExecute(object parameter)
	{
		return _canExecute == null || _canExecute(parameter);
	}

	public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}