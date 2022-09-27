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
using GoKartTiming.BestTiming;
using System.Globalization;

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

        public static Dictionary<string, DateTime> DateTimeDict { get; private set; } = new Dictionary<string, DateTime>();

        public string KartCenterKey { get; set; } = "aGV6ZW1hbnM6aW5kb29ya2FydGluZw==";

        public string DateTimeKey { get; set; }

        private void StampDateTimeDict()
        {
            var now = DateTime.Now;

            int diff = (int)now.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var week = now.AddDays(-(diff < 0 ? 7 + diff : diff));

            DateTimeDict.Add("Forever", new DateTime(now.Year, 1, 1).AddYears(-20));
            DateTimeDict.Add("This Year", new DateTime(now.Year, 1, 1));
            DateTimeDict.Add("This Month", new DateTime(now.Year, now.Month, 1));
            DateTimeDict.Add("This Week", new DateTime(week.Year, week.Month, week.Day));
            DateTimeDict.Add("Today", new DateTime(now.Year, now.Month, now.Day));
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

            try
            {
                WebBrowserLiveTiming.ObjectForScripting = new WebBrowserScriptInterface(OnLiveTiming);
            }
            catch (ArgumentException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                (WebBrowserLiveTiming.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
                (WebBrowserLiveTiming.ObjectForScripting as WebBrowserScriptInterface).Uri = new Uri("pack://siteoforigin:,,,/SmsTiming/LiveTiming.htm");
            }

            try
            {
                WebBrowserLiveTiming.Navigate((WebBrowserLiveTiming.ObjectForScripting as WebBrowserScriptInterface).Uri);
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

            try
            {
                WebBrowserBestTiming.ObjectForScripting = new WebBrowserScriptInterface(OnBestTiming);
            }
            catch (ArgumentException Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                (WebBrowserBestTiming.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
                (WebBrowserBestTiming.ObjectForScripting as WebBrowserScriptInterface).Uri = new Uri("pack://siteoforigin:,,,/SmsTiming/BestTimes.htm");
            }

            try
            {
                WebBrowserBestTiming.Navigate((WebBrowserBestTiming.ObjectForScripting as WebBrowserScriptInterface).Uri);
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

            StampDateTimeDict();

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
            (WebBrowserLiveTiming.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
            WebBrowserLiveTiming.Navigate((WebBrowserLiveTiming.ObjectForScripting as WebBrowserScriptInterface).Uri);

            CpbTiming.BestTimingCollection.Reset();
            (WebBrowserBestTiming.ObjectForScripting as WebBrowserScriptInterface).auth = KartCenterKey;
            WebBrowserBestTiming.Navigate((WebBrowserBestTiming.ObjectForScripting as WebBrowserScriptInterface).Uri);
        }

        private void ListView_BestTimingCollection_scoregroupcollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView_BestTimingCollection_parametergroupcollection.Items.Refresh();
            ListView_BestTimingCollection_recordgroupcollection.Items.Refresh();
        }

        private void ListView_BestTimingCollection_parametergroupcollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GetRecordGroup((ListView_BestTimingCollection_parametergroupcollection.SelectedItem as ParameterGroup).rscId,
                (ListView_BestTimingCollection_parametergroupcollection.SelectedItem as ParameterGroup).scgId,
                (ListView_BestTimingCollection_parametergroupcollection.SelectedItem as ParameterGroup).startDate);
        }

        private void GetRecordGroup(string rscId, string scgId, string startDate)
        {
            if (WebBrowserBestTiming.Document != null)
            {
                Object[] objArray = new Object[3];
                objArray[0] = (Object)rscId;
                objArray[1] = (Object)scgId;
                objArray[2] = (Object)startDate;
                WebBrowserBestTiming.InvokeScript("getBestTimesGroup", objArray);
            }
        }

        private void ComboBox_BestTimingDateTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}