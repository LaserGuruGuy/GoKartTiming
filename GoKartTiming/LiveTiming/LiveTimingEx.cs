﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace GoKartTiming.LiveTiming
{
    public class LiveTimingEx : LiveTiming, ILiveTimingEx, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private DateTime _DateTime = DateTime.Now;

        private new UniqueObservableCollection<DriverEx> _Drivers;

        /* pdf */
        private HeatOverzicht _HeatOverzicht = new HeatOverzicht();

        /* pdf */
        private Records _Records = new Records();

        public DateTime DateTime
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                RaisePropertyChanged();
            }
        }

        public new UniqueObservableCollection<DriverEx> Drivers
        {
            get
            {
                return _Drivers;
            }
            set
            {
                _Drivers = value;
                RaisePropertyChanged();
            }
        }

        /* pdf */
        public HeatOverzicht HeatOverzicht
        {
            get
            {
                return _HeatOverzicht;
            }
            set
            {
                _HeatOverzicht = value;
                RaisePropertyChanged();
            }
        }

        /* pdf */
        public Records Records
        {
            get
            {
                return _Records;
            }
            set
            {
                _Records = value;
                RaisePropertyChanged();
            }
        }

        /* pdf */
        public void Parse(string Page)
        {
            if (Drivers == null)
            {
                Drivers = new UniqueObservableCollection<DriverEx>();
            }

            //string _HeatName = string.Empty;
            //DateTime _DateTime = new DateTime();
            DriverEx Driver = new DriverEx();

            string[] Lines = Page.Split('\n');
            for (int line = 0; line < Lines.Length; line++)
            {
                if (Lines[line].Equals("Persoonlijk Overzicht"))
                {
                    while (Driver.Parse(Lines[line + 1], ref _HeatName, ref _DateTime)) line++;
                    HeatName = _HeatName;
                    DateTime = _DateTime;
                }
                else if (Lines[line].Equals("Uw race geschiedenis (Beste Ronde)"))
                {
                    ;
                }
                else if (Lines[line].Equals("UW BESTE RONDE Gemiddelde snelheid Gemiddelde rondetijd"))
                {
                    line++;
                    string[] Chunk = Lines[line].Split(' ');

                    for (var idx = 0; idx < Chunk.Length; idx++)
                    {
                        switch (idx)
                        {
                            // UW BESTE RONDE
                            case 0:
                                TimeSpan BestLapTime = new TimeSpan();
                                if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
                                {
                                    if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
                                    {
                                        if (TimeSpan.TryParseExact(Chunk[idx], @"h\:mm\:ss\.fff", CultureInfo.InvariantCulture, out BestLapTime) == false)
                                        {
                                            break;
                                        }
                                    }
                                }
                                Driver.BestLapTimeTotalMilliseconds = (int)BestLapTime.TotalMilliseconds;
                                break;
                            // Gemiddelde snelheid
                            case 1:
                                Driver.AverageSpeed = float.Parse(Chunk[idx], new CultureInfo(CultureInfo.InvariantCulture.Name)
                                {
                                    NumberFormat = { NumberDecimalSeparator = "," }
                                });
                                break;
                            // Gemiddelde rondetijd
                            case 2:
                                TimeSpan AvarageLapTime = new TimeSpan();
                                if (TimeSpan.TryParseExact(Chunk[idx], @"s\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
                                {
                                    if (TimeSpan.TryParseExact(Chunk[idx], @"ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
                                    {
                                        if (TimeSpan.TryParseExact(Chunk[idx], @"m\:ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
                                        {
                                            if (TimeSpan.TryParseExact(Chunk[idx], @"mm\:ss\.fff", CultureInfo.InvariantCulture, out AvarageLapTime) == false)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                Driver.AvarageLapTimeTotalMilliseconds = (int)AvarageLapTime.TotalMilliseconds;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (Lines[line].Equals("Ronden"))
                {
                    while (Driver.Parse(Lines[line + 1])) line++;
                }
                else if (Lines[line].Equals("Heat overzicht Beste tijd"))
                {
                    HeatOverzicht.Clear();
                    while (HeatOverzicht.Parse(Lines[line + 1])) line++;
                }
                else if (Lines[line].Equals("Records"))
                {
                    while (Records.Parse(Lines[line + 1])) line++;
                }
            }

            Drivers.Add(Driver);
        }

        private void ResetDateTime()
        {
            _DateTime = DateTime.Now;
        }

        private void ResetDrivers()
        {
            _Drivers.Clear();
        }

        private void ResetHeatOverzicht()
        {
            _HeatOverzicht = new HeatOverzicht();
        }

        private void ResetRecords()
        {
            _Records = new Records();
        }
    }
}