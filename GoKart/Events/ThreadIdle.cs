using System;
using System.Windows;
using System.Windows.Controls;

namespace GoKart
{
    public partial class MainWindow
    {
        private void listView_UpdateWidth(ListView listView)
        {
            //    GridView gridView = listView.View as GridView;

            //    var ActualWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            //    for (var i = 0; i < gridView.Columns.Count; i++)
            //    {
            //        gridView.Columns[i].Width = gridView.Columns[i].ActualWidth;
            //        //ActualWidth += gridView.Columns[i].ActualWidth;
            //    }

            //    // listView.Width = ActualWidth + SystemParameters.VerticalScrollBarWidth;
        }

        void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
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

                AbsoluteLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                CumulativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
                RelativeLapTimeWindow?.UpdatePlot(ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);

                listView_UpdateWidth(ListView_LiveTiming);
            }
        }
    }
}