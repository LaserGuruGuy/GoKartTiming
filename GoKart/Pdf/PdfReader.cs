using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using GoKart.Pdf;

namespace GoKart
{
    public partial class CpbTiming
    {
        public static List<string> ExtractTextBookFromPdf(string path)
        {
            List<string> Text = new List<string>();

            PdfReader pdfReader = null;
            try
            {
                pdfReader = new PdfReader(path);

                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    if (pdfDocument.GetNumberOfPages() > 1)
                    {
                        for (int Page = 1; Page <= pdfDocument.GetNumberOfPages(); Page++)
                        {
                            string Location = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(Page), new LocationTextExtractionStrategy());
                            string Header = Location.Substring(0, Location.IndexOf("Ronden Heat overzicht Beste tijd\n"));

                            string Simple = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(Page), new SimpleTextExtractionStrategy());
                            string Ronden = Simple.Substring(Simple.IndexOf("Ronden"));

                            Text.Add(Header + Ronden);
                        }
                    }
                    else if (pdfDocument.GetNumberOfPages() == 1)
                    {
                        string Location = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(1), new LocationTextExtractionStrategy(new LaxTextChunkLocationStrategy()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                pdfReader.Close();
            }

            return Text;
        }
    }
}