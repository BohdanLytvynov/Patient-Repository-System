using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLib.Commands.CommandBase;

namespace PatientRep.ViewModelBase.Commands
{
    public class LambdaCommand : CommandBaseClass
    {
        private readonly Action<object> m_execute;

        private readonly Func<object, bool> m_canExecute;
        public override bool CanExecute(object parameter) =>
            m_canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) =>
            m_execute.Invoke(parameter);

        public LambdaCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            m_execute = execute ?? throw new ArgumentNullException(nameof(execute));

            m_canExecute = canExecute;
        }
    }
}
