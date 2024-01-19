﻿using ControllerBaseLib;
using IronOcr;
using SmartParser.Dependencies.Interfaces;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text;
using JsonDataProviderLibDNC;
using JsonDataProviderLibDNC.Interfaces;
using SmartParser.Comparators;

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

    internal class ViberParserTemp
    {
        public string ReadFileName { get; set; }

        public DateTime ReadFileCreationDate { get; set; }

        public bool MoreThenFirstTime { get; set; }

        public ViberParserTemp()
        {
                
        }
    }
    
    public class ViberParser : ControllerBaseClass<ViberParserOperations>,
        ISmartParser<string>
    {
        #region Fields

        IDataProvider<ViberParserDataProviderOperations> m_dataProvider;

        private OCR m_OCR;

        private IOCRResultParser<string[]> m_OCRResultParser;

        //private CancellationTokenSource m_cts;

        private ViberParserTemp m_temp;

        private string m_pathToTemp;
 
        #endregion

        #region Properties

        public string PathToDebuggingFolder { get; set; }

        #endregion

        #region Ctor

        public ViberParser(
            IOCRResultParser<string[]> OCRresParser,
            OCR OCR, IDataProvider<ViberParserDataProviderOperations> dataProvider)
        {
            if (OCRresParser == null)
                throw new ArgumentNullException("OCRresParser");

            if (OCR == null)
                throw new ArgumentNullException("OCR");

            if (dataProvider == null)
                throw new ArgumentNullException("dataProvider");
           
            m_OCRResultParser = OCRresParser;

            m_OCR = OCR;

            m_dataProvider = dataProvider;

            (m_dataProvider as ControllerBaseClass<ViberParserDataProviderOperations>).OnOperationFinished += ViberParser_OnOperationFinished;
          
            CreateTempFile();

            m_dataProvider.LoadFile<ViberParserTemp>(m_pathToTemp, m_temp, ViberParserDataProviderOperations.ReadFromTemp);                     
        }

        private void ViberParser_OnOperationFinished(object s, ControllerBaseLib.EventArgs.OperationFinishedEventArgs<ViberParserDataProviderOperations> e)
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

                        break;               
                }

                if (e.Result != null)
                {
                    m_temp = e.Result;
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

            var path_to_temp_dir = Path.Combine(envPath,  @"\Temp");

            m_dataProvider.IfDirectoryNotExistsCreateIt(path_to_temp_dir);

            var path_to_temp_file = Path.Combine(path_to_temp_dir, @"\temp." + m_dataProvider.FileExtension);

            m_dataProvider.IfFileNotExistsCreateIt(path_to_temp_file);

            m_pathToTemp = path_to_temp_file;
        }

        public void ParseImages(FileInfo [] img_pathes)
        {
            if (img_pathes == null)
                throw new NullReferenceException("img_pathes");

            if (img_pathes.Count() == 0)
                return;

            //Decide what to parse

            Array.Sort<FileInfo>(img_pathes, new FileInfo_Compartors.CompareByCreationTime());

            int i = -1;

            if (m_temp.MoreThenFirstTime)//Some files were already written
            {
                i = Array.BinarySearch<FileInfo>(img_pathes, new FileInfo(m_temp.ReadFileName), new FileInfo_Compartors.CompareByName()) + 1;
            }
            else
            {
                i = 0;
            }

            if (i == -1)
            {
                throw new Exception("No propriate index was found in a sorted array");
            }

            int length = img_pathes.Length;

            for (; i < length; i++)
            {
                this.Parse(img_pathes[i].FullName);
            }

            m_temp.ReadFileCreationDate = img_pathes[length-1].CreationTime;

            m_temp.ReadFileName = img_pathes[length - 1].Name;

            m_temp.MoreThenFirstTime = true;

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

                   var Crops = m_OCR.GetCropRectanglesWithText(img, out image);

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

                   foreach (var crop in Crops)
                   {                       
                       var OcrRes = m_OCR.GetOCRResultAccordingToCropRegion(image, crop,
                           (inp) =>
                           {
                               //inp.ToGrayScale().Dilate();

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

                       if (m_OCRResultParser.AllFound())
                           break;

                       if (elnWithBarcode && m_OCRResultParser.SurenameFound &&
                       m_OCRResultParser.NameFound && m_OCRResultParser.LastnameFound)
                           break;
                   }

                   if (String.IsNullOrEmpty(MainResult[MainResult.Length - 1]))//Maybe there is a barcode
                   {
                       string eln = String.Empty;

                       OcrResult try2 = m_OCR.SimpleConvertToText(img, null);

                       Debug.WriteLine("Attempt to find BarCode!!!");
                      
                       sw.WriteLine("Crop scaning failure somthing hasn't been found!!! Try to use simple parse");
                       
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
                       
                       if (try2.Barcodes.Length > 0)
                       {
                           MainResult[MainResult.Length - 1] = try2.Barcodes[0].Value;
                       }
                       else
                       {
                           foreach (var paragraph in try2.Paragraphs)
                           {
                               m_OCRResultParser.FindElnRefRecursively(paragraph, ref eln);
                           }
                       }
                   }

                   sw.Close();

                   sw.Dispose();

                   if (image != null)
                       image.Dispose();
                   
                   if (!AllParsedSuccesfully(MainResult))
                   {
                       FailToReadPaths = img;
                   }
                   else
                   {
                       Directory.Delete(debugFolder, true);
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