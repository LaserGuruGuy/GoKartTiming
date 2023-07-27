using System;
using System.Windows;

namespace GoKart
{
    public partial class MainWindow
    {
        private LapTimeWindow RelativeLapTimeWindow;

        private void MenuItem_View_RelativeTime_Click(object sender, RoutedEventArgs e)
        {
            if (RelativeLapTimeWindow == null)
            {
                RelativeLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Relative);
                RelativeLapTimeWindow.Title = "RelativeLapTime";
                RelativeLapTimeWindow.Closed += RelatieveRondeTijdenWindow_Closed;
                RelativeLapTimeWindow.Show();
                RelativeLapTimeWindow.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }
            else
            {
                if (RelativeLapTimeWindow.IsVisible)
                {
                    RelativeLapTimeWindow.Close();
                }
            }
        }

        private void RelatieveRondeTijdenWindow_Closed(object sender, EventArgs e)
        {
            RelativeLapTimeWindow = null;
        }
    }
}