using System;
using System.Windows;

namespace GoKart
{
    public partial class MainWindow
    {
        private LapTimeWindow AbsoluteLapTimeWindow;

        private void MenuItem_View_AbsoluteTime_Click(object sender, RoutedEventArgs e)
        {
            if (AbsoluteLapTimeWindow == null)
            {
                AbsoluteLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Absolute);
                AbsoluteLapTimeWindow.Title = "AbsoluteLapTime";
                AbsoluteLapTimeWindow.Closed += AbsoluteLapTimeWindow_Closed;
                AbsoluteLapTimeWindow.Show();
                AbsoluteLapTimeWindow.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }
            else
            {
                if (AbsoluteLapTimeWindow.IsVisible)
                {
                    AbsoluteLapTimeWindow.Close();
                }
            }
        }

        private void AbsoluteLapTimeWindow_Closed(object sender, EventArgs e)
        {
            AbsoluteLapTimeWindow = null;
        }
    }
}