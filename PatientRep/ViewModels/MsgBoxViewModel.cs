using SmartParser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRep.ViewModels
{
    internal class MsgBoxViewModel : ViewModelBaseLib.VM.ViewModelBaseClass
    {
        #region Fields

        private Window m_current;

        private int m_ViberParserTaskStatus;

        private float m_ViberParserProgress;

        #endregion

        #region Properties

        public int ViberParserTaskStatus 
        {
            get=> m_ViberParserTaskStatus; 
            set=> Set(ref m_ViberParserTaskStatus, value, nameof(ViberParserTaskStatus)); 
        }

        public float ViberParserProgress 
        {
            get => m_ViberParserProgress;
            set => Set(ref m_ViberParserProgress, value, nameof(ViberParserProgress)); 
        }

        #endregion

        #region Ctor
        public MsgBoxViewModel(Window current)
        {
            m_current = current;

            m_ViberParserTaskStatus = 0;

            m_ViberParserProgress = 0f;

            ViberParser.OnPartOfTheTaskDone += ViberParser_OnPartOfTheTaskDone;
        }

        private void ViberParser_OnPartOfTheTaskDone(float arg1, int arg2)
        {
            ViberParserTaskStatus = arg2;

            ViberParserProgress = arg1;

            if (ViberParserProgress == 0)
            {
                m_current.Dispatcher.Invoke(() =>
                {
                    m_current.Close();
                });                
            }
        }
        #endregion

        #region Methods

        #endregion
    }
}
