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
        public Logs()
        {
            InitializeComponent();
            Medicine._logs = this;
        }

        public void AddTimeLabel(string time)
        {
            View label = new Label { Text = time };
            timeLabelsStackLayout.Children.Add(label);
        }
    }
}