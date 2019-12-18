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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            drawingView.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            drawingView.OnDisappearing();
        }

        //Publishes information on the DetailPage xaml that was obtained from the district
        public void setWeather(District district, NextDay nextD)
        {
            cityName.Text = district.getCityName().ToString();
            mainTemperature.Text = district.getMainTemperature().ToString() + "ºC";
            temperature.Text = "Min: " + district.getMainMinTemperature().ToString() + "ºC | " +
                " Max: " + district.getMainMaxTemperature().ToString() + "ºC";
            precipitation.Text = district.getRainVolume().ToString() + " mm";
            humidity.Text = district.getMainHumidity().ToString() + " %";
            pressure.Text = district.getMainPressure().ToString() + " hpa";
            wind.Text = district.getWindSpeed().ToString() + " m/s";
            //weatherIconImage.Source = district.getIcon();

            // Weather Animations
            // Real Value
            //drawingView.setConditions(district.getConditions(), district.getDescription(), (float)district.getWindSpeed(), 70);
            // Weather Tests
            // Clear Sky
            // drawingView.setConditions("Clear", district.getDescription(), (float)district.getWindSpeed(), 0);
            // Moderate Rain
            //drawingView.setConditions("Rain", district.getDescription(), (float) district.getWindSpeed(), 70);
            // Heavy Rain
            //drawingView.setConditions("Rain", district.getDescription(), (float)district.getWindSpeed(), 150);
            // Heavy Rain, Heavy Wind
            // drawingView.setConditions("Rain", district.getDescription(), 30, 150);
            // Light Thunder
            // drawingView.setConditions("Thunderstorm", "light thunderstorm", (float)district.getWindSpeed(), 70);
            // Heavy Thunder
            drawingView.setConditions("Thunderstorm", "ragged thunderstorm", (float)district.getWindSpeed(), 70);
            // Drizzle
            //drawingView.setConditions("Drizzle", district.getDescription(), (float)district.getWindSpeed(), 70);
            // Snow
            //drawingView.setConditions("Snow", district.getDescription(), (float)district.getWindSpeed(), 70);


            nextDay = nextD;
        }
    }
}