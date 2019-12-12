using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master : ContentPage
    {
        //public Detail detail = new Detail();
        public Master(CheckWeather checkWeather)
        {
            InitializeComponent();

            buttonA.Clicked += delegate
            {
                checkWeather.getWeather(buttonA.Text);
            };

            buttonB.Clicked += delegate
            {
                checkWeather.getWeather(buttonB.Text);
            };
        }
    }
}