using System;
using System.Windows;
using System.Windows.Controls;

namespace GoKart
{
    public partial class MainWindow
    {
        private void listView_UpdateWidth(ListView listView)
        {
            GridView gridView = listView.View as GridView;

            foreach (var Column in gridView.Columns)
            {
                if (double.IsNaN(Column.Width))
                {
                    Column.Width = Column.ActualWidth;
                }
                Column.Width = double.NaN;
            }
        }

        void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CpbTiming.RaceStatus))
                ListView_LiveTiming.IsSynchronizedWithCurrentItem = false;
            else
                ListView_LiveTiming.IsSynchronizedWithCurrentItem = true;

            if (UpdateLiveTiming)
            {
                UpdateLiveTiming = false;
                //ListView_LiveTimingCollection.Items.Refresh();
            }

            if (UpdateDriver)
            {
                UpdateDriver = false;
                //ListView_LiveTiming.Items.Refresh();

            }

            if (UpdatePosition)
            {
                UpdatePosition = false;
                //ListViewSort(ListView_LiveTiming, "Position");
            }

            if (UpdateLapTime)
            {
                UpdateLapTime = false;
                //ListView_LapTime.Items.Refresh();
                //ListView_LapTimePosition.Items.Refresh();
                listView_UpdateWidth(ListView_LiveTiming);

                AbsoluteLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                CumulativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                RelativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                PositionWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }

            if (UpdateRecordGroup)
            {
                UpdateRecordGroup = false;
                listView_UpdateWidth(ListView_BestTimingCollection);
            }

            System.Threading.Thread.Sleep(0);
        }
    }
}