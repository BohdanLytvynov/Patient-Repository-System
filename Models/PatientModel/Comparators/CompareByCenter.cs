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
            uint n1 = 0;

            uint n2 = 0;
            
            if (uint.TryParse(x.Center, out n1) && uint.TryParse(y.Center, out n2))
            { 
                return n1.CompareTo(n2);
            }

            return 0;
        }        
    }
}
