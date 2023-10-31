using IronOcr;
using SmartParser.Dependencies.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IronOcr.OcrResult;

namespace SmartParser.Dependencies
{
    public class OCRResultParser : IOCRResultParser<string[]>
    {
        #region Fields

        readonly Regex m_code;

        readonly Regex m_Name_or_Surename_or_Lastname;

        readonly Regex m_Surename_Name;

        readonly Regex m_Surename_Name_Lastname;

        #endregion

        #region Ctor

        public OCRResultParser(Regex Code, Regex Name_or_Surename_or_Lastname,
            Regex Surename_Name, Regex Surename_Name_Laastname)
        {
            m_code = Code;

            m_Name_or_Surename_or_Lastname = Name_or_Surename_or_Lastname;

            m_Surename_Name = Surename_Name;

            m_Surename_Name_Lastname = Surename_Name_Laastname;
        }

        #endregion

        #region Methods

        public async Task<string[]> ParseAsync(IEnumerable<OcrResult> res)
        {
            string[] r = new string[4];

            await Task.Run(() =>
            {
                //Debug.WriteLine("In Method!!");

                bool CodeCorrect = false;

                bool SNLFound = false;

                if (res != null)
                    foreach (var ocrResult in res)
                    {
                        if (ocrResult.Text.Length == 0)
                            continue;

                        #region Old version

                        //var Lines = ocrResult.Lines;

                        //foreach (var item in Lines)
                        //{
                        //    if (!CodeCorrect)
                        //    {
                        //        foreach (var word in item.Words)
                        //        {
                        //            CodeCorrect = m_code.IsMatch(word.Text);

                        //            if (CodeCorrect)// El refferal was found
                        //            {
                        //                code = word.Text;

                        //                break;
                        //            }
                        //        }
                        //    }

                        //    if (!SNFound)
                        //    {
                        //        SNFound = AreWordsValid(item.Words, out snl, 2);
                        //    }

                        //    if (!LastNameFound)
                        //    {
                        //        LastNameFound = AreWordsValid(item.Words, out l, 1);
                        //    }

                        //    if (CodeCorrect && SNFound && LastNameFound)
                        //    {
                        //        break;
                        //    }
                        //}

                        //if (CodeCorrect && SNFound && LastNameFound)
                        //{
                        //    r[0] = snl[0];

                        //    r[1] = snl[1];

                        //    r[2] = l[0];

                        //    r[3] = code;
                        //}

                        #endregion
                        //SNL Found
                        if ((m_Surename_Name_Lastname.IsMatch(ocrResult.Text)
                        || m_Surename_Name.IsMatch(ocrResult.Text))
                        && !SNLFound)
                        {
                            int start = (ocrResult.Words.Length <= 3) ? ocrResult.Words.Length - 1 : 0;

                            //int mod = (start==0)? 0 : 

                            for (; start < 3; start++)
                            {
                                r[start] = ocrResult.Words[start].Text;
                            }

                            SNLFound = true;
                        }
                        else if (m_code.IsMatch(ocrResult.Text) && !CodeCorrect)
                        {
                            r[r.Length - 1] = ocrResult.Text;

                            CodeCorrect = true;
                        }

                        if (SNLFound && CodeCorrect)
                            break;
                    }

            });

            return r;
        }

        public string[] Parse(IEnumerable<OcrResult> res)
        {
            string[] r = new string[4];

            //Debug.WriteLine("In Method!!");

            bool CodeCorrect = false;

            bool SNLFound = false;

            if (res != null)
                foreach (var ocrResult in res)
                {
                    if (ocrResult.Text.Length == 0)
                        continue;

                    //SNL Found
                    if ((m_Surename_Name_Lastname.IsMatch(ocrResult.Text)
                    || m_Surename_Name.IsMatch(ocrResult.Text))
                    && !SNLFound)
                    {
                        int start = (ocrResult.Words.Length < 3) ? ocrResult.Words.Length - 1 : 0;

                        int mod = (start == 0) ? 0 : 1;

                        for (; start < 3; start++)
                        {
                            r[start] = ocrResult.Words[start - mod].Text;
                        }

                        SNLFound = true;
                    }
                    else if (m_code.IsMatch(RewriteFromChars(ocrResult)) && !CodeCorrect)
                    {
                        r[r.Length - 1] = RewriteFromChars(ocrResult);

                        CodeCorrect = true;
                    }

                    if (SNLFound && CodeCorrect)
                        break;
                }

            return r;
        }

        private string RewriteFromChars(OcrResult res)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in res.Characters)
            {
                sb.Append(item.Text);
            }

            return sb.ToString();
        }

        #endregion
    }
}
