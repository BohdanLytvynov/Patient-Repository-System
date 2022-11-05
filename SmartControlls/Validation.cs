using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartControlls
{
    public static class Validation
    {
        public static bool ValidateAllCode(string code)
        {
            if (String.IsNullOrEmpty(code))
            {                
                return false;
            }

            var ar = code.Split('-');

            if (ar.Length != 4)
            {               
                return false;
            }

            for (int i = 0; i < ar.Length; i++)
            {
                for (int j = 0; j < ar[i].Length; j++)
                {
                    if (!Char.IsDigit(ar[i][j]))
                    {                       
                        return false;
                    }
                }
            }

            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i].Length != 4)
                {                    
                    return false;
                }
            }            

            return true;
        }

        public static bool CheckValidArray(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateCode(string code, out string error, int txtbxIndex)
        {
            if (String.IsNullOrEmpty(code))
            {
                error = "Поле не має бути порожнім!";

                return false;
            }

            if (code.Length != 4)
            {
                error = $"Невірна кількість цифр в {txtbxIndex} группі";

                return false;
            }

            for (int j = 0; j < code.Length; j++)
            {
                if (!Char.IsDigit(code[j]))
                {
                    error = $"{txtbxIndex} группа цифр містить невірний символ під індексом {j + 1}!!";

                    return false;
                }
            }
                                      
            error = "";

            return true;
        }

        public static bool ValidateNumber(string number, out string error, Func<string, string, bool> AddCondition = null)
        {
            error = String.Empty;

            if (String.IsNullOrWhiteSpace(number))
            {
                error = "Порожнє поле!";

                return false;
            }

            for (int i = 0; i < number.Length; i++)
            {
                if (!Char.IsDigit(number[i]))
                {
                    error = $"Невірний символ під індексом {i + 1}";

                    return false;
                }
            }
           
            if (AddCondition != null)
            {              
                if (!AddCondition.Invoke(error, number))
                {                    
                    return false;
                }
                
            }
           
            return true;
        }
    }
   
}
