﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configuration.ReasonModels.ReasonStorageModel
{
    public class ReasonStorageModel
    {
        #region Properties

        public int Code { get; set; }

        public string TextValue { get; set; }

        public bool DocDependent { get; set; }

        #endregion

        #region Ctor

        public ReasonStorageModel(int code, string txtValue, bool docDep)
        {
            Code = code;

            TextValue = txtValue;

            DocDependent = docDep;
        }

        #endregion
    }
}
