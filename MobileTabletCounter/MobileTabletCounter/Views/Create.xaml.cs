using MobileTabletCounter.Models;
using MobileTabletCounter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Create : ContentPage
    {
        //PARAMETRS AND CONSTRUCTOR
        private Bar futureBar;
        public bool noProblemsUponChecking = false;

        public Create(Bar bar)
        {
            futureBar = bar;
            InitializeComponent();
        }

        //CONFIRM/CANCEL BUTON + METHOD
        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            CheckIUpdateEnteries();
            if (noProblemsUponChecking)
            {
                await Navigation.PopModalAsync();
                tcs?.TrySetResult(true);
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            futureBar.MaxDoze = -1;
            await Navigation.PopModalAsync();
            tcs?.TrySetResult(false);
        }

        private TaskCompletionSource<bool> tcs;
        public Task<bool> WaitForConfirmationAsync()
        {
            tcs = new TaskCompletionSource<bool>();
            return tcs.Task;
        }


        //RENDER PREVIEV
        public void RenderPreview()
        {
            nameLb.Text = futureBar.Name;
            descriptionLb.Text = futureBar.Description;
            barOutside.BorderColor = futureBar.Color;
            barInside.BorderColor = futureBar.Color;
            barInside.WidthRequest = futureBar.Fill;
            barInside.BackgroundColor = futureBar.Color;
            maxDozeLb.Text = futureBar.MaxDoze.ToString();
            currentDozeLb.Text = futureBar.CurrentDoze.ToString();
        }


        //EVENTS
        private void RandomColorButton_Clicked(object sender, EventArgs e)
        {
            RandomColor();
            string hexColor = RandomColor().ToHex();

            // Set the color entry value
            ColorEntry.Text = hexColor;
            CheckIUpdateEnteries();
            RenderPreview();
        }

        private void AnyEntry_Unfocused(object sender, FocusEventArgs e)
        {
            CheckIUpdateEnteries();
            RenderPreview();
        }


        //FUNC FOR CHEKING
        private bool IsNumeric(string input)
        {
            if (int.TryParse(input, out _)&&IsEmptyOrWhiteSpace(input)==false)
                return true;
            return false;
        }

        private bool IsEmptyOrWhiteSpace(string input)
        {
            if (string.IsNullOrWhiteSpace(input)==true||input == null)
                return true;
            return false;
        }

        private bool IsHexColor(string input)
        {
            Color[] excludedColors = {
                (Color)Application.Current.Resources["AntiFlashWhite"],
                (Color)Application.Current.Resources["Turquoise"],
                (Color)Application.Current.Resources["Black"]
            };
            if (IsEmptyOrWhiteSpace(input)|| input[0] != '#')
                return false;

            string colorCode = input.ToUpperInvariant();
            return !excludedColors.Any(color => color.ToHex().ToUpperInvariant() == colorCode);
        }


        //MAJOR CHECKING FUNC
        public void CheckIUpdateEnteries()
        {
            noProblemsUponChecking = true;

            //Name check
            if (IsEmptyOrWhiteSpace(NameEntry.Text)==false)
            {
                futureBar.Name = NameEntry.Text;
                ErrorLabel.Text = "";
            }
            else
            {
                ErrorLabel.Text = "Not valid (Name)";
                noProblemsUponChecking = false;
                return;
            }

            //Description check
            if (IsEmptyOrWhiteSpace(DescriptionEntry.Text)==false)
            {
                futureBar.Description = DescriptionEntry.Text;
                ErrorLabel.Text = "";
            }
            else
            {
                ErrorLabel.Text = "Not valid (Description)";
                noProblemsUponChecking = false;
                return;
            }

            //Color check
            if (IsHexColor(ColorEntry.Text))
            {
                futureBar.Color = Color.FromHex(ColorEntry.Text);
                ErrorLabel.Text = "";
            }
            else
            {
                ErrorLabel.Text = "Not valid (Color)";
                noProblemsUponChecking = false;
                return;
            }

            //MaxDoze Check
            if (IsNumeric(MaxDozeEntry.Text))
            {
                if (int.Parse(MaxDozeEntry.Text) > 0)
                {
                    futureBar.MaxDoze = int.Parse(MaxDozeEntry.Text);
                    ErrorLabel.Text = "";
                }
                else
                {
                    ErrorLabel.Text = "Not valid (Max doze)";
                    noProblemsUponChecking = false;
                    return;
                }
            }
            else
            {
                ErrorLabel.Text = "Not valid (Max doze)";
                noProblemsUponChecking = false;
                return;
            }

            //CurrentDoze Check
            if (IsNumeric(CurrentDozeEntry.Text))
            {
                if ((int.Parse(CurrentDozeEntry.Text) >= 0) && (int.Parse(MaxDozeEntry.Text) >= int.Parse(CurrentDozeEntry.Text)))
                {
                    futureBar.CurrentDoze = int.Parse(CurrentDozeEntry.Text);
                    ErrorLabel.Text = "";
                }
                else
                {
                    ErrorLabel.Text = "Not valid (Current doze or Max doze)";
                    noProblemsUponChecking = false;
                    return;
                }
            }
            else
            {
                ErrorLabel.Text = "Not valid (Current doze)";
                noProblemsUponChecking = false;
                return;
            }
        }


        //FUNC FOR GENERATION
        public static Color RandomColor()
        {
            // Generate a random color
            Random random = new Random();
            return Color.FromHex(String.Format("#{0:X6}", random.Next(0x1000000)));
        }

    }
}
