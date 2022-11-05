using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    public static class Functions
    {
        public static bool Contains(string strSource, string strSearch)
        {
            if (strSource == null && strSearch == null)
            {
                return false;
            }

            bool flag = false;

            bool iterStop = false;

            int strSourceLength = strSource.Length;

            int strSearchLength = strSearch.Length;

            int Jtemp = 0;

            for (int i = 0; i < strSourceLength; i++) // Iterate Search
            {
                for (int j = Jtemp; j < strSearchLength; j++) // iterate Source
                {
                    if (j == strSearchLength)
                    {
                        iterStop = true;

                        break;
                    }

                    if (strSource[i].ToString().Equals(strSearch[j].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }

                    Jtemp = j + 1;

                    break;
                }

                if (iterStop)
                {
                    break;
                }
            }

            return flag;
        }
    }
}
