using Rg.Plugins.Popup.Services;
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
    public partial class PopupView
    {
        public PopupView(Master master)
        {
            InitializeComponent();

            citySubmit.Completed += (s, e) =>
            {
                master.submitEntry(citySubmit.Text);
                PopupNavigation.Instance.PopAsync(true);
            };
        }
    }
}