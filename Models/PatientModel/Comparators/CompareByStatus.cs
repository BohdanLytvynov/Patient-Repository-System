using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.PatientModel.PatientVisualModel;

namespace Models.Comparators
{
    public class CompareByStatus : IComparer<Patient>
    {
        public int Compare(Patient? x, Patient? y)
        {
            return x.Status.CompareTo(y.Status);
        }
    }
}
