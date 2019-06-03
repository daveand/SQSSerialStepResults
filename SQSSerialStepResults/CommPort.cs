using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Configuration;
using System.Windows;

namespace SQSSerialStepResults
{
    class MyCommPort : INotifyPropertyChanged
    {


        SerialPort serialPort = null;
        public MyCommPort()
        {
            string comPort = ConfigurationManager.AppSettings.Get("comPort");
            int comBaud = int.Parse(ConfigurationManager.AppSettings.Get("comBaud"));
            serialPort = new SerialPort(comPort, comBaud);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {

                MessageBox.Show($"{comPort} could not be opened.\nConfigure another COM-port in \"SQSSerialStepResults.exe.config\"", "Error", MessageBoxButton.OK);
            }
        }
        ~MyCommPort()
        {
            serialPort.Close();
        }


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            //string testString = null;
            string indata = sp.ReadTo("%E%");



            BlockData = indata;
            
            Console.WriteLine(indata);
            
            ResultModel result = new ResultModel();

            result.ResultData = indata;
            
            SqliteDataAccess.SaveResult(result);

            MainWindow.AppWindow.LoadResultsList();


        }



        private string blockData;
        public string BlockData
        {
            get { return blockData; }
            set
            {
                blockData = value;
                OnPropertyChanged("BlockData");
            }
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
