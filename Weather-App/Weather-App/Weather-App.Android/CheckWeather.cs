using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Weather_App.Droid
{
    [Activity(Label = "CheckWeather")]
    public class CheckWeather : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.check_weather);

            getWeather();

            // Create your application here
        }

        async private void getWeather()
        {
            //TextViews
            TextView temperatureText = (TextView)FindViewById<TextView>(Resource.Id.temperature);
            TextView minTemperatureText = (TextView)FindViewById<TextView>(Resource.Id.minTemperature);
            TextView maxTemperatureText = (TextView)FindViewById<TextView>(Resource.Id.maxTemperature);
            TextView precipitationText = (TextView)FindViewById<TextView>(Resource.Id.precipitation);
            TextView humidityText = (TextView)FindViewById<TextView>(Resource.Id.humidity);
            TextView pressureText = (TextView)FindViewById<TextView>(Resource.Id.pressure);
            TextView windText = (TextView)FindViewById<TextView>(Resource.Id.wind);


            //Api Request
            string apiKey = "12064b9f476aaa5afe370326c61f5e1b";
            string place = "Porto";
            string url = "https://api.openweathermap.org/data/2.5/weather?q=";
            string apiRequest = url + place + "&units=metric" + "&appid=" + apiKey;

            //Sending the request and saving the result
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(apiRequest);

            //Creating a district with the information received
            District district = new District();
            district.jsonToWeather(result);

            temperatureText.Text = district.getMainTemperature().ToString();
            minTemperatureText.Text = district.getMainMinTemperature().ToString() + " | ";
            maxTemperatureText.Text = district.getMainMaxTemperature().ToString();
            precipitationText.Text = "Precipitation: " + district.getAllClouds().ToString();
            humidityText.Text = "Humidity: " + district.getMainHumidity().ToString();
            pressureText.Text = "Pressure: " + district.getMainPressure().ToString();
            windText.Text = "Wind: " + district.getWindSpeed().ToString();

            Console.WriteLine(result);
        }
    }
}