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

        // Ripple
        public bool isDisappearing;
        public float timeToSpawn;
        public float rippleAnimationTime;

        public WeatherDroplet(int wd, int hg, float windSpeed)
        {
            this.wd = wd;
            this.hg = hg;
            this.windSpeed = windSpeed/2.0f;
            hasReset = false;
            
            path = new SKPath();

            Reset();
                        
            isDisappearing = false;
            rippleAnimationTime = 0.5f;
        }

        private void Reset()
        {
            Random rnd = new Random();
            posX = rnd.Next(wd / 5, 3 * wd / 4);
            posY = rnd.Next(hg / 4, hg / 4);
            timeToSpawn = (rnd.Next(0, 100)) / 100.0f;

            int lowerY = (int)(hg / 1.3f);
            maxY = rnd.Next(lowerY, hg);
            proximity = (maxY - lowerY) / (hg - lowerY);
            // The further away, the slower it will appear
            speedY = (hg / 60) + 5 * proximity;
            speedX = (rnd.Next(10, 20) / 70.0f) * windSpeed * proximity;
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
                rippleAnimationTime -= 1 / 60f;
                if (rippleAnimationTime <= 0)
                {
                    isDisappearing = false;
                    rippleAnimationTime = 0.5f;
                    
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

            if (proximity < 0.2f)
                paint.Color = SKColors.DarkCyan;
            else if (proximity < 0.5f)
                paint.Color = SKColors.Cyan;
            else 
                paint.Color = SKColors.LightCyan;
            
            // Don't draw when resetting positions
            if (hasReset)
                hasReset = false;
            else
                cnv.DrawPath(path, paint);

        }
    }
}
