using System;
using System.Windows;

namespace GoKart
{
    public partial class MainWindow
    {
        private LapTimeWindow PositionWindow;

        private void MenuItem_View_RacePosition_Click(object sender, RoutedEventArgs e)
        {
            if (PositionWindow == null)
            {
                PositionWindow = new LapTimeWindow(LapTimeWindow.LapTimeType.Position);
                PositionWindow.Title = "RacePosition";
                PositionWindow.Closed += PositionWindow_Closed;
                PositionWindow.Show();
                PositionWindow.UpdatePlot(ListView_LiveTiming.SelectedItems.Count.Equals(0) ? ListView_LiveTiming.Items : ListView_LiveTiming.SelectedItems, ListView_LiveTiming.SelectedItem);
            }
            else
            {
                if (PositionWindow.IsVisible)
                {
                    PositionWindow.Close();
                }
            }
        }

        private void PositionWindow_Closed(object sender, EventArgs e)
        {
            PositionWindow = null;
        }
    }
}