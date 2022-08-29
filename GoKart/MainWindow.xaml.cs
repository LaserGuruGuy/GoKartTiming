using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CpbTiming.SmsTiming;

namespace GoKart
{
    public partial class MainWindow : Window, IConfiguration
    {
        public CpbTiming CpbTiming { get; set; } = new CpbTiming();

        public Uri Uri { get; set; }

        public string baseUrl { get; set; }

        public string auth { get; set; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string name = string.Empty;

            if (sender.GetType().Equals(typeof(UniqueObservableCollection<LiveTimingEx>)))
            {
                try
                {
                    UniqueObservableCollection<LiveTimingEx> collection = (sender as UniqueObservableCollection<LiveTimingEx>);
                    var element = collection[collection.Count - 1];
                    name += (name == string.Empty ? element.HeatName : "\n" + element.HeatName);
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }
            else if (sender.GetType().Equals(typeof(UniqueObservableCollection<DriverEx>)))
            {
                try
                {
                    UniqueObservableCollection<DriverEx> collection = (sender as UniqueObservableCollection<DriverEx>);
                    var element = collection[collection.Count - 1];
                    name += (name == string.Empty ? element.DriverName : "\n" + element.DriverName);
                }
                catch (Exception ex)
                {
                    name = ex.Message;
                }
            }

            Console.WriteLine(e.Action + " " + name);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                Console.WriteLine("Set " + e.PropertyName + "=" + sender.GetType().GetProperty(e.PropertyName).GetValue(sender)?.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set " + ex.Message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Closed += new EventHandler(MainWindow_Closed);

            //CpbTiming.LiveTimingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            //CpbTiming.LiveTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);

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