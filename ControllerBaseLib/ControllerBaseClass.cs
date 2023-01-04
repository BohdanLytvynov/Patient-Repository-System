using ControllerBaseLib.Enums;
using ControllerBaseLib.EventArgs;
using System.Buffers;

namespace ControllerBaseLib
{
    /// <summary>
    /// Base Class for Controller. This class can be used to create controllers, that can get result of function executing through event or in return value.
    /// TOperType - argument type that is the Enum (Operation Type)
    /// </summary>
    /// <typeparam name="TOperType"></typeparam>
    public abstract class ControllerBaseClass<TOperType>
        where TOperType : struct, Enum
    {
        #region Delegates
        /// <summary>
        /// Delegate that is used to create event On Operation Finished
        /// s - object who fired the event
        /// e - OperationFinishedEventArgs - used to deliver operation execution info.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public delegate void OnOperationFinishedDelegate(object s, OperationFinishedEventArgs<TOperType> e);

        #endregion

        #region Events
        /// <summary>
        /// Event that will fire when operation execution finishes with some result  
        /// </summary>
        public event OnOperationFinishedDelegate OnOperationFinished;

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
        public void ExecuteFunctionAndGetResultThroughEvent(TOperType operType, Func<object, dynamic> func, object state = null)
            
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
                OperationFinishedEventArgs<TOperType> e = new OperationFinishedEventArgs<TOperType>(operStatus, operType, ex);

                e.Result = res;

                e.OperationType = operType;

                OnOperationFinished.Invoke(this, e);
            }
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
            object state = null, CancellationTokenSource cts = null)
            
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

            OperationFinishedEventArgs<TOperType> e = new OperationFinishedEventArgs<TOperType>(operStatus, operType);

            e.Result = res;

            e.Exception = ex;

            e.OperationType = operType;

            OnOperationFinished.Invoke(this, e);
        }

        #endregion

    }
}