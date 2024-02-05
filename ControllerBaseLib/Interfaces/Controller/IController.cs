using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.Interfaces.Controller
{
    public interface IController<TOperationType>
        where TOperationType : struct, Enum
    {
        #region Events

        public event Action<object, IOperationFinishedEventArgs<TOperationType>> OnOperationFinished;

        #endregion

        #region Methods

        void ExecuteFunctionAndGetResultThroughEvent(TOperationType operType, Func<object, dynamic> func, object? state = null);

        IOperationFinishedEventArgs<TOperationType> ExecuteFunction(TOperationType operType, Func<object, dynamic> func, object? state = null);

        Task<IOperationFinishedEventArgs<TOperationType>> ExecuteFunctionAction(TOperationType operType, Func<object, dynamic> func, object? state = null);

        Task ExecuteFunctionAndGetResultThroughEventAsync(TOperationType operType, Func<object, CancellationTokenSource, dynamic> func,
            object? state = null, CancellationTokenSource? cts = null);

        #endregion

    }
}
