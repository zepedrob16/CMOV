using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApplication
{
    class WeatherCloud
    {
        public float posX, posY, size, radius, currPos, speed;
        int wd, hg;
        private SKPath path;
        Random rnd;
        String conditions;
        SKPaint paint, thunderP;

        public WeatherCloud(int wd, int hg, float windSpeed, int i, int numClouds, String conditions, int distance, SKPaint paint, SKPaint thunderPaint)
        {
            thunderP = thunderPaint;
            this.conditions = conditions;
            this.paint = paint;
            rnd = new Random();
            this.wd = wd;
            this.hg = hg;
            
            posX = wd / 4 + i * (wd / 2) / numClouds;
            posY = hg / 8 + (rnd.Next(0, hg / 8)) + (hg/16) * distance;
            size = rnd.Next(hg/9, hg/7);
            radius = (rnd.Next(hg/20, hg/8))/ 50f;
            speed = (rnd.Next(2, 20) * (windSpeed/3)) / 80f;
            path = new SKPath();

            size -=  distance*2;
            radius /= (Math.Max(1,distance*2));
            speed /= (Math.Max(1, distance*2));
        }

        public void UpdateAndDraw(SKCanvas cnv, bool isThundering)
        {
            Update();
            
            if (isThundering)
                cnv.DrawCircle(posX, posY, size, thunderP);
            else
                cnv.DrawCircle(posX, posY, size, paint);

        }

        private void Update()
        {
            currPos += speed / (Math.Min(radius,1) * 2);
            posX = posX + (float)Math.Cos(currPos) * radius;
            posY = posY + (float)Math.Sin(currPos) * radius;
        }

        public void TriggerThunder(SKCanvas cnv)
        {
            path.Reset();
            int x1 = rnd.Next(wd / 4, 3 * wd / 4);
            path.MoveTo(x1, hg / 5);

            int x2 = Math.Min(wd, Math.Max(0, rnd.Next(x1 - wd/8, x1 + wd/8)));
            path.LineTo(x2, hg / 2);

            int x3 = Math.Min(wd, Math.Max(0, rnd.Next(x2 - wd / 8, x1 + wd / 8)));
            path.LineTo(x3, rnd.Next((int) (hg / 1.35f), hg));

            cnv.DrawPath(path, thunderP);            
        }
    }
}
