using System;
using System.Collections.Generic;
using System.Linq;
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
        public int next_day_pressure { get; set; }
        public int humidity { get; set; }
        public int next_day_humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double next_day_temp_min { get; set; }
        public double next_day_temp_max { get; set; }
        public string next_day_weather { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double next_day_speed { get; set; }
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

    public class Rain
    {
        public double volume { get; set; }
        public double next_day_volume { get; set; }
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
        string city = "";
        Main main = new Main();
        Wind wind = new Wind();
        Clouds clouds = new Clouds();
        Weather weather = new Weather();
        Rain rain = new Rain();
        List<JToken> nextDayObjs = new List<JToken>();

        //Stores information obtained from the json
        public void jsonToWeather(string jsonResult)
        {
            JObject json = JObject.Parse(jsonResult);

            city = json["name"].ToString();
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
            var today = DateTime.Now;
            string nextDay = today.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss").Substring(0,10);

            getListOfObjects(nextDay, jsonResult);            
        }

        private void getListOfObjects(string nextDay, string jsonResult)
        {
            //List<JObject> allObjects = new List<JObject>();
            JObject json = JObject.Parse(jsonResult);
            double maximumTemp = -60.0;
            double minimumTemp = 80.0;
            double maxRain = 0.0;
            int maxHumidity = 0;
            double maxWind = 0;
            int maxPressure = 0;

            List<string> weatherList = new List<string>();
            

            foreach (var obj in json["list"])
            {
                string dayToCompare = obj["dt_txt"].ToString().Substring(0, 10);

                if (dayToCompare == nextDay)
                {
                    nextDayObjs.Add(obj);
                    double maxTempToCompare = Double.Parse(obj["main"]["temp_max"].ToString());
                    double minTempToCompare = Double.Parse(obj["main"]["temp_min"].ToString());

                    //Min and max temperatures
                    if (maxTempToCompare > maximumTemp)
                        maximumTemp = maxTempToCompare;
                    if (minTempToCompare < minimumTemp)
                        minimumTemp = minTempToCompare;

                    //Rain volume
                    if (obj["rain"] != null)
                    {
                        double rainToCompare = Double.Parse(obj["rain"]["3h"].ToString());

                        if (rainToCompare > maxRain)
                            maxRain = rainToCompare;
                    }

                    //Max humidity
                    int humidityToCompare = Int32.Parse(obj["main"]["humidity"].ToString());
                    if (humidityToCompare > maxHumidity)
                        maxHumidity = humidityToCompare;

                    //Max Wind Speed
                    double windToCompare = Double.Parse(obj["wind"]["speed"].ToString());
                    if (windToCompare > maxWind)
                        maxWind = windToCompare;

                    //Max Pressure
                    int pressureToCompare = Int32.Parse(obj["main"]["pressure"].ToString());
                    if (pressureToCompare > maxPressure)
                        maxPressure = pressureToCompare;

                    //Main weather
                    weatherList.Add(obj["weather"][0]["main"].ToString());
                    
                }  
            };

            main.next_day_temp_max = maximumTemp;
            main.next_day_temp_min = minimumTemp;
            main.next_day_humidity = maxHumidity;
            rain.next_day_volume = maxRain;
            wind.next_day_speed = maxWind;
            main.next_day_pressure = maxPressure;
            main.next_day_weather = weatherList.GroupBy(x => x)
                          .OrderByDescending(x => x.Count())
                          .First().Key;
        }

        //Gets

        public string getCityName() { return city;  }
        public double getMainTemperature() { return main.temp; }
        public double getMainMinTemperature() { return main.temp_min; }
        public double getMainMaxTemperature() { return main.temp_max; }
        public int getMainPressure() { return main.pressure; }
        public int getMainHumidity() { return main.humidity; }
        public double getWindSpeed() { return wind.speed; }
        public int getAllClouds() { return clouds.all; }
        public string getIcon() { return weather.icon; }
        public double getMainNextDayMinTemp() { return main.next_day_temp_min; }
        public double getMainNextDayMaxTemp() { return main.next_day_temp_max; }
        public int getMainNextDayHumidity() { return main.next_day_humidity; }
        public double getRainNextDayVolume() { return rain.next_day_volume; }
        public double getWindNextDaySpeed() { return wind.next_day_speed; }
        public double getMainNextDayPressure() { return main.next_day_pressure; }
        public List<JToken> getNextDayObjs() { return nextDayObjs; }
        public string getMainNextDayWeather() { return main.next_day_weather; }
    }

    public class DistrictInfo
    {
        public string name;
        public int id;

    }
}
