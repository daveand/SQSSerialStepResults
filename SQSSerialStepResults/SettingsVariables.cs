using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQSSerialStepResults
{
    public class SettingsVariables : INotifyPropertyChanged
    {
        private string keepResultsDays = ConfigurationManager.AppSettings.Get("KeepResultsDays");
        public string KeepResultsDays
        {
            get { return keepResultsDays; }
            set
            {
                keepResultsDays = value;
                //GetValue();
                OnPropertyChanged("KeepResultsDays");
            }
        }

        public string GetNewValue()
        {
            ConfigurationManager.RefreshSection("appSettings");
            KeepResultsDays = ConfigurationManager.AppSettings.Get("KeepResultsDays");
            Console.WriteLine($"Return Value: {KeepResultsDays}");
            return KeepResultsDays;
        }



        // Extend the INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                // Alert anyone bound to this that the value has changed
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
    }
}
