using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApplication
{
    class WeatherDroplet
    {
        public float posX, posY, speedX, speedY, windSpeed, proximity, maxY;
        private int hg, wd;
        private SKPath path;
        private bool hasReset;

        private String type;

        private SKPaint paint, thunderP;

        // Ripple
        public bool isDisappearing;
        public float timeToSpawn, timeToRespawn;     

        public WeatherDroplet(int wd, int hg, float windSpeed, String type, SKPaint rainFarP, SKPaint rainMediumP, SKPaint rainCloseP, SKPaint thunderP)
        {
            this.type = type;
            this.wd = wd;
            this.hg = hg;
            this.thunderP = thunderP;
            this.windSpeed = windSpeed/2.0f;
            hasReset = false;
            
            path = new SKPath();
                        
            isDisappearing = false;
            timeToRespawn = 0.5f;

            Random rnd = new Random();
            int lowerY = (int)(hg / 1.3f);
            maxY = rnd.Next(lowerY, hg);

            proximity = (maxY - lowerY) / (hg - lowerY);

            Reset();

        // Parallax colors
            if (proximity < 0.2f)
                paint = rainFarP;
            else if (proximity < 0.5f)
                paint = rainMediumP;
            else
                paint = rainCloseP;
             
        }


        private void Reset()
        {
            Random rnd = new Random();
            posX = rnd.Next(wd / 4, 3 * wd / 4);
            posY = rnd.Next(hg / 5, hg / 4);
            
            timeToSpawn = (rnd.Next(0, 200)) / 100.0f;
            if (type == "Drizzle")
                timeToSpawn = (rnd.Next(0, 800)) / 100.0f;
            if (type == "Snow")
                timeToSpawn = (rnd.Next(0, 1600)) / 100.0f;

            // The further away, the slower it will appear
            speedY = (hg / 60) + 5 * proximity;

            speedX = (rnd.Next(10, 20) / 70.0f) * windSpeed * (proximity/2.0f);

            if (type == "Snow" || type == "Drizzle")
            {
                speedY /= 10f;
                speedX /= 10f;
            }

            timeToRespawn = 0.5f;
            if (type == "Snow")
                timeToRespawn = 10f;
        }

        public void Update()
        {   
            if (timeToSpawn >= 0)
            {
                timeToSpawn -= 1 / 60f;
                return;
            }
                
            if (!isDisappearing)
            {
                posX += speedX;
                posY += speedY;
                if (posY > maxY)
                    isDisappearing = true;
            } else
            {
                timeToRespawn -= 1 / 60f;
                if (timeToRespawn <= 0)
                {
                    isDisappearing = false;
                    
                    Reset();
                    hasReset = true;
                }
            }
        }

        public void UpdateAndDraw(SKCanvas cnv, bool hasThundered)
        {
            path.Reset();
            path.MoveTo(posX, posY);
            Update();
            path.LineTo(posX, posY);
                        
            if (hasReset)
                hasReset = false;
            else if (timeToSpawn < 0)
            {
                if (hasThundered)
                    cnv.DrawPath(path, thunderP);
                else
                    cnv.DrawPath(path, paint);
            }
        }
    }
}
