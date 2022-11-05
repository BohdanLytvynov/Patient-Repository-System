using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegartedDataLib
{
    public class Physicians : IEnumerable<string>
    {
        public static string [] DoctorsProp { get; set; }
        static Physicians()
        {
            DoctorsProp = new string []
            { "Doctor 1" };
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)DoctorsProp).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return DoctorsProp.GetEnumerator();
        }
    }
}
