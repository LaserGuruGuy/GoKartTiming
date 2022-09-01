using System;
using System.Collections.Generic;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using OxyPlot.Legends;
using CpbTiming.SmsTiming;
using OxyPlot.Series;

namespace GoKart
{
    public partial class LapTimeWindow : Window
    {
        public PlotModel PlotModel { get; private set; } = new PlotModel() { EdgeRenderingMode = EdgeRenderingMode.Adaptive };

        private PngExporter pngExporter = new PngExporter { Width = 1920, Height = 1080 };

        public enum LapTimeType
        {
            Absolute = 0,
            Cumulative = 1,
            Relative = 2
        }

        private LapTimeType SelectedLapTimeType;

        public LapTimeWindow(LapTimeType LapTimeType = LapTimeType.Absolute)
        {
            InitializeComponent();

            SelectedLapTimeType = LapTimeType;

            InitPlot();
        }

        public void InitPlot()
        {
            PlotModel.Legends.Clear();
            PlotModel.Axes.Clear();

            PlotModel.PlotType = PlotType.XY;

            PlotModel.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.RightTop
            });

            LinearAxis LinearAxisBottomX = new LinearAxis
            {
                Title = "Laps",
                Key = "Bottom",
                Position = AxisPosition.Bottom,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };
            PlotModel.Axes.Add(LinearAxisBottomX);

            LinearAxis LinearAxisLeftY = new LinearAxis
            {
                Title = "Time",
                Unit = "seconds",
                Key = "Left",
                Position = AxisPosition.Left,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };
            PlotModel.Axes.Add(LinearAxisLeftY);
        }

        private List<LineSeries> LineSeries = new List<LineSeries>();

        public void UpdatePlot(System.Collections.IList SelectedItems, Object ReferencedItem = null)
        {
            PlotModel.Series.Clear();

            Driver ReferenceRaceOverviewReport = null;
            try
            {
                ReferenceRaceOverviewReport = ReferencedItem as Driver;
            }
            catch
            {
            }
            finally
            {
                switch(SelectedLapTimeType)
                {
                    case LapTimeType.Absolute:
                        PlotModel.Title = "Absolute laptime";
                        break;
                    case LapTimeType.Cumulative:
                        PlotModel.Title = "Cumulative laptime";
                        break;
                    case LapTimeType.Relative:
                        PlotModel.Title = "Relative laptime";
                        break;
                }
            }

            try
            {
                foreach (var SelectedItem in SelectedItems)
                {
                    Driver Driver = SelectedItem as Driver;
                    LineSeries LineSeries = new LineSeries()
                    {
                        Title = "#" + Driver.Position.ToString() + " " + Driver.DriverName + " Car " + Driver.KartNumber,
                        XAxisKey = "Bottom",
                        YAxisKey = "Left",
                        StrokeThickness = 1
                    };
                    double y = 0;
                    switch (SelectedLapTimeType)
                    {
                        case LapTimeType.Absolute:
                            foreach (var LapTime in Driver.LapTime)
                            {
                                LineSeries.Points.Add(new DataPoint(LapTime.Key, LapTime.Value.TotalSeconds));
                            }
                            break;
                        case LapTimeType.Cumulative:
                            foreach (var LapTime in Driver.LapTime)
                            {
                                y += LapTime.Value.TotalSeconds;
                                LineSeries.Points.Add(new DataPoint(LapTime.Key, y));
                            }
                            break;
                        case LapTimeType.Relative:
                            foreach (var LapTime in Driver.LapTime)
                            {
                                foreach(var RefTime in ReferenceRaceOverviewReport.LapTime)
                                {
                                    if (LapTime.Key.Equals(RefTime.Key))
                                    {
                                        y += LapTime.Value.TotalSeconds - RefTime.Value.TotalSeconds;
                                        LineSeries.Points.Add(new DataPoint(LapTime.Key, y));
                                        break;
                                    }
                                }                               
                            }
                            break;
                    }
                    PlotModel.Series.Add(LineSeries);
                }
            }
            finally
            {
                PlotModel.InvalidatePlot(true);
            }
        }

        private void Oxy_PlotView_Save_OnRightClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            dlg.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Environment.MachineName + "_" + Environment.UserName + "_" + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local).ToString("yyyyMMdd_hhmmss_") + "GoKartTiming.png";
            dlg.DefaultExt = ".png";
            dlg.Filter = "(.png)|*.png";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                pngExporter.ExportToFile(PlotModel, dlg.FileName);
            }
        }
    }
}