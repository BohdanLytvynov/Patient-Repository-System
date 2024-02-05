using ControllerBaseLib;
using IronOcr;
using SmartParser.Dependencies.Interfaces;
using System.Diagnostics;
using System.Text;
using JsonDataProviderLibDNC.Interfaces;
using SmartParser.Comparators;
using IronSoftware.Drawing;
using BitSetLibrary;
using ControllerBaseLib.Interfaces.Controller;

namespace SmartParser.Parsers
{    
    public enum ViberParserOperations : byte
    {
        Parse = 0
    }

    public enum ViberParserDataProviderOperations : byte
    {
        WriteToTemp = 0, ReadFromTemp
    }

    public class ViberParserResult
    {
        #region Properties

        public IEnumerable<string> SuccessfullyRead { get; }

        public string FailedToReadPaths { get; }

        public bool Fail { get; }

        #endregion

        #region Ctor

        public ViberParserResult(IEnumerable<string> successfullyRead,
            string failedToRead,
            bool fail)
        {
            SuccessfullyRead = successfullyRead;

            FailedToReadPaths = failedToRead;
            Fail = fail;
        }

        #endregion

        public override string ToString()
        {
            var str = String.Empty;

            str += $"Fail: {Fail}\n";

            foreach (var item in SuccessfullyRead)
            {
                str += $"{item}\n";
            }

            str += $"FailPath: {FailedToReadPaths}\n";

            return str;
        }
    }

    public class ViberParserTemp
    {
        public string? ReadFileName { get; set; }

        public DateTime ReadFileCreationDate { get; set; }

        public bool MoreThenFirstTime { get; set; }

        public int CurrentImagesCount { get; set; }

        public ViberParserTemp(string readFileName, DateTime readFileCreationDate, bool moreThenFirstTime, int currentImagesCount)
        {
            ReadFileName = readFileName;
            ReadFileCreationDate = readFileCreationDate;
            MoreThenFirstTime = moreThenFirstTime;
            CurrentImagesCount = currentImagesCount;
        }

        public ViberParserTemp()
        {

        }
    }

    public class ViberParser : ControllerBaseClass<ViberParserOperations>,
        ISmartParser<string>
    {
        #region Viber Parser Events

        public static event Action<float, int>? OnPartOfTheTaskDone;

        #endregion

        #region Fields

        IDataProvider<ViberParserDataProviderOperations> m_dataProvider;

        private OCR m_OCR;

        private IOCRResultParser<string[]> m_OCRResultParser;

        //private CancellationTokenSource m_cts;

        private ViberParserTemp? m_temp;

        private string m_pathToTemp;

        Action<OcrInput>? m_OCRInputPreprocessors;

        bool m_isRunning;

        float m_prog_value;

        #endregion

        #region Properties

        public ViberParserTemp TempData { get => m_temp; }

        public string? PathToDebuggingFolder { get; set; }

        public bool IsRunning { get => m_isRunning; }

        #endregion

        #region Ctor

        public ViberParser(
            IOCRResultParser<string[]> OCRresParser,
            OCR OCR, IDataProvider<ViberParserDataProviderOperations> dataProvider,            
            Action<OcrInput>? OCRInputPreprocessor_For_Crops_Reader = null,
            Action<OcrInput>? OCRInputPreprocessor_For_Ordinay_Reader = null)
        {           
            m_isRunning = false;

            if (OCRInputPreprocessor_For_Crops_Reader != null)
            {
                m_OCRInputPreprocessors = OCRInputPreprocessor_For_Crops_Reader;
            }

            if (OCRInputPreprocessor_For_Ordinay_Reader != null)
            {
                Delegate.Combine(m_OCRInputPreprocessors, OCRInputPreprocessor_For_Ordinay_Reader);
            }

            if (OCRresParser == null)
                throw new ArgumentNullException("OCRresParser");

            if (OCR == null)
                throw new ArgumentNullException("OCR");

            if (dataProvider == null)
                throw new ArgumentNullException("dataProvider");

            m_OCRResultParser = OCRresParser;

            m_prog_value = 0f;

            m_OCR = OCR;

            m_dataProvider = dataProvider;

            var temp = m_dataProvider as ControllerBaseClass<ViberParserDataProviderOperations>;

            if (temp != null)
                temp.OnOperationFinished += ViberParser_OnOperationFinished;

            CreateTempFile();

            m_dataProvider.LoadFile<ViberParserTemp>(m_pathToTemp, new ViberParserTemp(), ViberParserDataProviderOperations.ReadFromTemp);
        }

        private void ViberParser_OnOperationFinished(object s, IOperationFinishedEventArgs<ViberParserDataProviderOperations> e)
        {
            if (e.ExecutionStatus == ControllerBaseLib.Enums.Status.Succed)
            {
                switch (e.OperationType)
                {
                    case ViberParserDataProviderOperations.ReadFromTemp:

                        if (e.Result != null)
                        {
                            m_temp = e.Result;
                        }
                        else
                        {
                            m_temp = new ViberParserTemp("", new DateTime(), false, 0);
                        }

                        break;
                }
            }
            else
            {
                //Error occurred
            }
        }

        #endregion

        #region Methods

        private void CreateTempFile()
        {
            var envPath = Environment.CurrentDirectory;

            var path_to_temp_dir = envPath + Path.DirectorySeparatorChar + @"Temp";

            m_dataProvider.IfDirectoryNotExistsCreateIt(path_to_temp_dir);

            var path_to_temp_file = path_to_temp_dir + Path.DirectorySeparatorChar + @"temp." +
            m_dataProvider.FileExtension;

            m_dataProvider.IfFileNotExistsCreateIt(path_to_temp_file);

            m_pathToTemp = path_to_temp_file;
        }

        public void ParseImages(FileInfo[] img_pathes, CancellationTokenSource cts = null)
        {
            //Here we will use bit set for Task Execution Status: 000:
            //000 - initial state
            //001 - in Progress NR: 0
            //010 - Completed NR: 1
            //100 - Failed NR: 2
            //Number Ranks (from rigth to left) => 0 1 2

            if (img_pathes == null)
                throw new NullReferenceException("img_pathes");

            if (img_pathes.Count() == 0)
                return;

            //Decide what to parse

            if (cts.IsCancellationRequested)
            {
                m_isRunning = !cts.IsCancellationRequested;

                return;
            }

            m_isRunning = true;

            Array.Sort<FileInfo>(img_pathes, new FileInfo_Comparators.CompareByCreationTime());

            int i = -1;

            if (m_temp.MoreThenFirstTime)//Some files were already written
            {
                var count = img_pathes.Length;

                for (int j = 0; j < count; j++)
                {
                    if (img_pathes[j].Name == m_temp.ReadFileName && img_pathes[j].CreationTime == m_temp.ReadFileCreationDate)
                    {
                        i = j + 1;
                        break;
                    }
                }
            }
            else
            {
                i = 0;
            }

            if (i == -1)
            {
                throw new Exception("No propriate index was found in a sorted array");
            }

            m_isRunning = !cts.IsCancellationRequested;

            int length = img_pathes.Length;

            int current = 0;
            //Parsing Cycle
            for (; i < length && !cts.IsCancellationRequested; i++)
            {
                m_prog_value = 0;

                this.Parse(img_pathes[i].FullName);

                current = i;

                m_isRunning = !cts.IsCancellationRequested;

                if (cts.IsCancellationRequested)
                {
                    m_prog_value = 0;

                    OnPartOfTheTaskDone?.Invoke(m_prog_value, 0);
                }
            }

            m_temp.ReadFileCreationDate = img_pathes[current].CreationTime;

            m_temp.ReadFileName = img_pathes[current].Name;

            m_temp.MoreThenFirstTime = true;

            m_temp.CurrentImagesCount = current + 1;

            m_dataProvider.SaveFile(m_pathToTemp, m_temp, ViberParserDataProviderOperations.WriteToTemp);
        }

        
        public void Parse(string img)
        {
            if (String.IsNullOrEmpty(img))
                throw new ArgumentNullException("img");

            ExecuteFunctionAndGetResultThroughEvent(
               ViberParserOperations.Parse,
               (state) =>
               {
                   List<string> SuccessfullyRead =
                   new List<string>();

                   string FailToReadPaths = String.Empty;

                   string debugFolder = String.Empty;
                  
                   var txt = String.Empty;

                   StreamWriter sw = null;

                   m_OCRResultParser.ClearSearchFlags();

                   string[] MainResult = new string[4];

                   Image image = null;

                   IEnumerable<CropRectangle> Crops = null;

                   try
                   {
                       Crops = m_OCR.GetCropRectanglesWithText(img, out image);
                   }
                   catch (DivideByZeroException)//Case when finding some crope regions went wrong
                   {
                       FailToReadPaths = img;

                       return new ViberParserResult(SuccessfullyRead, FailToReadPaths,
                       String.IsNullOrEmpty(FailToReadPaths) ? false : true);
                   }

                   int j = 0;

                   if (!String.IsNullOrWhiteSpace(PathToDebuggingFolder))
                   {
                       var paths = img.Split('\\');

                       debugFolder = PathToDebuggingFolder + Path.DirectorySeparatorChar +
                           paths[paths.Length - 1];

                       if (!Directory.Exists(debugFolder))
                       {
                           Directory.CreateDirectory(debugFolder);
                       }

                       txt = debugFolder + Path.DirectorySeparatorChar + "debugout.txt";

                       if (!File.Exists(txt))
                       {
                           var fs = File.Create(txt);

                           fs.Close();

                           fs.Dispose();
                       }

                       sw = new StreamWriter(txt, true, Encoding.UTF8);
                   }

                   if (Crops == null)
                   {
                       FailToReadPaths = img;

                       return new ViberParserResult(SuccessfullyRead, FailToReadPaths,
                       String.IsNullOrEmpty(FailToReadPaths) ? false : true);
                   }

                   //Calculate the amount of incrreasing value for Progress Bars

                   float addValue = 0.2f;

                   float value = (1 - addValue) / Crops.Count();

                   foreach (var crop in Crops)
                   {                       
                       var OcrRes = m_OCR.GetOCRResultAccordingToCropRegion(image, crop,
                           (inp) =>
                           {
                               //Call 1 OCR Input Preprocessor

                               m_OCRInputPreprocessors?.GetInvocationList()?[0]?.DynamicInvoke(inp);

                               //Debuging system

                               inp.StampCropRectangleAndSaveAs(
                                   crop, IronSoftware.Drawing.Color.Red,
                                   debugFolder + Path.DirectorySeparatorChar + $"{j + 1}",
                                   IronSoftware.Drawing.AnyBitmap.ImageFormat.Png
                                  );

                               j++;

                           });

                       bool elnWithBarcode = !(m_OCRResultParser.SurenameFound &&
                       m_OCRResultParser.NameFound && m_OCRResultParser.LastnameFound) && (j > 4);

                       sw.WriteLine(OcrRes.Text);

                       var tempRes = m_OCRResultParser.Parse(OcrRes, elnWithBarcode);
                       //Modify result 1
                       if (!String.IsNullOrEmpty(OcrRes.Text))
                           for (int i = 0; i < MainResult.Length; i++)
                           {
                               if (String.IsNullOrEmpty(MainResult[i]))
                                   MainResult[i] = tempRes[i];
                           }

                       m_prog_value += value;

#if DEBUG
                       Debug.WriteLine($"Progress of current photo parsing: {m_prog_value}");
#endif

                       OnPartOfTheTaskDone?.Invoke(m_prog_value, BitSet.SetBit(0, 0));//Increase the progressbar

                       if (m_OCRResultParser.AllFound())
                           break;

                       if (elnWithBarcode && m_OCRResultParser.SurenameFound &&
                       m_OCRResultParser.NameFound && m_OCRResultParser.LastnameFound)
                           break;
                   }

                   if (String.IsNullOrEmpty(MainResult[MainResult.Length - 1]))//Maybe there is a barcode
                   {
                       string eln = String.Empty;

                       OcrResult try2 = m_OCR.SimpleConvertToText(img, (input) =>
                       {
                           m_OCRInputPreprocessors?.GetInvocationList()?[1]?.DynamicInvoke(input);
                       });
#if DEBUG
                       Debug.WriteLine("Attempt to find BarCode!!!");

                       sw.WriteLine("Crop scaning failure anything hasn't been found!!! Try to use simple parse");

                       foreach (var item in try2.Paragraphs)
                       {
                           foreach (var item2 in item.Words)
                           {
                               Debug.Write(item2.Text);

                               sw.Write(item2.Text);
                           }

                           sw.WriteLine();

                           Debug.WriteLine("\n");
                       }
#endif
                       if (try2.Barcodes.Length > 0)
                       {
                           MainResult[MainResult.Length - 1] = try2.Barcodes[0].Value;                           
                       }
                       else
                       {
                           eln = m_OCRResultParser.FindElnRefinParagraph(try2.Paragraphs);

                           MainResult[MainResult.Length - 1] = eln;
                       }
                   }

                   sw.Close();

                   sw.Dispose();

                   if (image != null)
                       image.Dispose();

                   m_prog_value = 1;//Task finished, with one of the results: failure, completed

                   if (!AllParsedSuccesfully(MainResult))//failure
                   {
                       FailToReadPaths = img;

                       OnPartOfTheTaskDone?.Invoke(m_prog_value, BitSet.SetBit(0, 2));
                   }
                   else//completed
                   {                       
                       Directory.Delete(debugFolder, true);
                       
                       OnPartOfTheTaskDone?.Invoke(m_prog_value, BitSet.SetBit(0, 1));
                   }

                   SuccessfullyRead.AddRange(MainResult);
                   
                   return new ViberParserResult(SuccessfullyRead, FailToReadPaths,
                       String.IsNullOrEmpty(FailToReadPaths) ? false : true);
               }
               );
        }

        private bool AllParsedSuccesfully(string[] parseResult)
        {
            if (parseResult == null)
                return false;

            foreach (var item in parseResult)
                if (String.IsNullOrEmpty(item))
                    return false;

            return true;
        }

        #endregion

    }
}