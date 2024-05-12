using System.Windows.Input;

namespace ViewModel
{
    public class Commands : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler? CanExecuteChanged;
        public Commands(Action execute) : this(execute, null) { }

        public Commands(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }



        public bool CanExecute(object parameter)
        {
            if (canExecute == null) return true;
            if (parameter == null) return canExecute();

            return canExecute();
        }

        public virtual void Execute(object parameter)
        {
            this.execute();
        }

        internal void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
