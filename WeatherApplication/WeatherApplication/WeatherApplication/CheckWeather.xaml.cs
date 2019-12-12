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
    public partial class CheckWeather : MasterDetailPage
    {
        public CheckWeather()
        {
            InitializeComponent();

            //getWeather("Porto");

            this.Master = new Master();
            this.Detail = new NavigationPage(new Detail());
        }

        private async void getWeather(string v)
        {
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

            //temperature.Text = district.getMainTemperature().ToString();
           /* minTemperature.Text = district.getMainMinTemperature().ToString() + " | ";
            maxTemperature.Text = district.getMainMaxTemperature().ToString();
            precipitation.Text = "Precipitation: " + district.getAllClouds().ToString() + "%";
            humidity.Text = "Humidity: " + district.getMainHumidity().ToString();
            pressure.Text = "Pressure: " + district.getMainPressure().ToString();
            wind.Text = "Wind: " ff+ district.getWindSpeed().ToString();*/

            Console.WriteLine(result);
        }
    }
}