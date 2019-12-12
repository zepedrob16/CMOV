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
        public DetailPage()
        {
            InitializeComponent();
        }

        public void setWeather(District district)
        {

            temperature.Text = district.getMainTemperature().ToString();
            minTemperature.Text = district.getMainMinTemperature().ToString() + " | ";
            maxTemperature.Text = district.getMainMaxTemperature().ToString();
            precipitation.Text = "Precipitation: " + district.getAllClouds().ToString() + "%";
            humidity.Text = "Humidity: " + district.getMainHumidity().ToString();
            pressure.Text = "Pressure: " + district.getMainPressure().ToString();
            wind.Text = "Wind: " + district.getWindSpeed().ToString();

        }
    }
}