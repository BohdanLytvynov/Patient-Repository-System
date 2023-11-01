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
    
        readonly Regex m_Surename_Name_Lastname;

        #endregion

        #region Maintainance Fields

        bool m_CodeFound = false;

        bool m_SNLFound = false;

        #endregion

        #region Properties

        public bool SNLFound { get => m_SNLFound; set => m_SNLFound = value; }

        public bool CodeFound { get => m_CodeFound; set => m_CodeFound = value; }

        #endregion

        #region Ctor

        public OCRResultParser(Regex Code, Regex Surename_Name_Laastname)
        {
            m_code = Code;
           
            m_Surename_Name_Lastname = Surename_Name_Laastname;
        }

        #endregion

        #region Methods
        
        public string[] Parse(OcrResult res)
        {
            string[] r = new string[4];
                        
            if (res != null)
                foreach (var paragraph in res.Paragraphs)
                {
                    if (paragraph.Text.Length == 0)
                        continue;

                    //SNL Found
                    if ((m_Surename_Name_Lastname.IsMatch(paragraph.Text))                    
                    && !m_SNLFound)
                    {
                        int start = (paragraph.Words.Length < 3) ? paragraph.Words.Length - 1 : 0;

                        int mod = (start == 0) ? 0 : 1;

                        for (; start < 3; start++)
                        {
                            r[start] = paragraph.Words[start - mod].Text;
                        }

                        m_SNLFound = true;
                    }
                    else if (m_code.IsMatch(RewriteFromChars(paragraph.Characters)) && !m_CodeFound)
                    {
                        r[r.Length - 1] = RewriteFromChars(paragraph.Characters);

                        m_CodeFound = true;
                    }

                    if (m_SNLFound && m_CodeFound)
                        break;
                }

            return r;
        }

        private string RewriteFromChars(IEnumerable<Character> chars)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in chars)
            {
                sb.Append(item.Text);
            }

            return sb.ToString();
        }

        #endregion
    }
}
