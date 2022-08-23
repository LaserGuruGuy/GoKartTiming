﻿using System;
using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace GoKart
{
    public partial class MainWindow
    {
        private void ParsePdfFiles(string[] FileNames)
        {
            foreach (var FileName in FileNames)
            {
                if (File.Exists(FileName))
                {
                    if (Path.GetExtension(FileName).Equals(".pdf"))
                    {
                        PersonalRaceOverviewReportCollection.Parse(
                            Path.GetFileNameWithoutExtension(FileName),
                            File.GetCreationTime(FileName),
                            ExtractTextBookFromPdf(FileName));
                    }
                }
            }
        }

        public static List<string> ExtractTextBookFromPdf(string path)
        {
            List<string> Text = new List<string>();

            PdfReader pdfReader = null;
            try
            {
                pdfReader = new PdfReader(path);

                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
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