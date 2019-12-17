//using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApplication
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        NextDay nextDay;
        public DetailPage()
        {
            InitializeComponent();

            moreInfoButton.Clicked += delegate
            {
                Navigation.PushAsync(nextDay);
            };
        }

        //Publishes information on the DetailPage xaml that was obtained from the district
        public void setWeather(District district, NextDay nextD)
        {

            temperature.Text = district.getMainTemperature().ToString();
            minTemperature.Text = district.getMainMinTemperature().ToString() + " | ";
            maxTemperature.Text = district.getMainMaxTemperature().ToString();
            precipitation.Text = "Precipitation: " + district.getAllClouds().ToString() + "%";
            humidity.Text = "Humidity: " + district.getMainHumidity().ToString();
            pressure.Text = "Pressure: " + district.getMainPressure().ToString();
            wind.Text = "Wind: " + district.getWindSpeed().ToString();
            weatherIconImage.Source = district.getIcon();

            nextDay = nextD;
        }
    }
}