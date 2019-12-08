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
            string apiKey = "12064b9f476aaa5afe370326c61f5e1b";
            string place = "Porto";
            string url = "https://api.openweathermap.org/data/2.5/weather?q=";
            string apiRequest = url + place + "&units=metric" + "&appid=" + apiKey;

            //string apiRequest = "https://api.openweathermap.org/data/2.5/weather?q=Porto&appid=12064b9f476aaa5afe370326c61f5e1b";

            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(apiRequest);

            Console.WriteLine(result);
        }
    }
}