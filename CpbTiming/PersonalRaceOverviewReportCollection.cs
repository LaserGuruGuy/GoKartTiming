using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CpbTiming
{
    public class PersonalRaceOverviewReportCollection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<PersonalRaceOverviewReport> _RaceOverviewReportCollection = new ObservableCollection<PersonalRaceOverviewReport>();

        public ObservableCollection<PersonalRaceOverviewReport> RaceOverviewReportCollection
        {
            get
            {
                return _RaceOverviewReportCollection;
            }
            set
            {
                _RaceOverviewReportCollection = value;
                RaisePropertyChanged("RaceOverviewReportCollection");
            }
        }

        private System.Windows.Input.ICommand _DeletePersonalRaceOverviewReport;

        public System.Windows.Input.ICommand DeletePersonalRaceOverviewReport => _DeletePersonalRaceOverviewReport ?? (_DeletePersonalRaceOverviewReport = new RelayCommand<PersonalRaceOverviewReport>(DeletePersonalRaceOverviewReportCommand));

        public void DeletePersonalRaceOverviewReportCommand(PersonalRaceOverviewReport PersonalRaceOverviewReport)
        {
            RaceOverviewReportCollection.Remove(PersonalRaceOverviewReport);
        }

        public PersonalRaceOverviewReportCollection()
        {
        }

        public void Parse(string Title, DateTime DateTime, List<string> Book)
        {
            RaceOverviewReportCollection.Add(new PersonalRaceOverviewReport(Title, DateTime, Book));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}