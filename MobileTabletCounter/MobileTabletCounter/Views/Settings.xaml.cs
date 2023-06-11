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
            Logs._settings = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (adminMode)
            {
                clearBars.IsVisible = true;
                clearLogs.IsVisible = true;
                clearData.IsVisible = true;

                _medicine.Title = "Admin";
                _logs.Title = "Admin";
                this.Title = "Admin";
            }
            else
            {
                DisplayAlert("Alert", "Dont click here if you dont want to destroy your tabs!", "OK");
                clearBars.IsVisible = false;
                clearLogs.IsVisible = false;
                clearData.IsVisible = false;

                _medicine.Title = "Medicine";
                _logs.Title = "Logs";
                this.Title = "Settings";
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
            }
            else
            {
                clearBars.IsVisible = false;
                clearLogs.IsVisible = false;
                clearData.IsVisible = false;
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