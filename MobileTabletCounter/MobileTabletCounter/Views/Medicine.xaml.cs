using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MobileTabletCounter.Models;

using System.Diagnostics;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    public partial class Medicine : ContentPage
    {

        //CONSTRUCTOR AND PARAMETRS
        public Medicine()
        {
            Settings._medicine = this;
            Logs._medicine = this;
            InitializeComponent();
            LoadData();
        }
        public static Logs _logs;
        public static Settings _settings;
        public static List<Bar> bars = new List<Bar>();

        private async void CreateBarButton_Clicked(object sender, EventArgs e)
        {
            Bar bar = new Bar("Name", "Description", 3,1,Color.Green);

            // Dialog with the user about the futureBar (barFromCreate)
            Create dialogPage = new Create(bar);
            var tcs = new TaskCompletionSource<bool>();
            dialogPage.Disappearing += (s, args) => tcs.TrySetResult(true);
            await Navigation.PushModalAsync(dialogPage);

            // Wait for the dialog to complete before continuing
            await tcs.Task;
            if (bar.MaxDoze == -1)
                return;

            // Create the new bar and add it to the unfilled bars stack layout
            var barView = CreateBarView(bar);
            if(bar.CurrentDoze==bar.MaxDoze)
                filledBarsStackLayout.Children.Add(barView);
            else
                unfilledBarsStackLayout.Children.Add(barView);
            bars.Add(bar);
        }

        
        //CREATES A "BAR"
        private View CreateBarView(Bar bar)
        {
            //Because Xamarin and Binding sucks
            #region
            StackLayout mainPanel = new StackLayout();

            Label textAbove = new Label { Text = bar.Name, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.End };
            Label textBelow = new Label { Text = bar.Description, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start };

            Frame barOutside = new Frame { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, WidthRequest = 200, HeightRequest = 15, BorderColor = bar.Color, CornerRadius = 50 };
            Frame barInside = new Frame { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, WidthRequest = bar.Fill, HeightRequest = 15, BackgroundColor = bar.Color, BorderColor = bar.Color, CornerRadius = 50 };

            Label progressText = new Label { Text = bar.MaxDoze.ToString(), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            Label progressHelpText = new Label { Text = bar.CurrentDoze.ToString(), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            Label slash = new Label { Text = "/", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

            StackLayout progressCounter = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            progressCounter.Children.Add(progressHelpText);
            progressCounter.Children.Add(slash);
            progressCounter.Children.Add(progressText);

            Grid gridInside = new Grid();
            gridInside.Children.Add(barOutside);
            gridInside.Children.Add(barInside);
            gridInside.Children.Add(progressCounter);

            mainPanel.Children.Add(textAbove);
            mainPanel.Children.Add(gridInside);
            mainPanel.Children.Add(textBelow);
            #endregion

            if(bar.CurrentDoze==0)
            {
                barInside.IsVisible = false;
            }

            var tapGestureRecognizer = new TapGestureRecognizer();
            bool isCodeDisabled = false;

            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (!isCodeDisabled && bar.CurrentDoze < bar.MaxDoze)
                {
                    isCodeDisabled = true; // Disable the code execution

                    bar.CurrentDoze++;
                    if (!barInside.IsVisible)
                        barInside.IsVisible = true;
                    if (bar.CurrentDoze == bar.MaxDoze)
                    {
                        filledBarsStackLayout.Children.Add(mainPanel);
                        unfilledBarsStackLayout.Children.Remove(mainPanel);
                    }
                    progressHelpText.Text = bar.CurrentDoze.ToString();
                    barInside.WidthRequest = bar.Fill;
                    bar.LastConsume = DateTime.Now.Date;

                    _logs.AddTimeLabel($"{DateTime.Now.ToString("HH:mm (dd.MM.yyyy)")} Used {bar.Name}, progress: {bar.CurrentDoze}/{bar.MaxDoze}");

                    // Start a timer to reset the flag after a delay
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        isCodeDisabled = false; // Enable the code execution
                        return false; // Stop the timer
                    });
                    SaveData();
                }
            };
            mainPanel.GestureRecognizers.Add(tapGestureRecognizer);

            return mainPanel;
        }

        
        //ON APEARING
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ToggleAddOnOff();
        }

        public void ToggleAddOnOff()
        {
            if(_settings == null||_settings.adminMode == false)
            {
                AddButton.IsEnabled = false;
            }
            else
            {
                AddButton.IsEnabled = true;
            }
        }


        //FOR SETTINGS

        public void RemoveLogs()
        {
            _logs.Clear();
            SaveData();
        }

        public void RemoveBars()
        {
            bars.Clear();
            unfilledBarsStackLayout.Children.Clear();
            filledBarsStackLayout.Children.Clear();
            SaveData();
        }

        //FUNC FOR LOAD/SAVE
        
        public void SaveData()
        {
            var lines = new List<string>();

            // Save data as string
            foreach (var bar in bars)
            {
                var line = string.Join("|", bar.Name.Replace("|", "l"), bar.Description.Replace("|", "l"),
                    bar.Color.ToHex(), bar.MaxDoze, bar.CurrentDoze, bar.LastConsume);
                lines.Add(line);
                Debug.WriteLine(line);
            }

            // Save the lines to a text file
            var fileBarsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveBars.txt");
            File.WriteAllLines(fileBarsPath, lines);

            var fileLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveLogs.txt");
            File.WriteAllLines(fileLogsPath, Logs.logs);

            Debug.WriteLine("Saved");
        }

        public void LoadData()
        {
            // Check if the save file exists
            var fileBarsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveBars.txt");
            if (!File.Exists(fileBarsPath))
                return;

            var fileLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "saveLogs.txt");
            if (!File.Exists(fileLogsPath))
                return;

            //Clears all Children in app
            unfilledBarsStackLayout.Children.Clear();
            filledBarsStackLayout.Children.Clear();

            //Read all lines 
            StreamReader sr = new StreamReader(fileBarsPath);
            var lines = sr.ReadToEnd().Split('\n');
            sr.Close();

            StreamReader srLogs = new StreamReader(fileLogsPath);
            var linesLogs = srLogs.ReadToEnd().Split('\n');
            srLogs.Close();
            foreach (var log in linesLogs)
            {
                _logs.AddTimeLabel(log);
                
            }

            foreach (var line in lines)
            {
                var parameters = line.Split('|').ToArray();
                if (parameters.Length == 6)
                {
                    Bar bar = new Bar
                    {
                        Name = parameters[0],
                        Description = parameters[1],
                        Color = Color.FromHex(parameters[2]),
                        MaxDoze = int.Parse(parameters[3]),
                        CurrentDoze = int.Parse(parameters[4]),
                        LastConsume = DateTime.Parse(parameters[5])
                    };

                    Debug.WriteLine($"{bar.LastConsume.Date} < {DateTime.Now.Date}");

                    DateTime currentDate = DateTime.Now;
                    if (bar.LastConsume.Date < currentDate.Date)
                    {
                        bar.CurrentDoze = 0;
                    }

                    if (bar.MaxDoze > bar.CurrentDoze)
                        unfilledBarsStackLayout.Children.Add(CreateBarView(bar));
                    else
                        filledBarsStackLayout.Children.Add(CreateBarView(bar));
                    bars.Add(bar);
                }
            }
            Debug.WriteLine("Loaded");
        }

    }
}