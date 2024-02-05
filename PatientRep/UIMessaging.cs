using ControllerBaseLib.Enums;
using ControllerBaseLib.EventArgs;
using ControllerBaseLib.Interfaces.Controller;
using NotesExporterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRep
{
    public static class UIMessaging
    {
        public static MessageBoxResult CreateMessageBox(string message, string title, MessageBoxButton button, MessageBoxImage img)
        {
            return MessageBox.Show(message, title, button, img);
        }

        public static string GetExceptionsRecursive(Exception ex, string prevmessage = "")
        {            
            if (ex.InnerException != null)
            {
                prevmessage = GetExceptionsRecursive(ex.InnerException, prevmessage);
            }

            prevmessage += $"\n\t-> {ex.Message} \nStack Trace: {ex.StackTrace}";

            return prevmessage;            
        }

        public static MessageBoxResult CreateMessageBoxAccordingToResult<TOperationType>(IOperationFinishedEventArgs<TOperationType> e, string MsgBoxtitle, 
            Action ExecuteIfOperationSucceded, bool enableCancelMsg = true, bool enableFailMsg = true)
            where TOperationType : struct, Enum
        {
            MessageBoxResult result = MessageBoxResult.OK;

            if (e.ExecutionStatus == Status.Succed)
            {
                ExecuteIfOperationSucceded?.Invoke();
            }
            else if (e.ExecutionStatus == Status.Canceled) // Operation Canceled
            {
                if (enableCancelMsg)
                {
                    result = CreateMessageBox($"Operation: {e.OperationType} was {e.ExecutionStatus}", MsgBoxtitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }                
            }
            else
            {
                if (enableFailMsg)
                {
                    result = CreateMessageBox($"Operation: {e.OperationType} {e.ExecutionStatus} cause: {GetExceptionsRecursive(e.Exception)}",
                    MsgBoxtitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }

            return result;
        }

        
    }
}
