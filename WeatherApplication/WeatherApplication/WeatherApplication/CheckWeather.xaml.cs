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
        DetailPage detailPage;
        public CheckWeather()
        {
            InitializeComponent();

            getWeather("Porto");
            this.detailPage = new DetailPage();
            this.Master = new Master(this);
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

            detailPage.setWeather(district);

            Console.WriteLine(result);
        }
    }
}