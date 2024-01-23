using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRep.Enums
{
    public enum PatientRepDataProviderOperations : byte
    {
        SavePatientsDB = 1, LoadPatientsDB, SaveHistoryNotesDb, LoadHistoryNotesDb, LoadSettings, SaveSettings
    }
}
