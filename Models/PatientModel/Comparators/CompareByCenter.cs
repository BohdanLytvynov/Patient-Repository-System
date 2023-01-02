using Models.PatientModel.PatientVisualModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.PatientModel.Comparators
{
    public class CompareByCenter : IComparer<Patient>
    {
        public int Compare(Patient? x, Patient? y)
        {
            return x.Center.CompareTo(y.Center);
        }        
    }
}
