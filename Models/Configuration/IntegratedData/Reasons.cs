using Models.Configuration.ReasonModels.ReasonVisualModel;
using System.Collections;

namespace Models.Configuration.IntegratedData
{
    public class Reasons : IEnumerable<string>
    {
        public static List<Reason> ReasonsProp { get; set; }

        public static List<int> ShowDocIndexes;

        static Reasons()
        {
            ReasonsProp = new List<Reason>();

            ShowDocIndexes = new List<int>();
            //{
            //    "1) Відсутня декларація.",
            //    "2) Не було огляду терапевта.", //need add info about patient
            //    "3) Поза роб. часом терапевта.",                
            //    "4) Не працювала система.",                
            //    "5) Контроль лікування",
            //    "6) Наявність скарг",                
            //    ////////////////////////////////////(Codes that controls visibility of doctor's field)  need add info about patient
            //    "7) Погоджено з завідуючим:",
            //    "8) Направлення від:",
            //    "9) Не виписано єл. направ.:",
            //    /////////////////////////////////
            //    "10) Переносний рентген (РВ)",
            //    "11) Планове дослідження"


            //}; 
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)ReasonsProp).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ReasonsProp.GetEnumerator();
        }

        public static int GetCode(string r, int index=0)
        {
            if (String.IsNullOrEmpty(r))
            {
                return -1;
            }

            if (!r.Contains(')'))
            {
                return -1;
            }

            string[] ar = r.Split(')');

            int res = 0;

            int.TryParse(ar[index], out res);

            return res;
        }

        public static bool CompareReasons(string r1, string r2)
        { 
            return GetCode(r1) == GetCode(r2);
        }
    }
}