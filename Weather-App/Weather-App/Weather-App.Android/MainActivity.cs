using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;

namespace Weather_App.Droid
{
    [Activity(Label = "Weather_App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_activity);
            
            View v;
            //Create A Button Object To Set The Event
            Button button = FindViewById<Button> (Resource.Id.checkWeatherButton);

            //Assign The Event To Button
            button.Click += delegate {

                //Call Your Method When User Clicks The Button
                Intent intent = new Intent(this, typeof(CheckWeather));
                StartActivity(intent);
            };
        }

    }

}