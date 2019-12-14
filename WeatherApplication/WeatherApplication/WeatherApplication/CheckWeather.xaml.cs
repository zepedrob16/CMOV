using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public List<string> favourites = new List<string>();
        DetailPage detailPage;
        Master master;
        public CheckWeather()
        {
            InitializeComponent();

            getWeather("Porto");
            this.detailPage = new DetailPage();
            this.Master = new Master(this);
            master = new Master(this);
            this.Detail = new NavigationPage(detailPage);
            App.MasterDetail = this;
        }

        public async void getWeather(string place)
        {
            //Api Request
            string apiKey = "12064b9f476aaa5afe370326c61f5e1b";
            string url = "https://api.openweathermap.org/data/2.5/weather?q=";
            string apiRequest = url + place + "&units=metric" + "&appid=" + apiKey;

            //Sending the request and saving the result
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(apiRequest);

            //Creating a district with the information received
            District district = new District();
            district.jsonToWeather(result);


            //Creates the page with the forecast for the next day
            string nextDayUrl = "https://api.openweathermap.org/data/2.5/forecast?q=";
            string nextDayRequest = nextDayUrl + place + "&units=metric" + "&appid=" + apiKey;

            var nextDayHandler = new HttpClientHandler();
            HttpClient nextDayClient = new HttpClient(nextDayHandler);
            string nextDayResult = await nextDayClient.GetStringAsync(nextDayRequest);

            district.nextDayJsonToWeather(nextDayResult);

            NextDay nextDay = new NextDay();
            nextDay.setInfo(district);

            detailPage.setWeather(district, nextDay);

            Console.WriteLine(result);
        }

        public void addFavourite(string city)
        {
            favourites.Add(city);
           // master.updateFavourites()
        }

        public List<string> getFavourites() { return favourites; }
    }
}