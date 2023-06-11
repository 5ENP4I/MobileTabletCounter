using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileTabletCounter.Models;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    public partial class Settings : ContentPage
    {
        public bool adminMode;
        public static Medicine _medicine;
        public static Logs _logs;
            
        public Settings()
        {
            InitializeComponent();
            Medicine._settings = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (adminMode)
            {
                clearBars.IsVisible = true;
                clearLogs.IsVisible = true;
                clearData.IsVisible = true;
                // Set app color theme to red
                switchModes.Style = (Style)Application.Current.Resources["RedButton"];

            }
            else
            {
                DisplayAlert("Alert", "Welcome to the Settings page!", "OK");
                clearBars.IsVisible = false;
                clearLogs.IsVisible = false;
                clearData.IsVisible = false;
                // Set app color theme to blue
                switchModes.Style = (Style)Application.Current.Resources["BlueButton"];
            }
        }

        private void SwitchModeButton_Clicked(object sender, EventArgs e)
        {
            adminMode = !adminMode;
            OnAppearing();

            if (adminMode)
            {
                clearBars.IsVisible = true;
                clearLogs.IsVisible = true;
                clearData.IsVisible = true;
                // Set app color theme to red
            }
            else
            {
                DisplayAlert("Alert", "Welcome to the Settings page!", "OK");
                clearBars.IsVisible = false;
                clearLogs.IsVisible = false;
                clearData.IsVisible = false;
                // Set app color theme to blue
            }
        }

        private void ClearDataButton_Clicked(object sender, EventArgs e)
        {
            _medicine.RemoveBars();
            _medicine.RemoveLogs();
        }

        private void ClearBarsButton_Clicked(object sender, EventArgs e)
        {
            _medicine.RemoveBars();
        }

        private void ClearLogsButton_Clicked(object sender, EventArgs e)
        {
            _medicine.RemoveLogs();
        }
    }
}