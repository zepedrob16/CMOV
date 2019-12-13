using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WeatherApplication
{
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        //public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double next_day_temp_min { get; set; }
        public double next_day_temp_max { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class RootObject
    {
        //public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class District
    {

        Main main = new Main();
        Wind wind = new Wind();
        Clouds clouds = new Clouds();
        Weather weather = new Weather();

        //Stores information obtained from the json
        public void jsonToWeather(string jsonResult)
        {
            JObject json = JObject.Parse(jsonResult);

            main.temp = Double.Parse(json["main"]["temp"].ToString());
            main.temp_min = Double.Parse(json["main"]["temp_min"].ToString());
            main.temp_max = Double.Parse(json["main"]["temp_max"].ToString());
            main.pressure = Int32.Parse(json["main"]["pressure"].ToString());
            main.humidity = Int32.Parse(json["main"]["humidity"].ToString());
            wind.speed = Double.Parse(json["wind"]["speed"].ToString());
            clouds.all = Int32.Parse(json["clouds"]["all"].ToString());

            //This contains the weather icon url
            weather.icon = "http://openweathermap.org/img/w/" + json["weather"][0]["icon"].ToString() + ".png";
        }

        public void nextDayJsonToWeather(string jsonResult)
        {
            main.next_day_temp_min = 30.12f;
            string myDate = DateTime.Now.ToString();
        }

        //Gets
        public double getMainTemperature() { return main.temp; }
        public double getMainMinTemperature() { return main.temp_min; }
        public double getMainMaxTemperature() { return main.temp_max; }
        public int getMainPressure() { return main.pressure; }
        public int getMainHumidity() { return main.humidity; }
        public double getWindSpeed() { return wind.speed; }
        public int getAllClouds() { return clouds.all; }
        public string getIcon() { return weather.icon; }
        public double getMainNextDayMinTemp() { return main.next_day_temp_min; }

    }
}
