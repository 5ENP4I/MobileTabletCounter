using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    public partial class Logs : ContentPage
    {
        public static List<string> logs = new List<string>();
        public static Medicine _medicine;
        public Logs()
        {
            InitializeComponent();
            Medicine._logs = this;
            //_medicine.LoadData();
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
    }
}