using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MobileTabletCounter.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    public partial class Medicine : ContentPage
    {
        public Medicine()
        {
            InitializeComponent();
            
        }

        public static Logs _logs;


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


            // Create the new bar and add it to the unfilled bars stack layout
            var barView = CreateBarView(bar);
            if(bar.CurrentDoze==bar.MaxDoze)
                filledBarsStackLayout.Children.Add(barView);
            else
                unfilledBarsStackLayout.Children.Add(barView);
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
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if(bar.CurrentDoze < bar.MaxDoze)
                {
                    bar.CurrentDoze++;
                    if(barInside.IsVisible==false) barInside.IsVisible = true;
                    if (bar.CurrentDoze == bar.MaxDoze)
                    {
                        filledBarsStackLayout.Children.Add(mainPanel);
                        unfilledBarsStackLayout.Children.Remove(mainPanel);
                    }
                    progressHelpText.Text = bar.CurrentDoze.ToString();
                    barInside.WidthRequest = bar.Fill;

                    _logs.AddTimeLabel($"{DateTime.Now.ToString("HH:mm (dd.MM.yyyy)")} Used {bar.Name}, progress: {bar.CurrentDoze}/{bar.MaxDoze}");
                }
            };
            mainPanel.GestureRecognizers.Add(tapGestureRecognizer);

            return mainPanel;
        }

        private void RemoveAllButton_Clicked(object sender, EventArgs e)
        {
            unfilledBarsStackLayout.Children.Clear();
            filledBarsStackLayout.Children.Clear();
        }



        //Should be edited asap
        //FUNC FOR LOAD/SAVE
        //public void SaveData()
        //{
        //    var lines = new List<string>();

        //    // Save FinishedMedicine data
        //    foreach (var bar in FinishedMedicine)
        //    {
        //        var line = string.Join("|", bar.Id.Replace("|", "l"), bar.Name.Replace("|", "l"), bar.Description.Replace("|", "l"),
        //            bar.ColorToHex(bar.BarColor), bar.MaxConsumations.Replace("|", "l"), bar.CurrentProgres.Replace("|", "l"));
        //        lines.Add(line);
        //    }

        //    // Save UnfinishedMedicine data
        //    foreach (var bar in UnfinishedMedicine)
        //    {
        //        var line = string.Join("|", bar.Id.Replace("|", "l"), bar.Name.Replace("|", "l"), bar.Description.Replace("|", "l"),
        //            bar.ColorToHex(bar.BarColor), bar.MaxConsumations.Replace("|", "l"), bar.CurrentProgres.Replace("|", "l"));
        //        lines.Add(line);
        //    }

        //    // Save the lines to a text file
        //    var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.txt");
        //    File.WriteAllLines(filePath, lines);
        //}

        //public void LoadData()
        //{
        //    // Check if the save file exists
        //    var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.txt");
        //    if (!File.Exists(filePath))
        //        return;

        //    var lines = File.ReadAllLines(filePath);

        //    foreach (var line in lines)
        //    {
        //        var parameters = line.Split('|').ToArray();
        //        if (parameters.Length == 6)
        //        {
        //            Bar bar = new Bar
        //            {
        //                Id = parameters[0],
        //                Name = parameters[1],
        //                Description = parameters[2],
        //                BarColor = Color.FromHex(parameters[3]),
        //                MaxConsumations = parameters[4],
        //                CurrentProgres = parameters[5]
        //            };

        //            if (int.Parse(bar.MaxConsumations) > int.Parse(bar.CurrentProgres))
        //                UnfinishedMedicine.Add(bar);
        //            else
        //                FinishedMedicine.Add(bar);
        //        }
        //    }

        //    // Calculate the heights of the CollectionViews
        //    UpdateCollectionViews();
        //}

    }
}