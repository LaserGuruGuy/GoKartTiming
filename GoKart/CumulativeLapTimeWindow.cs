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
                CumulativeLapTimeWindow.Closed += CummulatieveRondeTijdenWindow_Closed;
                CumulativeLapTimeWindow.Show();
                CumulativeLapTimeWindow.UpdatePlot(ListView_RaceOverviewReport.SelectedItems, ListView_RaceOverviewReport.SelectedItem);
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