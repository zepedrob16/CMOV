using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextDay : ContentPage
    {
        public NextDay()
        {
            InitializeComponent();
        }

        public void setInfo(District district)
        {
            nextDayMinTemperature.Text = "Min Temp: " + district.getMainNextDayMinTemp().ToString();
            nextDayMaxTemperature.Text = "Max temp: " + district.getMainNextDayMaxTemp().ToString();
            nextDayHumidity.Text = "Humidity: " + district.getMainNextDayHumidity().ToString();
            nextDayPrecipitation.Text = "Rain: " + district.getRainNextDayVolume().ToString();
            nextDayPressure.Text = "Pressure: " + district.getMainNextDayPressure().ToString();
            nextDayWind.Text = "Wind: " + district.getWindNextDaySpeed().ToString();
        }
    }
}