using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Threading;
using System.ComponentModel;
using System.Collections.Specialized;
using GoKart.WebBrowser;
using System.Collections.Generic;

namespace GoKart
{
    public partial class MainWindow : Window
    {
        Thread WorkerThread;

        private CpbTiming CpbTiming { get; set; } = new CpbTiming();

        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public static Dictionary<string, string> KartCenterDict { get; } = new Dictionary<string, string>
        {
            {"Hezemans Indoor Karting", "aGV6ZW1hbnM6aW5kb29ya2FydGluZw==" },
            {"Circuit Park Berghem", "Y2lyY3VpdHBhcmtiZXJnaGVtOjNmZGIwZDY5LWQxYmItNDZmMS1hYTAyLWNkZDkzODljMmY1MQ==" }
        };

        public string KartCenterKey { get; set; } = "Y2lyY3VpdHBhcmtiZXJnaGVtOjNmZGIwZDY5LWQxYmItNDZmMS1hYTAyLWNkZDkzODljMmY1MQ==";

        public MainWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);

            Closed += new EventHandler(MainWindow_Closed);

            CpbTiming.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            CpbTiming.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            CpbTiming.LiveTimingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);

            DataContext = CpbTiming;

            try
            {
                WebBrowser.ObjectForScripting = new WebBrowserScriptInterface(OnJSONReceived);
            }
            catch (ArgumentException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                (WebBrowser.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
            }

            try
            {
                WebBrowser.Navigate((WebBrowser.ObjectForScripting as WebBrowserScriptInterface).Uri);
            }
            catch (ObjectDisposedException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            catch (InvalidOperationException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            catch (System.Security.SecurityException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
            }

            ComponentDispatcher.ThreadIdle += new EventHandler(ComponentDispatcher_ThreadIdle);
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
            (WebBrowser.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
            WebBrowser.Navigate((WebBrowser.ObjectForScripting as WebBrowserScriptInterface).Uri);
        }
    }
}