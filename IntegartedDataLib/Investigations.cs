﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegartedDataLib
{
    public class Investigations : IEnumerable<string>
    {
        public static List<string> InvestProperty;

        static Investigations()
        {
            InvestProperty = new List<string> 
            {
                "ФЛГ"
            };
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)InvestProperty).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InvestProperty.GetEnumerator();
        }
    }
}
