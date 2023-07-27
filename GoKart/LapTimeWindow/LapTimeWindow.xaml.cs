using System;
using System.Collections.Generic;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using OxyPlot.Legends;
using GoKartTiming.LiveTiming;
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
            Relative = 2,
            Position = 3
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

            TimeSpanAxis TimeSpanAxisBottomX = new TimeSpanAxis
            {
                Title = "Time",
                Unit = "h:mm:ss",
                Key = "Bottom",
                Minimum = 0,
                Position = AxisPosition.Bottom,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid,
            };

            LinearAxis LinearAxisBottomX = new LinearAxis
            {
                Title = "Laps",
                Unit = null,
                Key = "Bottom",
                Position = AxisPosition.Bottom,
                MinorStep = 1,
                MajorStep = 1,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };

            if (SelectedLapTimeType == LapTimeType.Position)
            {
                PlotModel.Axes.Add(TimeSpanAxisBottomX);
            }
            else
            {
                PlotModel.Axes.Add(LinearAxisBottomX);
            }

            LinearAxis LinearAxisLeftY = new LinearAxis
            {
                Title = "Position",
                Key = "Left",
                Position = AxisPosition.Left,
                Minimum = 1,
                MinorStep = 1,
                MajorStep = 1,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };

            TimeSpanAxis TimeSpanAxisLeftY = new TimeSpanAxis
            {
                Title = "Time",
                Unit = "h:mm:ss",
                Key = "Left",
                Minimum = 0,
                Position = AxisPosition.Left,
                MajorGridlineColor = OxyColor.FromArgb(40, 100, 0, 139),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 139),
                MinorGridlineStyle = LineStyle.Solid
            };

            if (SelectedLapTimeType == LapTimeType.Position)
            {
                PlotModel.Axes.Add(LinearAxisLeftY);
            }
            else
            {
                PlotModel.Axes.Add(TimeSpanAxisLeftY);
            }

            switch (SelectedLapTimeType)
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
                case LapTimeType.Position:
                    PlotModel.Title = "Race Position";
                    break;
            }
        }
        
        //TODO: do not redraw all points but merely add new ones
        private List<LineSeries> LineSeries = new List<LineSeries>();

        public void UpdatePlot(System.Collections.IList SelectedItems, Object ReferencedItem = null)
        {
            PlotModel.Series.Clear();

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
                        StrokeThickness = 2
                    };
                    TimeSpan TotalTime = TimeSpan.Zero;
                    switch (SelectedLapTimeType)
                    {
                        case LapTimeType.Absolute:
                            foreach (var LapTime in Driver.LapTime)
                            {
                                LineSeries.Points.Add(new DataPoint(LapTime.Key, TimeSpanAxis.ToDouble(LapTime.Value)));
                            }
                            break;
                        case LapTimeType.Cumulative:
                            foreach (var LapTime in Driver.LapTime)
                            {
                                TotalTime += LapTime.Value;
                                LineSeries.Points.Add(new DataPoint(LapTime.Key, TimeSpanAxis.ToDouble(TotalTime)));
                            }
                            break;
                        case LapTimeType.Relative:
                            Driver ReferencedDriver = ReferencedItem as Driver;
                            if (ReferencedDriver != null)
                            {
                                foreach (var LapTime in Driver.LapTime)
                                {
                                    foreach (var RefTime in ReferencedDriver.LapTime)
                                    {
                                        if (LapTime.Key.Equals(RefTime.Key) && RefTime.Key > 0 && LapTime.Key > 0)
                                        {
                                            TotalTime += LapTime.Value - RefTime.Value;
                                            LineSeries.Points.Add(new DataPoint(LapTime.Key, TimeSpanAxis.ToDouble(TotalTime)));
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case LapTimeType.Position:
                            foreach (var LapTimePosition in Driver.LapTimePosition)
                            {
                                LineSeries.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(LapTimePosition.Value), LapTimePosition.Key));
                            }
                            break;
                    }
                    PlotModel.Series.Add(LineSeries);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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