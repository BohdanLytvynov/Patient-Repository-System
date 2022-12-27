using ControllerBaseLib;
using System.Text;

namespace NotesExporterLib
{
    public enum NotesExporterOperations : byte
    { 
        ExportNotes = 1,
        ExportReports
    }

    public class NotesExporter : ControllerBaseClass<NotesExporterOperations>
    {        
        #region Methods

        public void Export<TNoteType>(NotesExporterOperations oper, string filePath, List<TNoteType> notesForExport)
        {
            ExecuteFunctionAdnGetResultThroughEvent
                (
                    oper, 
                    (obj) =>
                    {
                        StreamWriter sw = new StreamWriter(filePath, false, encoding: new UTF8Encoding());

                        sw.Write(obj);

                        foreach (var item in notesForExport)
                        {

                        }               

                        return null;
                    }
                );
        }
        
        #endregion

    }
}