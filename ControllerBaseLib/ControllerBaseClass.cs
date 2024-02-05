using ControllerBaseLib.Enums;
using ControllerBaseLib.EventArgs;
using ControllerBaseLib.Interfaces.Controller;

namespace ControllerBaseLib
{
    /// <summary>
    /// Base Class for Controller. This class can be used to create controllers, that can get result of function executing through event or in return value.
    /// TOperType - argument type that is the Enum (Operation Type)
    /// </summary>
    /// <typeparam name="TOperType"></typeparam>
    public abstract class ControllerBaseClass<TOperType> : IController<TOperType>
        where TOperType : struct, Enum
    {       
        #region Events
        /// <summary>
        /// Event that will fire when operation execution finishes with some result  
        /// </summary>
        public event Action<object, IOperationFinishedEventArgs<TOperType>>? OnOperationFinished;

        #endregion
        
        #region Ctor
        public ControllerBaseClass()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute function (func) using arguments (state) synchronously. Result and Operation execution info can be get from event OnOperationFinished
        /// (TOperType) - type of executed operation.
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        public void ExecuteFunctionAndGetResultThroughEvent(TOperType operType, Func<object, dynamic> func, object? state = null)
            
        {
            Exception ex = null;

            Status operStatus = Status.Succed;

            dynamic res = null;

            try
            {
                res = func.Invoke(state);

                operStatus = Status.Succed;
            }
            catch (Exception e)
            {
                operStatus = Status.Failed;

                ex = e;
            }
            finally
            {
                IOperationFinishedEventArgs<TOperType> e = new OperationFinishedEventArgs<TOperType>(operStatus, operType, ex);

                e.Result = res;

                e.OperationType = operType;

                OnOperationFinished.Invoke(this, e);
            }
        }

        /// <summary>
        /// Execute function (func) using arguments (state) synchronously.
        /// (TOperType) - type of executed operation.
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        public IOperationFinishedEventArgs<TOperType> ExecuteFunction(TOperType operType, Func<object, dynamic> func, object? state = null)
        {
            Exception ex = null;

            Status operStatus = Status.Succed;

            dynamic res = null;

            IOperationFinishedEventArgs<TOperType> e = null;

            try
            {
                res = func.Invoke(state);

                operStatus = Status.Succed;
            }
            catch (Exception exc)
            {
                operStatus = Status.Failed;

                ex = exc;
            }
            finally
            {
                e = new OperationFinishedEventArgs<TOperType>(operStatus, operType, ex);

                e.Result = res;

                e.OperationType = operType;                
            }

            return e;
        }

        /// <summary>
        /// Execute function asynchroniously (func) using arguments (state) synchronously.
        /// (TOperType) - type of executed operation.
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        public async Task<IOperationFinishedEventArgs<TOperType>> ExecuteFunctionAction(TOperType operType, Func<object, dynamic> func, object? state = null)
        {
            Exception ex = null;

            Status operStatus = Status.Succed;

            dynamic res = null;

            IOperationFinishedEventArgs<TOperType> e = null;

            await Task.Run(() =>
            {
                try
                {
                    res = func.Invoke(state);

                    operStatus = Status.Succed;
                }
                catch (Exception exc)
                {
                    operStatus = Status.Failed;

                    ex = exc;
                }
                finally
                {
                    e = new OperationFinishedEventArgs<TOperType>(operStatus, operType, ex);

                    e.Result = res;

                    e.OperationType = operType;
                }
            });
            
            return e;
        }

        /// <summary>
        /// Execute function (func) using arguments (state) asynchronously. Cancellation Token (cts) ca be used to cancell operation.
        /// Result and Operation execution info can be get from event OnOperationFinished
        /// (TOperType) - type of executed operation.
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public async Task ExecuteFunctionAndGetResultThroughEventAsync(TOperType operType, Func<object, CancellationTokenSource, dynamic> func,
            object? state = null, CancellationTokenSource? cts = null)
            
        {
            Exception ex = null;

            Status operStatus = Status.Succed;

            dynamic res = null;

            await Task.Run(() =>
            {
                try
                {
                    res = func.Invoke(state, cts);

                    if (cts != null)
                    {
                        if (cts.IsCancellationRequested)
                        {
                            operStatus = Status.Canceled;
                        }                        
                    }
                    else
                    {
                        operStatus = Status.Succed;
                    }
                }
                catch (Exception e)
                {
                    ex = e;

                    operStatus = Status.Failed;
                }
            });

            IOperationFinishedEventArgs<TOperType> e = new OperationFinishedEventArgs<TOperType>(operStatus, operType);

            e.Result = res;

            e.Exception = ex;

            e.OperationType = operType;

            OnOperationFinished.Invoke(this, e);
        }

        #endregion

    }
}