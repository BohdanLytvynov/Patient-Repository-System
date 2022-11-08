﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configuration.ReasonModels.ReasonVisualModel
{
    public class Reason : AdditionalInfoViewModel
    {
        #region Fields

        bool m_DocDependent;

        #endregion

        #region Properties

        public bool DocDependent
        {
            get => m_DocDependent; set => Set(ref m_DocDependent, value, nameof(DocDependent));
        }


        #endregion

        #region Ctor

        public Reason(int number, string value, bool docDependent) : base(number, value)
        {
            m_DocDependent = docDependent;
        }

        #endregion

        #region Methods

        #endregion


    }
}
