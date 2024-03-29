﻿using Models.Configuration.ReasonModels.ReasonVisualModel;
using System.Collections;

namespace Models.Configuration.IntegratedData
{
    public class Reasons : IEnumerable<string>
    {
        public static List<string> ReasonsProp { get; set; }

        public static Dictionary<string, List<int>> ConfigCodeUsageDictionary;

        static Reasons()
        {
           ReasonsProp = new List<string>();

           ConfigCodeUsageDictionary = new Dictionary<string, List<int>>();
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

        public static string GetReason(string reason)
        {
            if (String.IsNullOrEmpty(reason))
            {
                return String.Empty;
            }

            if (!reason.Contains('['))
            {
                return String.Empty;
            }

            string[] ar = reason.Split('[');

            return ar[0];
        }

        /// <summary>
        /// Gets code from Reason. r - Reason, index - Part of splited array where code is.
        /// </summary>
        /// <param name="r" description="Reason"></param>
        /// <param name="index" description="Part of splited array where code is."></param>
        /// <returns></returns>
        public static int GetCode(string r, int index=1)
        {
            if (String.IsNullOrEmpty(r))
            {
                return -1;
            }

            if (!r.Contains('['))
            {
                return -1;
            }

            string[] ar = r.Split('[');

            int res = 0;

            int.TryParse(ar[index].Trim(']'), out res);

            return res;
        }

        public static bool IsReasonsEqual(string r1, string r2)
        { 
            return GetCode(r1) == GetCode(r2);
        }

        public static bool GetReasonAccordingToCode(int Code, out string reason)
        {
            reason = String.Empty;

            foreach (var item in ReasonsProp)
            {
                if (GetCode(item) == Code)
                {
                    reason = item;

                    return true;
                }
            }

            return false;
        }
    }
}