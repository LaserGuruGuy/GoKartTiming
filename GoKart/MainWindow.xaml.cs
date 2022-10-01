using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Threading;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using GoKartTiming.BestTiming;
using System.Globalization;
using GoKart.SmsTiming;

namespace GoKart
{
    public partial class MainWindow : Window
    {
        Thread WorkerThread;
        private GoKartTiming CpbTiming { get; set; } = new GoKartTiming();

        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public static Dictionary<string, string> KartCenterDict { get; } = new Dictionary<string, string>
        {
            {"Hezemans Indoor Karting", "aGV6ZW1hbnM6aW5kb29ya2FydGluZw==" },
            {"Circuit Park Berghem", "Y2lyY3VpdHBhcmtiZXJnaGVtOjNmZGIwZDY5LWQxYmItNDZmMS1hYTAyLWNkZDkzODljMmY1MQ==" }
        };

        public static Dictionary<string, Uri> KartCenterIconDict { get; } = new Dictionary<string, Uri>
        {
            {"Hezemans Indoor Karting", new Uri("./Icons/hezemans-logo.ico", UriKind.Relative) },
            {"Circuit Park Berghem", new Uri("./Icons/cpb-logo.ico", UriKind.Relative) }
        };

        public string KartCenterKey { get; set; } = "aGV6ZW1hbnM6aW5kb29ya2FydGluZw==";

        ConnectionServiceBestTimes ConnectionServiceBestTimes;
        ConnectionServiceLiveTiming ConnectionServiceLiveTiming;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);

            Closed += new EventHandler(MainWindow_Closed);

            CpbTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            CpbTiming.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            CpbTiming.LiveTimingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            CpbTiming.BestTimingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);

            DataContext = CpbTiming;

            ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);

            ConnectionServiceBestTimes = new ConnectionServiceBestTimes(OnBestTiming);
            ConnectionServiceLiveTiming = new ConnectionServiceLiveTiming(OnLiveTiming);
        }

        protected void MainWindow_Closed(object sender, EventArgs args)
        {
            WorkerThread?.Abort();
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
                        WorkerThread = new Thread(CpbTiming.AddTextBook);
                        WorkerThread.SetApartmentState(ApartmentState.STA);
                        WorkerThread.IsBackground = true;
                        WorkerThread.Name = "CpbTiming.AddTextBook";
                        WorkerThread.Start(FileName);
                    }
                    else if (Path.GetExtension(FileName).Equals(".json"))
                    {
                        WorkerThread = new Thread(CpbTiming.AddJson);
                        WorkerThread.SetApartmentState(ApartmentState.STA);
                        WorkerThread.IsBackground = true;
                        WorkerThread.Name = "CpbTiming.AddJson";
                        WorkerThread.Start(FileName);
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

        private void ComboBox_LiveTimingKartCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string KartCenter = ((KeyValuePair<string, string>)(comboBox.SelectedItem)).Key;

            CpbTiming.BestTimingCollection.Reset();
            ConnectionServiceBestTimes.Init(KartCenterKey);
            ConnectionServiceLiveTiming.Init(KartCenterKey);

            Uri IconUri;
            KartCenterIconDict.TryGetValue(KartCenter, out IconUri);
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(IconUri);
        }

        private void ComboBox_BestTimingDateTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var now = DateTime.Now;
            GetRecordGroup(
                CpbTiming.BestTimingCollection.resourceId,
                (ComboBox_BestTimes_ScoreGroup.SelectedItem as ScoreGroup)?.scoreGroupId,
                ((KeyValuePair<string, DateTime>)ComboBox_BestTimingDateTime.SelectedItem).Value.ToString(CultureInfo.InvariantCulture),
                "",
                "20");
        }

        private void ComboBox_BestTimes_ScoreGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var now = DateTime.Now;
            GetRecordGroup(
                CpbTiming.BestTimingCollection.resourceId,
                (ComboBox_BestTimes_ScoreGroup.SelectedItem as ScoreGroup)?.scoreGroupId,
                ((KeyValuePair<string, DateTime>)ComboBox_BestTimingDateTime.SelectedItem).Value.ToString(CultureInfo.InvariantCulture),
                "",
                "20");
        }

        private void GetRecordGroup(string rscId, string scgId, string startDate, string endDate, string maxResults)
        {
            ConnectionServiceBestTimes.Update(rscId, scgId, startDate, endDate, maxResults);
        }
    }
}