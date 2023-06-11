using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    public partial class Logs : ContentPage
    {
        public static List<string> logs = new List<string>();
        public static Medicine _medicine;
        public static Settings _settings;
        public Logs()
        {
            InitializeComponent();
            Medicine._logs = this;
            Settings._logs = this;
            LoadLogs();
        }

        public void AddTimeLabel(string time)
        {
            View label = new Label { Text = time };
            timeLabelsStackLayout.Children.Add(label);
            logs.Add(time);
        }

        public void Clear()
        {
            logs.Clear();
            timeLabelsStackLayout.Children.Clear();
        }

        public void LoadLogs()
        {
            Clear();

            var fileLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveLogs.txt");
            if (!File.Exists(fileLogsPath))
                return;

            StreamReader srLogs = new StreamReader(fileLogsPath);
            var linesLogs = srLogs.ReadToEnd().Split('\n');
            srLogs.Close();
            foreach (var log in linesLogs)
            {
                AddTimeLabel(log);

            }

        }

        public void SaveLogs() 
        {
            var fileLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveLogs.txt");
            File.WriteAllLines(fileLogsPath, logs);
        }
    }
}