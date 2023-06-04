using MobileTabletCounter.Models;
using MobileTabletCounter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileTabletCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Create : ContentPage
    {
        private Bar futureBar;
        public string[] values = new string[5];

        public Create()
        {
            InitializeComponent();
        }

        //EVENT FOR SAVING AND CONFIRMING
        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            // Check for negative values


            // Close the dialog page

            if (futureBar.CurrentDoze >= 0 && futureBar.MaxDoze >= 0 && futureBar.MaxDoze>= futureBar.CurrentDoze)
            {
                ErrorLabel.Text = "Check doze parametrs!";
                return;
            }

            Navigation.PopModalAsync();
        }

        //LEAVIG TB EVENT

        //FUNC FOR CHEKING

        private void RandomColorButton_Clicked(object sender, EventArgs e)
        {
            // Generate a random color
            Random random = new Random();
            string hexColor = String.Format("#{0:X6}", random.Next(0x1000000));

            // Set the color entry value
            ColorEntry.Text = hexColor;
        }

        private bool IsNumeric(string input)
        {
            if (double.TryParse(input, out _)||float.TryParse(input, out _)||int.TryParse(input, out _))
                return true;
            return false;
        }

        private bool IsEmptyOrWhiteSpace(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        //Make "June" wariant
        private bool IsHexColor(string input)
        {
            Color[] excludedColors = {
                (Color)Application.Current.Resources["Cerise"],
                (Color)Application.Current.Resources["AntiFlashWhite"],
                (Color)Application.Current.Resources["ByzantineBlue"],
                (Color)Application.Current.Resources["Turquoise"],
                (Color)Application.Current.Resources["Black"]
            };
            if (input.Length != 7 || input[0] != '#')
                return false;

            string colorCode = input.ToUpperInvariant();
            return !excludedColors.Any(color => color.ToHex().ToUpperInvariant() == colorCode);
        }

    }
}