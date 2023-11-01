using ControllerBaseLib;
using IronOcr;
using SmartParser.Dependencies.Interfaces;

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

        #endregion

        #region Ctor

        public ViberParserResult(IEnumerable<string> successfullyRead,
            string failedToRead)
        {
            SuccessfullyRead = successfullyRead;

            FailedToReadPaths = failedToRead;
        }

        #endregion
    }

    public class ViberParser : ControllerBaseClass<ViberParserOperations>,
        ISmartParser<string, string[]>
    {
        #region Fields

        private int m_trys;

        private Dictionary<string, OCR> m_OCRs;

        private Dictionary<string, IOCRResultParser<string[]>> m_OCRResultParsers;

        private CancellationTokenSource m_cts;

        #endregion

        #region Indexer

        public OCR this[string key]
        {
            get
            {
                if (String.IsNullOrEmpty(key))
                    throw new ArgumentNullException("key");

                if (!m_OCRs.ContainsKey(key))
                    throw new Exception("There is no such key in OCRs Dictionary!!!");

                return m_OCRs[key];
            }
        }

        #endregion

        #region Ctor

        public ViberParser(IEnumerable<KeyValuePair<string,
            IOCRResultParser<string[]>>> OCRresParsers,
            IEnumerable<KeyValuePair<string, OCR>> OCRs,
            
            int trys)
        {            
            if (OCRresParsers == null)
                throw new ArgumentNullException("OCRresParser");

            if (OCRs == null)
                throw new ArgumentNullException("OCRs collection");

            if (OCRs.Count() == 0)
                throw new Exception("There are no OCRs in OCRS collection!!!");

            if (OCRresParsers.Count() == 0)
                throw new Exception("There are no OCRresParsers in OCRresParsers collection!!!");

            m_OCRs = new Dictionary<string, OCR>();

            m_OCRResultParsers = new Dictionary<string, IOCRResultParser<string[]>>();

            foreach (var item in OCRs)
            {
                m_OCRs.Add(item.Key, item.Value);
            }

            foreach (var item in OCRresParsers)
            {
                m_OCRResultParsers.Add(item.Key, item.Value);
            }

            //m_cts = cts;

            m_trys = trys;
        }

        #endregion

        #region Methods

        public void AddOCR(string key, OCR ocr)
        {
            if (ocr == null)
                throw new ArgumentNullException("ocr");

            if (m_OCRs.ContainsKey(key))
                throw new Exception("OCRs Dicionary has already had this key!");

            m_OCRs.Add(key, ocr);
        }

        public IEnumerable<string> GetAllOCRsKeys()
        {
            return m_OCRs.Keys;
        }

        public bool ContainsBarCode(OcrResult res)
        {
            return res.Barcodes.Length > 0;
        }

        public void Parse(string img, string[] ParsersToUse, string[] OCRsTouse)
        {
            if (String.IsNullOrEmpty(img) || OCRsTouse == null || ParsersToUse == null)
                throw new ArgumentNullException("images or OCRsTouse or ParsersToUse");


             ExecuteFunctionAndGetResultThroughEvent(
                ViberParserOperations.Parse,
                (state) =>
                {
                    List<string> SuccessfullyRead =
                    new List<string>();

                   string FailToReadPaths = String.Empty;

                    int OCRTouseIndex = 0;

                    int ParsersToUseIndex = 0;

                    OCRTouseIndex = 0;

                    string[] r = null;

                    int curtry = 0;


                    var ocrResultsSimple = m_OCRs[OCRsTouse[OCRTouseIndex]].SimpleConvert(img);

                    var ocrResults = m_OCRs[OCRsTouse[OCRTouseIndex]].ConvertPhotoToText(img);

                        

                    //Failed to read any text from image 
                    if (ocrResults.Count == 0)
                        {
                            OCRTouseIndex++;                           
                        }



                        r = m_OCRResultParsers[ParsersToUse[ParsersToUseIndex]]
                        .Parse(ocrResults);
                        //Failed To parse OCR Result
                        if (r.Length == 0)
                        {
                            ParsersToUseIndex++;
                            
                        }

                        m_trys++;

                    

                    if (!AllParsedSuccesfully(r))
                    {
                        FailToReadPaths = img;
                    }
                    else
                    {
                        foreach (var item in r)
                        {
                            SuccessfullyRead.Add(item);
                        }
                    }

                    return new ViberParserResult(SuccessfullyRead, FailToReadPaths);
                }
                );
        }

        private bool AllParsedSuccesfully(string[] parseResult)
        {
            foreach (var item in parseResult)
                if (String.IsNullOrEmpty(item))
                    return false;

            return true;
        }

        public void RemoveOCR(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (!m_OCRs.ContainsKey(key))
                throw new Exception("There is no such key in the OCRs Dictionary!");

            m_OCRs.Remove(key);
        }

        #endregion

    }
}