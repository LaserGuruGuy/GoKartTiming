using System;
using System.Windows;
using System.Windows.Controls;

namespace GoKart
{
    public partial class MainWindow : Window, IConfiguration
    {
        public CpbTiming CpbTiming { get; set; } = new CpbTiming();

        public Uri Uri { get; set; }

        public string baseUrl { get; set; }

        public string auth { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Closed += new EventHandler(MainWindow_Closed);

            DataContext = CpbTiming;

            PopulateConfiguration("Config.json");

            WebBrowserHelper.GetBrowserVersion();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length != 0)
            {
                ParsePdfFiles(args);
            }

            try
            {
                WebBrowser.ObjectForScripting = new ScriptInterface(this);
                ((ScriptInterface)WebBrowser.ObjectForScripting).PopulateFromFile(@"C:\Users\xboxl\source\repos\GoKartTiming\racelogfile.json");
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
                WebBrowser.Navigate(Uri);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
            }
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
            AbsoluteLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            CumulativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            RelativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
        }
    }
}