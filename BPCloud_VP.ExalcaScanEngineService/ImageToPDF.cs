

using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BPCloud_VP.ExalcaScanEngineService
{
    public class ImageToPDF
    {

        public static bool ConvertImageToPDF(string inputFile, int width = 600)
        {
            try
            {
                string path = Path.ChangeExtension(inputFile, ".pdf");
                Image image1 = Image.FromFile(inputFile);
                PdfDocument pdfDocument = new PdfDocument();
                for (int index = 0; index < image1.GetFrameCount(FrameDimension.Page); ++index)
                {
                    image1.SelectActiveFrame(FrameDimension.Page, index);
                    XImage image2 = XImage.FromGdiPlusImage(image1);
                    PdfPage page = new PdfPage();
                    int height = (int)((double)width / (double)image2.PixelWidth * (double)image2.PixelHeight);
                    page.Width = (XUnit)width;
                    page.Height = (XUnit)height;
                    pdfDocument.Pages.Add(page);
                    XGraphics.FromPdfPage(pdfDocument.Pages[index]).DrawImage(image2, 0, 0, width, height);
                }
                pdfDocument.Save(path);
                pdfDocument.Close();
                image1.Dispose();
                File.Delete(inputFile);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("ConvertImageToPDF " + ex.Message);
                return false;
            }
        }
    }
}
