using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.ComponentModel;

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

            Loaded += new RoutedEventHandler(MainWindow_Loaded);

            Closed += new EventHandler(MainWindow_Closed);

            //CpbTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            //CpbTiming.LiveTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            CpbTiming.LiveTimingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);

            DataContext = CpbTiming;

            PopulateConfiguration("Config.json");

            WebBrowserHelper.GetBrowserVersion();

            try
            {
                WebBrowser.ObjectForScripting = new WebBrowserScriptInterface(this);
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

        protected void MainWindow_Loaded(object sender, EventArgs args)
        {
            string[] arg = Environment.GetCommandLineArgs();

            if (arg.Length != 0)
            {
                HandleFile(arg);
            }
        }

        private void HandleDroppedFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                HandleFile((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }

        private void HandleFile(string[] FileNames)
        {
            foreach (string FileName in FileNames)
            {
                if (File.Exists(FileName))
                {
                    if (Path.GetExtension(FileName).Equals(".pdf"))
                    {
                        CpbTiming.Add(ExtractTextBookFromPdf(FileName));
                    }
                    else if (Path.GetExtension(FileName).Equals(".json"))
                    {
                        foreach (string Serialized in File.ReadAllLines(FileName))
                        {
                            CpbTiming.Add(Serialized);
                        }
                    }
                }
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