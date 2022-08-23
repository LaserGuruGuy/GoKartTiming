using System;
using System.Windows;
using System.Windows.Controls;
using CpbTiming;

namespace GoKart
{
    public partial class MainWindow : Window
    {
        private PersonalRaceOverviewReportCollection PersonalRaceOverviewReportCollection = new PersonalRaceOverviewReportCollection();

        private Uri uri = new Uri("pack://siteoforigin:,,,/SmsTiming/LiveTiming.htm", UriKind.RelativeOrAbsolute);

        public MainWindow()
        {
            InitializeComponent();

            WebBrowserHelper.GetBrowserVersion();

            AbsoluteLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Absolute);
            AbsoluteLapTimeWindow.Closed += AbsoluteRondeTijdenWindow_Closed;

            CumulativeLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Cumulative);
            CumulativeLapTimeWindow.Closed += CummulatieveRondeTijdenWindow_Closed;

            RelativeLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Relative);
            RelativeLapTimeWindow.Closed += RelatieveRondeTijdenWindow_Closed;

            Closed += new EventHandler(MainWindow_Closed);

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length != 0)
            {
                ParsePdfFiles(args);
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
            AbsoluteLapTimeWindow?.Close();
            CumulativeLapTimeWindow?.Close();
            RelativeLapTimeWindow?.Close();
        }

        private void HandleDroppedFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ParsePdfFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }

        private void ListView_RaceOverviewReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AbsoluteLapTimeWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            CumulativeLapTimeWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
            RelativeLapTimeWindow?.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
        }
    }
}