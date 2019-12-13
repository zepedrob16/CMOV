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

            //Download Image
           // string icon = district.getIcon();

           /* string imageURL = "http://openweathermap.org/img/w/" + icon + ".png";
            WebRequest request = default(WebRequest);
            request = WebRequest.Create(imageURL);
            request.Timeout = 30;
            request.Method = "GET";

            WebResponse response = default(WebResponse);
            response = await request.GetResponseAsync();
            MemoryStream ms = new MemoryStream();
            response.GetResponseStream().CopyTo(ms);
            byte[] imageData = ms.ToArray();

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);*/

            detailPage.setWeather(district);

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