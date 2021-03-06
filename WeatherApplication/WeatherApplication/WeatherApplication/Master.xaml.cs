﻿using Rg.Plugins.Popup.Services;
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
        public bool loading = true;
        public List<Button> allCities = new List<Button>();
        public List<string> allNames = new List<string>();

        public Master(CheckWeather checkW)
        {
            InitializeComponent();
            checkWeather = checkW;

            addFavourite.Clicked += delegate
            {
                PopupNavigation.Instance.PushAsync(new PopupView(this, true));
            };
            removeFavourite.Clicked += delegate
            {
                PopupNavigation.Instance.PushAsync(new PopupView(this, false));
            };

            List<KeyValuePair<int, string>> favourites = checkW.getFavourites();

            for(int i = 0; i < favourites.Count; i++)
            {
                AddEntry(favourites[i].Value, favourites[i].Key);
                allNames.Add(favourites[i].Value);
            }
            loading = false;
        }
        
        public void submitEntry(string cityName)
        {
            foreach (var city in allNames)
            {
                if (cityName == city)
                    return;
            }

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
            if (!loading)
                checkWeather.addFavourite(cityName, id);

            //Creates a new button for the city that is being created
            Button button = new Button()
            {
                Text = cityName
            };
            button.TextColor = Xamarin.Forms.Color.White;
            button.BackgroundColor = Xamarin.Forms.Color.FromHex("#EE6C4D");
            button.Clicked += delegate
            {
                App.MasterDetail.IsPresented = false;
                checkWeather.getWeather(id);
            };

            //Retrieve the add favourite button
            Button newAddFavourite = (Button)FindByName("addFavourite");

            stackLayout.Children.Remove(addFavourite);
            //Adds city to the stack layout
            stackLayout.Children.Add(button);

            //Adds the favourite button again
            stackLayout.Children.Add(newAddFavourite);

            Button newRemoveFavourite = (Button)FindByName("removeFavourite");
            stackLayout.Children.Remove(removeFavourite);
            stackLayout.Children.Add(newRemoveFavourite);

            allCities.Add(button);
            allNames.Add(cityName);
        }

        public void removeFavouriteCity(string name)
        {
            StackLayout stackLayout = (StackLayout)FindByName("masterLayout");

            foreach (var city in allNames)
            {
                if (city == name)
                {
                    allNames.Remove(city);
                    break;
                }
            }

            foreach (var obj in allCities)
            {
                if (obj.Text == name)
                {
                    stackLayout.Children.Remove(obj);
                }
            }
        }
    }
}