using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApplication
{
    class WeatherCloud
    {
        public float posX, posY, size, radius, currPos, speed;

        public WeatherCloud(int wd, int hg, float windSpeed, int i, int numClouds)
        {
            Random rnd = new Random();

            posX = wd / 4 + i * (wd / 2) / numClouds;
            posY = hg / 8 + (rnd.Next(0, hg / 6));
            size = rnd.Next(hg / 9, hg / 7);
            radius = rnd.Next(hg/12, hg/8) / 500f;
            speed = (rnd.Next(10, 30) / 30f * Math.Max(windSpeed, 0.1f))/1000f;

        }

        public void Update()
        {
            currPos += speed;
            posX = posX + (float)Math.Cos(currPos) * radius;
            posY = posY + (float)Math.Sin(currPos) * radius;
        }
    }
}
