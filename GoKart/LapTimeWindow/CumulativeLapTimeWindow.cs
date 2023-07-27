using System;
using System.Windows;

namespace GoKart
{
    public partial class MainWindow
    {
        private LapTimeWindow CumulativeLapTimeWindow;

        private void MenuItem_View_CumulativeLapTime_Click(object sender, RoutedEventArgs e)
        {
            if (CumulativeLapTimeWindow == null)
            {
                CumulativeLapTimeWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Cumulative);
                CumulativeLapTimeWindow.Title = "CumulativeLapTime";
                CumulativeLapTimeWindow.Closed += CummulatieveRondeTijdenWindow_Closed;
                CumulativeLapTimeWindow.Show();
                CumulativeLapTimeWindow.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }
            else
            {
                if (CumulativeLapTimeWindow.IsVisible)
                {
                    CumulativeLapTimeWindow.Close();
                }
            }
        }

        private void CummulatieveRondeTijdenWindow_Closed(object sender, EventArgs e)
        {
            CumulativeLapTimeWindow = null;
        }
    }
}