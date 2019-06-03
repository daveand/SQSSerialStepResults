using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Xml;

namespace SQSSerialStepResults
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<ResultModel> results = new List<ResultModel>();

        public static MainWindow AppWindow;

        public MainWindow()
        {
            ConsoleManager.Show();
            InitializeComponent();

            
            textblock.DataContext = new MyCommPort();
            keepresults.DataContext = new SettingsVariables();
            
            

            SqliteDataAccess dataAccess = new SqliteDataAccess();

            string keepResultsDays = ConfigurationManager.AppSettings.Get("KeepResultsDays");
            string timeDelta = dataAccess.DeleteResult(keepResultsDays);
            Console.WriteLine(timeDelta); 

            LoadResultsList();
           
            AppWindow = this;

            
        }



        public void LoadResultsList()
        {
            results = SqliteDataAccess.LoadResults();
            foreach (var i in results)

                // Enable setting of variable from other class
                this.Dispatcher.Invoke(() =>
                {
                    lstBox.ItemsSource = results;
                });
        }


        private void Button_Click(object sender, RoutedEventArgs e)

        {

            //if (!string.IsNullOrEmpty(keepresults.Text))

            //{

                UpdateConfigKey("KeepResultsDays", keepresults.Text);
                keepresults.Text = string.Empty;


                // Enable setting of variable from other class
                this.Dispatcher.Invoke(() =>
                {
                    SettingsVariables newValue = new SettingsVariables();
                    keepresults.Text = newValue.GetNewValue();
                    
                    //Console.WriteLine(settingstext2.Text);
                });

            //}

            //else

                //MessageBox.Show("Please type some value.");

        }

        public void UpdateConfigKey(string strKey, string newValue)

        {

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");



            if (!ConfigKeyExists(strKey))

            {

                throw new ArgumentNullException("Key", "<" + strKey + "> not find in the configuration.");

            }

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");



            foreach (XmlNode childNode in appSettingsNode)

            {

                if (childNode.Attributes["key"].Value == strKey)

                    childNode.Attributes["value"].Value = newValue;

            }

            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            
            MessageBox.Show("Key Upated Successfullly");

        }

        public bool ConfigKeyExists(string strKey)

        {

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");



            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");



            foreach (XmlNode childNode in appSettingsNode)

            {

                if (childNode.Attributes["key"].Value == strKey)

                    return true;

            }

            return false;

        }

    }
}

