using IronOcr;
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



        public async Task<List<OcrResult>> ConvertPhotoToTextAsync(string ImgPath)
        {
            Image img = Image.Load(ImgPath);

            var regions = m_openCvClient.FindTextRegions(img, 1, 1, false, false);

            List<OcrResult> result = new List<OcrResult>();

            foreach (var region in regions)
            {
                var r = await m_tess.ReadAsync(img, region);

                result.Add(r);

                Debug.WriteLine(r.Text);
            }

            return result;
        }

        public List<OcrResult> ConvertPhotoToText(string ImgPath)
        {
            Image img = Image.Load(ImgPath);

            var regions = m_openCvClient.FindTextRegions(img, 1, 1, false, false);
            
            List<OcrResult> result = new List<OcrResult>();

            foreach (var region in regions)
            {
                var r = m_tess.Read(img, region);
               
                result.Add(r);

                Debug.WriteLine(r.Text);
            }

            return result;
        }
        #endregion
    }
}
