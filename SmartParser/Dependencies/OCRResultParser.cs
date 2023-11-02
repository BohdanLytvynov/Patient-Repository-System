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

        readonly Regex m_Surename_Name;

        readonly Regex m_1Word;

        #endregion

        #region Maintainance Fields

        bool m_CodeFound = false;

        bool m_surenameFound = false;

        bool m_nameFound = false;

        bool m_lastnameFound = false;

        #endregion

        #region Properties

        public bool CodeFound { get => m_CodeFound; }

        public bool SurenameFound { get=> m_surenameFound;  }

        public bool NameFound { get=> m_nameFound;  }

        public bool LastnameFound { get=> m_surenameFound; }

        #endregion

        #region Ctor

        public OCRResultParser(Regex Code, Regex Surename_Name_Lastname,
            Regex SurenameName, Regex Word1)
        {
            m_code = Code;
           
            m_Surename_Name_Lastname = Surename_Name_Lastname;

            m_Surename_Name = SurenameName;

            m_1Word = Word1;
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
                    && !(m_surenameFound && m_lastnameFound && m_nameFound))
                    {
                        if(paragraph.Words.Length == 3)
                        for (int i = 0; i < paragraph.Words.Length; i++)
                        {
                            r[i] = paragraph.Words[i].Text;
                        }

                        m_surenameFound = true;

                        m_nameFound = true;

                        m_lastnameFound = true;
                    }
                    else if (m_Surename_Name.IsMatch(paragraph.Text)
                        && !(m_surenameFound && m_nameFound))
                    {
                        if(paragraph.Words.Length == 2)
                        for (int i = 0; i < paragraph.Words.Length; i++)
                        {
                            r[i] = paragraph.Words[i].Text;
                        }

                        m_surenameFound = true;

                        m_nameFound = true;
                    }
                    else if (m_1Word.IsMatch(paragraph.Text))
                    {
                        if (paragraph.Words.Length == 1)
                        {
                            r[2] = paragraph.Words[0].Text;

                            m_lastnameFound = true;
                        }
                        else
                        {
                            foreach (var word in paragraph.Words)
                            {
                                if (m_code.IsMatch(RewriteFromChars(word.Characters)))
                                {
                                    r[r.Length - 1] = RewriteFromChars(word.Characters);

                                    m_CodeFound = true;
                                }
                            }
                        }
                    }
                    else if (m_code.IsMatch(RewriteFromChars(paragraph.Characters)) && !m_CodeFound)
                    {
                        r[r.Length - 1] = RewriteFromChars(paragraph.Characters);

                        m_CodeFound = true;
                    }
                    
                    if (m_surenameFound && m_nameFound && m_lastnameFound && m_CodeFound)
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

        public void ClearSearchFlags()
        {
            m_CodeFound = false;

            m_surenameFound = false;

            m_nameFound = false;

            m_lastnameFound = false;
        }

        public bool AllFound()
        {
            return m_surenameFound && m_nameFound && m_lastnameFound && m_CodeFound;
        }

        #endregion
    }
}
