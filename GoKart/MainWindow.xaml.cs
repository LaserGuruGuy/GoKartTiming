using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Interop;
using System.Threading;

namespace GoKart
{
    public partial class MainWindow : Window, IConfiguration
    {
        Thread WorkerThread;

        public CpbTiming CpbTiming { get; set; } = new CpbTiming();

        public Uri Uri { get; set; }

        public string baseUrl { get; set; }

        public string auth { get; set; }

        private readonly object UpdateLiveTimingLock = new object();

        private bool _UpdateLiveTiming = false;

        public bool UpdateLiveTiming
        {
            get
            {
                bool updateLiveTiming = false;
                lock (UpdateLiveTimingLock)
                {
                    updateLiveTiming = _UpdateLiveTiming;
                }
                return updateLiveTiming;
            }
            set
            {
                lock (UpdateLiveTimingLock)
                {
                    _UpdateLiveTiming = value;
                }
            }
        }

        private readonly object UpdateDriverLock = new object();

        private bool _UpdateDriver = false;

        public bool UpdateDriver
        {
            get
            {
                bool updateDriver = false;
                lock (UpdateDriverLock)
                {
                    updateDriver = _UpdateDriver;
                }
                return updateDriver;
            }
            set
            {
                lock (UpdateDriverLock)
                {
                    _UpdateDriver = value;
                }
            }
        }

        private readonly object UpdateLapTimeLock = new object();

        private bool _UpdateLapTime = false;

        public bool UpdateLapTime
        {
            get
            {
                bool updateLapTime = false;
                lock (UpdateLapTimeLock)
                {
                    updateLapTime = _UpdateLapTime;
                }
                return updateLapTime;
            }
            set
            {
                lock (UpdateLapTimeLock)
                {
                    _UpdateLapTime = value;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);

            Closed += new EventHandler(MainWindow_Closed);

            CpbTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            CpbTiming.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
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

            ComponentDispatcher.ThreadIdle += new System.EventHandler(ComponentDispatcher_ThreadIdle);
        }

        void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            if (UpdateLiveTiming)
            {
                UpdateLiveTiming = false;
                ListView_LiveTimingCollection.Items.Refresh();
            }

            if (UpdateDriver)
            {
                UpdateDriver = false;
                ListViewSort(ListView_LiveTiming, "Position");
            }

            if (UpdateLapTime)
            {
                UpdateLapTime = false;

                ListView_LapTime.Items.Refresh();

                AbsoluteLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                CumulativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                RelativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }
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
                        ///
                        WorkerThread = new Thread(CpbTiming.AddTextBook);
                        WorkerThread.Start(FileName);
                        ///
                        //CpbTiming.Add(ExtractTextBookFromPdf(FileName));
                    }
                    else if (Path.GetExtension(FileName).Equals(".json"))
                    {
                        //foreach (string Serialized in File.ReadAllLines(FileName))
                        //{
                        ///
                        WorkerThread = new Thread(CpbTiming.AddJson);
                        WorkerThread.Start(FileName);
                        ///
                        //CpbTiming.Add(Serialized);
                        //}
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