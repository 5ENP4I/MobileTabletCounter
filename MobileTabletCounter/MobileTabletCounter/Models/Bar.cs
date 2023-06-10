using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileTabletCounter.Models
{
    public class Bar
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxDoze { get; set; }
        public int CurrentDoze { get; set; }
        public float Fill 
        {
            get 
            {
                return 200f/MaxDoze*CurrentDoze;
            }
        }
        public Color Color { get; set; }

        public Bar(string name, string description, int maxDoze, int currentDoze, Color color)
        {
            Name = name;
            Description = description;
            MaxDoze = maxDoze;
            CurrentDoze = currentDoze;
            Color = color;
        }

        public Bar()
        {

        }
    }
}
