using ControllerBaseLib;
using JsonDataProviderLibDNC;
using System.Text;

namespace NotesExporterLib
{
    public enum NotesExporterToTxtOperations : byte
    { 
        ExportNotes = 1,
        ExportReports
    }

    public class NotesExporterToTxt : ControllerBaseClass<NotesExporterToTxtOperations>
    {        
        #region Methods

        public void Export<TNoteType>(NotesExporterToTxtOperations oper, string path, string fileName, string Header, List<TNoteType> notesForExport)
        {           
            string pathToFile = path + Path.DirectorySeparatorChar + fileName;

            ExecuteFunctionAndGetResultThroughEvent
                (
                    oper, 
                    (obj) =>
                    {
                        if (!File.Exists(pathToFile))
                        {
                            var fs = File.Create(pathToFile);

                            fs.Close();

                            fs.Dispose();
                        }

                        StreamWriter sw = new StreamWriter(pathToFile, false, encoding: new UTF8Encoding());
                        
                        sw.WriteLine(Header);

                        foreach (var item in notesForExport)
                        {
                            sw.WriteLine(item.ToString());
                        }

                        sw.Close();

                        sw.Dispose();

                        return pathToFile;
                    }
                );
        }
        
        #endregion

    }
}