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
            this.thunderP = thunderPaint;
            this.conditions = conditions;
            this.paint = paint;
            rnd = new Random();
            this.wd = wd;
            this.hg = hg;
            
            posX = wd / 4 + i * (wd / 2) / numClouds;
            posY = hg / 8 + (rnd.Next(0, hg / 8)) + (hg/16) * distance;
            size = rnd.Next(hg/9, hg/6);
            radius = (rnd.Next(hg/12, hg/7) * (windSpeed / 10f))/ 20f;
            speed = (rnd.Next(10, 30) * windSpeed/5) / 30f;
            path = new SKPath();

            size /= (Math.Max(1, distance));
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
            currPos += speed / radius;
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
