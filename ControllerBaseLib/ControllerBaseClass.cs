using ControllerBaseLib.Enums;
using ControllerBaseLib.EventArgs;
using System.Buffers;

namespace ControllerBaseLib
{
    public abstract class ControllerBaseClass<TOperType>
        where TOperType : struct, Enum
    {
        #region Delegates

        public delegate void OnOperationFinishedDelegate(object s, OperationFinishedEventArgs<TOperType> e);

        #endregion

        #region Events

        public event OnOperationFinishedDelegate OnOperationFinished;

        #endregion
        
        #region Ctor
        public ControllerBaseClass()
        {

        }
        #endregion

        #region Methods

        public void ExecuteFunctionAdnGetResultThroughEvent(TOperType operType, Func<object, dynamic> func, object state = null)
            
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
                OperationFinishedEventArgs<TOperType> e = new OperationFinishedEventArgs<TOperType>(operStatus, operType);

                e.Result = res;

                e.OperationType = operType;

                OnOperationFinished.Invoke(this, e);
            }
        }

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