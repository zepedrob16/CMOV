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
    public partial class Master : ContentPage
    {
        CheckWeather checkWeather;
        //public Detail detail = new Detail();
        public Master(CheckWeather checkW)
        {
            InitializeComponent();
            checkWeather = checkW;

            /*buttonA.Clicked += delegate
            {
                checkWeather.getWeather(2735941);
            };

            buttonB.Clicked += delegate
            {
                checkWeather.getWeather(2267056);
            };*/

            addFavourite.Clicked += delegate
            {
                PopupNavigation.Instance.PushAsync(new PopupView(this));
            };
        }
        
        public void submitEntry(string cityName)
        {

            foreach (var obj in checkWeather.districtInfos)
            {
                if (cityName == obj.name)
                    AddEntry(cityName, obj.id);
            }
        }

        public void AddEntry(string cityName, int id)
        {
            //Find stack Layout
            StackLayout stackLayout = (StackLayout)FindByName("masterLayout");

            //Adds a favourite on the CheckWeather Class
            checkWeather.addFavourite(cityName);

            //Creates a new button for the city that is being created
            Button button = new Button()
            {
                Text = cityName
            };
            button.TextColor = Xamarin.Forms.Color.White;
            button.BackgroundColor = Xamarin.Forms.Color.FromHex("#EE6C4D");
            button.Clicked += delegate
            {
                checkWeather.getWeather(id);
            };

            //Retrieve the add favourite button
            Button newAddFavourite = (Button)FindByName("addFavourite");

            stackLayout.Children.Remove(addFavourite);
            //Adds city to the stack layout
            stackLayout.Children.Add(button);

            //Adds the favourite button again
            stackLayout.Children.Add(newAddFavourite);
        }
    }
}