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

        public void setIntro(District district)
        {
            intro.Text = district.getMainNextDayMinTemp().ToString();
        }
    }
}