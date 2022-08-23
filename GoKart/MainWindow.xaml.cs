using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using CpbTiming;

namespace GoKart
{
    public partial class MainWindow : Window
    {
        private PersonalRaceOverviewReportCollection PersonalRaceOverviewReportCollection = new PersonalRaceOverviewReportCollection();

        private GridViewColumnHeader LastHeaderClicked = null;
        private ListSortDirection LastDirection;

        private RondeTijdenWindow AbsoluteRondeTijdenWindow;
        private RondeTijdenWindow CummulatieveRondeTijdenWindow;
        private RondeTijdenWindow RelatieveRondeTijdenWindow;

        private Uri uri = new Uri("pack://siteoforigin:,,,/SmsTiming/LiveTiming.htm", UriKind.RelativeOrAbsolute);

        //private void CheckBroswerVersion()
        //{
        //    var appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";

        //    RegistryKey Regkey = null;
        //    try
        //    {
        //        int BrowserVer, RegVal;

        //        // get the installed IE version
        //        using (WebBrowser Wb = new WebBrowser())
        //            BrowserVer = Wb.Version.Major;

        //        // set the appropriate IE version
        //        if (BrowserVer >= 11)
        //            RegVal = 11001;
        //        else if (BrowserVer == 10)
        //            RegVal = 10001;
        //        else if (BrowserVer == 9)
        //            RegVal = 9999;
        //        else if (BrowserVer == 8)
        //            RegVal = 8888;
        //        else
        //            RegVal = 7000;

        //        //For 64 bit Machine 
        //        if (Environment.Is64BitOperatingSystem)
        //            Regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
        //        else  //For 32 bit Machine 
        //            Regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);

        //        //If the path is not correct or 
        //        //If user't have priviledges to access registry 
        //        if (Regkey == null)
        //        {
        //            MessageBox.Show("Registry Key for setting IE WebBrowser Rendering Address Not found. Try run the program with administrator's right.");
        //            return;
        //        }

        //        string FindAppkey = Convert.ToString(Regkey.GetValue(appName));

        //        //Check if key is already present 
        //        if (FindAppkey == RegVal.ToString())
        //        {
        //            Regkey.Close();
        //            return;
        //        }

        //        Regkey.SetValue(appName, RegVal, RegistryValueKind.DWord);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Registry Key for setting IE WebBrowser Rendering failed to setup");
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        //Close the Registry 
        //        if (Regkey != null)
        //            Regkey.Close();
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();

            WebBrowserHelper.GetBrowserVersion();

            AbsoluteRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Absolute);
            AbsoluteRondeTijdenWindow.Closed += AbsoluteRondeTijdenWindow_Closed;

            CummulatieveRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Cumulative);
            CummulatieveRondeTijdenWindow.Closed += CummulatieveRondeTijdenWindow_Closed;

            RelatieveRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Relative);
            RelatieveRondeTijdenWindow.Closed += RelatieveRondeTijdenWindow_Closed;

            Closed += new EventHandler(MainWindow_Closed);

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length != 0)
            {
                ParseFiles(args);
            }

            try
            {
                WebBrowser.ObjectForScripting = new ScriptInterface(this);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
            }

            try
            {
                WebBrowser.Navigate(uri);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
            }

            DataContext = PersonalRaceOverviewReportCollection;
        }

        protected void MainWindow_Closed(object sender, EventArgs args)
        {
            AbsoluteRondeTijdenWindow?.Close();
            CummulatieveRondeTijdenWindow?.Close();
            RelatieveRondeTijdenWindow?.Close();
        }

        private void HandleDroppedFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ParseFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }

        private void ParseFiles(string[] FileNames)
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
                        var Strategy = new SimpleTextExtractionStrategy();

                        Text.Add(PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(Page), Strategy));
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

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    ListSortDirection direction;

                    if (headerClicked != LastHeaderClicked)
                    {
                        LastDirection = ListSortDirection.Descending;
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (LastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sender, sortBy, direction);

                    LastHeaderClicked = headerClicked;
                    LastDirection = direction;
                }
            }
        }

        private void Sort(object sender, string sortBy, ListSortDirection direction)
        {
            var listview = sender as ListView;
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listview.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void ListView_RaceOverviewReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AbsoluteRondeTijdenWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            CummulatieveRondeTijdenWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            RelatieveRondeTijdenWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
        }

        private void MenuItem_View_AbsoluteTime_Click(object sender, RoutedEventArgs e)
        {
            if (AbsoluteRondeTijdenWindow == null)
            {
                AbsoluteRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Absolute);
                AbsoluteRondeTijdenWindow.Show();
                AbsoluteRondeTijdenWindow.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            }
            else
            {
                if (AbsoluteRondeTijdenWindow.IsVisible)
                {
                    AbsoluteRondeTijdenWindow.Hide();
                }
                else
                {
                    AbsoluteRondeTijdenWindow.Show();
                }
            }
        }

        private void MenuItem_View_IntegralTime_Click(object sender, RoutedEventArgs e)
        {
            if (CummulatieveRondeTijdenWindow == null)
            {
                CummulatieveRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Cumulative);
                CummulatieveRondeTijdenWindow.Show();
                CummulatieveRondeTijdenWindow.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            }
            else
            {
                if (CummulatieveRondeTijdenWindow.IsVisible)
                {
                    CummulatieveRondeTijdenWindow.Hide();
                }
                else
                {
                    CummulatieveRondeTijdenWindow.Show();
                }
            }
        }

        private void MenuItem_View_RelativeTime_Click(object sender, RoutedEventArgs e)
        {
            if (RelatieveRondeTijdenWindow == null)
            {
                RelatieveRondeTijdenWindow = new RondeTijdenWindow(RondeTijdenWindow.RondeTijdenType.Relative);
                RelatieveRondeTijdenWindow.Show();
                RelatieveRondeTijdenWindow.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            }
            else
            {
                if (RelatieveRondeTijdenWindow.IsVisible)
                {
                    RelatieveRondeTijdenWindow.Hide();
                }
                else
                {
                    RelatieveRondeTijdenWindow.Show();
                }
            }
        }

        private void AbsoluteRondeTijdenWindow_Closed(object sender, EventArgs e)
        {
            AbsoluteRondeTijdenWindow = null;
        }

        private void RelatieveRondeTijdenWindow_Closed(object sender, EventArgs e)
        {
            RelatieveRondeTijdenWindow = null;
        }

        private void CummulatieveRondeTijdenWindow_Closed(object sender, EventArgs e)
        {
            CummulatieveRondeTijdenWindow = null;
        }
    }
}