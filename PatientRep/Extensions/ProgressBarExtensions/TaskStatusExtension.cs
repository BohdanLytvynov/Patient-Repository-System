using IronSoftware.Drawing;
using SmartParser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PatientRep.Extensions.ProgressBarExtensions
{
    internal class TaskStatusExtension
    {

        public static ViberParserTaskStatus GetTaskExecutionStatus(DependencyObject obj)
        {
            return (ViberParserTaskStatus)obj.GetValue(TaskExeecutionStatus);
        }

        public static void SetTaskExecutionStatus(DependencyObject obj, int value)
        {
            obj.SetValue(TaskExeecutionStatus, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskExeecutionStatus =
            DependencyProperty.RegisterAttached("TaskExecutionStatusProperty", typeof(ViberParserTaskStatus), 
                typeof(TaskStatusExtension), new PropertyMetadata(ViberParserTaskStatus.ReadyToStart, 
                    new PropertyChangedCallback(OnTaskExecutionStatusPropertyChanged)));

        public static void OnTaskExecutionStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progBar = d as ProgressBar;

            if (progBar == null)
            {
                return;
            }

            var newValue = (ViberParserTaskStatus)e.NewValue;

            switch (newValue)
            {
                case ViberParserTaskStatus.Successfully_Done:
                    break;
                case ViberParserTaskStatus.Failed:
                    progBar.Foreground = Brushes.Red;
                    break;
                case ViberParserTaskStatus.InProgress:
                    progBar.Foreground = Brushes.Orange;
                    break;
                case ViberParserTaskStatus.ReadyToStart:
                    progBar.Foreground= Brushes.Green;
                    break;
                default:
                    break;
            }
        }
    }
}
