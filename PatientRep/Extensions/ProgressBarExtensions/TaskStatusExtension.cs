using BitSetLibrary;
using IronSoftware.Drawing;
using SmartParser.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        #region Attached properties

        //Here we will use bit set for Task Execution Status: 000:
        //000 - initial state
        //001 - in Progress NR: 0
        //010 - Completed NR: 1
        //100 - Failed NR: 2
        //Number Ranks (from rigth to left) => 0 1 2

        public static int GetTaskExecutionStatus(DependencyObject obj)
        {
            return (int)obj.GetValue(TaskExecutionStatusProperty);
        }

        public static void SetTaskExecutionStatus(DependencyObject obj, int value)
        {
            obj.SetValue(TaskExecutionStatusProperty, value);
        }

        // Using a DependencyProperty as the backing store for TaskExecutionStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskExecutionStatusProperty;



        public static Brush GetSuccessfulFinishedTaskBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SuccessfulFinishedTaskBrushProperty);
        }

        public static void SetSuccessfulFinishedTaskBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SuccessfulFinishedTaskBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for SuccessfulFinishedTaskBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuccessfulFinishedTaskBrushProperty;

        public static Brush GetProcessingTaskBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ProcessingTaskBrushProperty);
        }

        public static void SetProcessingTaskBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(ProcessingTaskBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for ProcessingTaskBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcessingTaskBrushProperty;

        public static Brush GetFaildTaskBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FaildTaskBrushProperty);
        }

        public static void SetFaildTaskBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(FaildTaskBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for FaildTaskBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FaildTaskBrushProperty;

        #endregion

        #region Static ctor

        static TaskStatusExtension()
        {
            #region Attached properties registration

            TaskExecutionStatusProperty =
            DependencyProperty.RegisterAttached("TaskExecutionStatus", typeof(int),
                typeof(TaskStatusExtension), new PropertyMetadata(0, OnTaskExecutionPropertyChanged));

            SuccessfulFinishedTaskBrushProperty =
            DependencyProperty.RegisterAttached("SuccessfulFinishedTaskBrush", typeof(Brush),
                typeof(TaskStatusExtension), new PropertyMetadata(Brushes.Green, SuccessfulFinishedTaskBrushPropertyChanged));

            ProcessingTaskBrushProperty =
            DependencyProperty.RegisterAttached("ProcessingTaskBrush", typeof(Brush),
                typeof(TaskStatusExtension), new PropertyMetadata(Brushes.Orange, OnProcessingTaskBrushPropertyChanged));

            FaildTaskBrushProperty =
            DependencyProperty.RegisterAttached("FaildTaskBrush", typeof(Brush),
                typeof(TaskStatusExtension), new PropertyMetadata(Brushes.Red, OnFaildTaskBrushPropertyChanged));

            #endregion
        }       

        #endregion

        #region On Attached Properties Changed

        #region On Task Execution Status Propperty Changed

        public static void OnTaskExecutionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int bitSet = (int)e.NewValue;

            if (bitSet == 0)
                return;

            var progBar = d as ProgressBar;
            
            if (progBar == null)
                return;

            if (bitSet == BitSet.SetBit(0, 0))
            {
                progBar.Foreground = (Brush)d.GetValue(ProcessingTaskBrushProperty);
            }
            else if (bitSet == BitSet.SetBit(0, 1))
            {
                progBar.Foreground = (Brush)d.GetValue(SuccessfulFinishedTaskBrushProperty);
            }
            else if (bitSet == BitSet.SetBit(0, 2))
            {
                progBar.Foreground = (Brush)d.GetValue(FaildTaskBrushProperty);
            }
            else
            {
                throw new Exception("Incorrect bitset!!!");
            }
        }

        #endregion

        #region On Successful Finished Task Brush Property Changed

        public static void SuccessfulFinishedTaskBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {           
            d.SetValue(SuccessfulFinishedTaskBrushProperty, e.NewValue);
        }

        #endregion

        #region On Processing Task Brush Property Changed

        public static void OnProcessingTaskBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(ProcessingTaskBrushProperty, e.NewValue);
        }

        #endregion

        #region On Faild Task Brush Property Changed

        public static void OnFaildTaskBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(FaildTaskBrushProperty, e.NewValue);
        }

        #endregion

        #endregion



    }
}
