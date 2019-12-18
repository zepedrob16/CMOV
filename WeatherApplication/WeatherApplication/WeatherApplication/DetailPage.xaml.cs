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
            //drawingView.setConditions(district.getConditions(), (float)district.getWindSpeed(), 70);
            // Weather Tests
            // Clear Sky
            //drawingView.setConditions("Clear", (float)district.getWindSpeed(), 0);
            // Moderate Rain
            //drawingView.setConditions("Rain", (float) district.getWindSpeed(), 70);
            // Heavy Rain
            drawingView.setConditions("Rain", (float)district.getWindSpeed(), 150);


            nextDay = nextD;
        }
    }
}