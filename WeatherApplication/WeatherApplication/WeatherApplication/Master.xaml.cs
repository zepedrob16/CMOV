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

            buttonA.Clicked += delegate
            {
                checkWeather.getWeather(buttonA.Text);
            };

            buttonB.Clicked += delegate
            {
                checkWeather.getWeather(buttonB.Text);
            };

            addFavourite.Clicked += delegate
            {
                PopupNavigation.Instance.PushAsync(new PopupView(this));
            };
        }
        
        public void submitEntry(string cityName)
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
            button.Clicked += delegate
            {
                checkWeather.getWeather(button.Text);
            };

            //Retrieve the add favourite button
            Button newAddFavourite = (Button) FindByName("addFavourite");
   
            stackLayout.Children.Remove(addFavourite);
            //Adds city to the stack layout
            stackLayout.Children.Add(button);

            //Adds the favourite button again
            stackLayout.Children.Add(newAddFavourite);
        }
    }
}