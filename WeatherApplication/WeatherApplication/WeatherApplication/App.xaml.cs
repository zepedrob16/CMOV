using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CheckWeather();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
