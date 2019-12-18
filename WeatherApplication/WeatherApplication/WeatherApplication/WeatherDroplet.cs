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

        // Ripple
        public bool isDisappearing;
        public float timeToSpawn, timeToRespawn;     

        public WeatherDroplet(int wd, int hg, float windSpeed, String type)
        {
            this.type = type;
            this.wd = wd;
            this.hg = hg;
            this.windSpeed = windSpeed/2.0f;
            hasReset = false;
            
            path = new SKPath();

            Reset();
                        
            isDisappearing = false;
            timeToRespawn = 0.5f;
            
        }

        private void Reset()
        {
            Random rnd = new Random();
            posX = rnd.Next(wd / 5, 3 * wd / 4);
            posY = rnd.Next(hg / 5, hg / 4);
            
            timeToSpawn = (rnd.Next(0, 200)) / 100.0f;
            if (type == "Drizzle")
                timeToSpawn = (rnd.Next(0, 800)) / 100.0f;
            if (type == "Snow")
                timeToSpawn = (rnd.Next(0, 1600)) / 100.0f;

            int lowerY = (int)(hg / 1.3f);
            maxY = rnd.Next(lowerY, hg);
            proximity = (maxY - lowerY) / (hg - lowerY);
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

        public void UpdateAndDraw(SKPaint paint, SKCanvas cnv)
        {
            
            path.Reset();
            path.MoveTo(posX, posY);
            Update();
            path.LineTo(posX, posY);

            // If it is not thundering
            if (paint.Color != SKColors.White)
            {
                if (type == "Snow")
                {
                    if (proximity < 0.2f)
                        paint.Color = SKColors.Gray;
                    else if (proximity < 0.5f)
                        paint.Color = SKColors.LightGray;
                    else
                        paint.Color = SKColors.White;
                } else
                { 
                    // Parallax colors
                    if (proximity < 0.2f)
                        paint.Color = SKColors.DarkCyan;
                    else if (proximity < 0.5f)
                        paint.Color = SKColors.Cyan;
                    else
                        paint.Color = SKColors.LightCyan;
                }
            }

            // Don't draw when resetting positions
            if (hasReset)
                hasReset = false;
            else
                cnv.DrawPath(path, paint);

        }
    }
}
