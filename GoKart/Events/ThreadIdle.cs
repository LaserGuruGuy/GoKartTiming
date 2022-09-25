using System;

namespace GoKart
{
    public partial class MainWindow
    {
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
            }
        }
    }
}