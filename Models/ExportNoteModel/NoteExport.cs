using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ExportNoteModel
{
    public class NoteExport
    {
        #region Properties

        public int Number { get; set; }

        public string Surename { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Center { get; set; }

        public DateTime RegistrationDate { get; set; }
        #endregion

        #region Ctor
        public NoteExport(int number, string surename, string name, string lastname, string center, DateTime registrationDate)
        {
            Number = number;
            Surename = surename;
            Name = name;
            Lastname = lastname;
            Center = center;
            RegistrationDate = registrationDate;    
        }
        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{Number}) {Surename} {Name} {Lastname} {RegistrationDate.ToShortDateString()} | Центр: {Center}";
        }

        #endregion
    }
}
