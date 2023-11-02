using ControllerBaseLib;
using IronOcr;
using SmartParser.Dependencies.Interfaces;
using System.Drawing.Imaging;

namespace SmartParser.Parsers
{
    public enum ViberParserOperations
    {
        Parse = 0
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

    public class ViberParser : ControllerBaseClass<ViberParserOperations>,
        ISmartParser<string, string[]>
    {
        #region Fields

        private OCR m_OCR;

        private IOCRResultParser<string[]> m_OCRResultParser;

        private CancellationTokenSource m_cts;

        #endregion

        #region Properties

        public string PathToDebuggingFolder { get; set; }

        #endregion

        #region Ctor

        public ViberParser(
            IOCRResultParser<string[]> OCRresParser,
            OCR OCR)
        {
            if (OCRresParser == null)
                throw new ArgumentNullException("OCRresParser");

            if (OCR == null)
                throw new ArgumentNullException("OCR");

            m_OCRResultParser = OCRresParser;

            m_OCR = OCR;

            //m_cts = cts;            
        }

        #endregion

        #region Methods

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

                   m_OCRResultParser.ClearSearchFlags();                   

                   var r = m_OCR.SimpleConvertToText(img,
                       (input) =>
                       {
                           //input.DeNoise().Sharpen();

                       });

                   string[] MainResult = new string[4];

                   if (r!= null)
                     MainResult = m_OCRResultParser.Parse(r);

                   if (!AllParsedSuccesfully(MainResult))//Failed parse some elements
                   {
                       Image image = null;

                       var Crops = m_OCR.GetCropRectanglesWithText(img, out image);

                       int j = 0;

                       foreach (var crop in Crops)
                       {
                           var OcrRes = m_OCR.GetOCRResultAccordingToCropRegion(image, crop,
                               (inp) =>
                               {
                                   //inp.DeNoise().Sharpen();

                                   if (!String.IsNullOrWhiteSpace(PathToDebuggingFolder))
                                   {
                                       var paths = img.Split('\\');

                                       string debugFolder = PathToDebuggingFolder + Path.DirectorySeparatorChar +
                                           paths[paths.Length - 1]; 

                                       if (!Directory.Exists(debugFolder))
                                       {                                                                                     
                                           Directory.CreateDirectory(debugFolder);
                                       }

                                       inp.StampCropRectangleAndSaveAs(
                                           crop, IronSoftware.Drawing.Color.Red,
                                           debugFolder + Path.DirectorySeparatorChar+ $"{j + 1}",
                                           IronSoftware.Drawing.AnyBitmap.ImageFormat.Png
                                          );

                                       j++;
                                   }

                               });

                           var tempRes = m_OCRResultParser.Parse(OcrRes);
                           //Modify result 1
                           if (!String.IsNullOrEmpty(OcrRes.Text))
                               for (int i = 0; i < MainResult.Length; i++)
                               {
                                   if (String.IsNullOrEmpty(MainResult[i]))
                                       MainResult[i] = tempRes[i];
                               }

                           if (m_OCRResultParser.AllFound())
                               break;
                       }

                       if (image != null)
                           image.Dispose();
                   }
                   //Try to use segmentation reading
                   if (!AllParsedSuccesfully(MainResult))
                   {

                   }

                   if (!AllParsedSuccesfully(MainResult))
                   {
                       FailToReadPaths = img;
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