using IronOcr;
using IronSoftware.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParser
{
    public class OCR
    {
        #region Fields
        IronTesseract m_tess;
        
        OpenCvClient m_openCvClient;
        #endregion

        #region Properties

        #endregion

        #region Ctor
        public OCR(TesseractConfiguration config)
        {            
            m_tess = new IronTesseract();

            m_tess.Language = OcrLanguage.UkrainianBest;
            
            m_tess.Configuration = config;

            m_openCvClient = OpenCvClient.Instance;
        }
        #endregion

        #region Methods

        public OcrResult GetOCRResultAccordingToCropRegion(Image img, CropRectangle cropRect,
            Action<OcrInput> modImage)
        {
            OcrInput inp = new OcrInput();

            inp.Add(img, cropRect);

            modImage?.Invoke(inp);

            return m_tess.Read(inp);
        }

        public async Task<List<OcrResult>> ConvertPhotoToTextAsync(string ImgPath)
        {            
            Image img = Image.Load(ImgPath);

            var regions = m_openCvClient.FindTextRegions(img, 1, 1, false, false);

            List<OcrResult> result = new List<OcrResult>();

            foreach (var region in regions)
            {
                OcrImageInput inp = new OcrImageInput(ImgPath, null, region);

                inp.Sharpen().DeNoise();

                var r = await m_tess.ReadAsync(inp);

                result.Add(r);

                Debug.WriteLine(r.Text);

                inp.Dispose();
            }

            return result;
        }
        
        public List<OcrResult> GetMultipleOCRResults(string ImgPath)
        {
            Image img = Image.Load(ImgPath);

            var regions = m_openCvClient.FindTextRegions(img, 1, 1, false, false);
            
            List<OcrResult> result = new List<OcrResult>();

            foreach (var region in regions)
            {
                OcrInput inp = new OcrInput();

                inp.Add(ImgPath, region);

                inp.Sharpen().DeNoise();

                var r = m_tess.Read(inp);
               
                result.Add(r);

                Debug.WriteLine(r.Text);
            }

            return result;
        }

        public OcrResult SimpleConvertToText(string ImgPath, Action<OcrInput> ModInput)
        {
            OcrInput inp = new OcrInput(ImgPath);

            ModInput?.Invoke(inp);

            var r = m_tess.Read(inp);

            inp.Dispose();

            return r;
        }

        public IEnumerable<CropRectangle> GetCropRectanglesWithText(string pathToImg, out Image img,
            double scale=1.0,
            int dill_amount=1, bool binarize=false, bool invert=false)
        {
            img = Image.Load(pathToImg);

            return m_openCvClient.FindTextRegions(img, scale, dill_amount, binarize, invert);
        }

        #endregion
    }
}
