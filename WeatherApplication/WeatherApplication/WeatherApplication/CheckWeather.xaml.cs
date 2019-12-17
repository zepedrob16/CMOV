//using Android.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
        JObject jObject = new JObject();
        DetailPage detailPage;
        Master master;
        public List<DistrictInfo> districtInfos;
       
        public CheckWeather()
        {
            InitializeComponent();

            getWeather(2735941);
            this.detailPage = new DetailPage();
            this.Master = new Master(this);
            master = new Master(this);
            this.Detail = new NavigationPage(detailPage);
            App.MasterDetail = this;
            districtInfos = LoadJson();
        }

        public async void getWeather(int id)
        {
            //Api Request
            string apiKey = "12064b9f476aaa5afe370326c61f5e1b";
            string url = "https://api.openweathermap.org/data/2.5/weather?id=";
            string apiRequest = url + id.ToString() + "&units=metric" + "&appid=" + apiKey;

            //Sending the request and saving the result
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(apiRequest);

            //Creating a district with the information received
            District district = new District();
            district.jsonToWeather(result);


            //Creates the page with the forecast for the next day
            string nextDayUrl = "https://api.openweathermap.org/data/2.5/forecast?id=";
            string nextDayRequest = nextDayUrl + id.ToString() + "&units=metric" + "&appid=" + apiKey;

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

        private List<DistrictInfo> LoadJson()
        {
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(CheckWeather)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("WeatherApplication.Portugal.json");

            using (var reader = new StreamReader(stream))
            {
                string jsonStr = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<DistrictInfo>>(jsonStr);
            }
        }

        public List<string> getFavourites() { return favourites; }
    }
}