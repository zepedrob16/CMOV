using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;

namespace WeatherApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextDay : ContentPage
    {
        private int margin;
        private List<JToken> forecast;
        private float maxTemp;
        private float minTemp;
        private int ylen;
        public NextDay()
        {
            InitializeComponent();
            canvas.PaintSurface += OnPaint;
        }

        public void setInfo(District district)
        {
            minTemp = Math.Min(float.Parse(district.getMainNextDayMinTemp().ToString()),0);
            maxTemp = Math.Max(float.Parse(district.getMainNextDayMaxTemp().ToString()),25);
            nextDayMinTemp.Text = "Min Temp: " + district.getMainNextDayMinTemp().ToString();
            nextDayMaxTemp.Text = "Max temp: " + district.getMainNextDayMaxTemp().ToString();
            nextDayHumidity.Text = "Humidity: " + district.getMainNextDayHumidity().ToString();
            nextDayPrecipitation.Text = "Rain: " + district.getRainNextDayVolume().ToString();
            nextDayPressure.Text = "Pressure: " + district.getMainNextDayPressure().ToString();
            nextDayWind.Text = "Wind: " + district.getWindNextDaySpeed().ToString();
            forecast = district.getNextDayObjs();
        }

        public void OnPaint(object sender, SKPaintSurfaceEventArgs args)
        {
            int wd = args.Info.Width;
            int hg = args.Info.Height;
            SKCanvas cnv = args.Surface.Canvas;

            cnv.Clear();
            
            DrawAxis(cnv, wd, hg);
            DrawGraph(cnv, wd, hg);
        }

        void DrawAxis(SKCanvas cnv, int wd, int hg)
        {
            SKPaint coorPaint = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Coral,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square,
                TextSize = 2 * margin
            };

            margin = Math.Min(hg / 4, wd / 4);

            ylen = hg - 2 * margin;   

            cnv.DrawLine(margin, hg - ylen - margin, margin, hg - margin, coorPaint);  // draw the Y axis

            float factor = ((0 - minTemp) / (maxTemp - minTemp));

            cnv.DrawLine(margin, margin + ylen - factor*ylen, wd - margin, margin + ylen - factor * ylen, coorPaint); // draw the X axis
        }

        void DrawGraph(SKCanvas cnv, int wd, int hg)
        {
            SKPaint gPaint = new SKPaint
            {        // paint for the graphic
                Style = SKPaintStyle.Stroke,
                Color = SKColors.LightGray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Butt

            };

            SKPaint tPain_t = new SKPaint
            {        // paint for the graphic
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.LightGray,
                StrokeWidth = 1,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = margin/2
            };

            SKPath path = new SKPath();

            float offsetX = wd / 10; // Empty space for the graph takes one entrance of the count
            
            float graphW = wd - 2 * margin - 2* offsetX;
            
            float inc = graphW / (forecast.Count - 1);

            float currTemp, currX, currY;
            string currHour;
            
            for (int k = 0; k < forecast.Count; k++)
            {
                currTemp = float.Parse(forecast[k]["main"]["temp_max"].ToString());
                currHour = forecast[k]["dt_txt"].ToString().Split(null)[1].Substring(0, 5);
                currX = margin + offsetX + k * inc;
                currY = margin + ylen - (((currTemp - minTemp) / (maxTemp - minTemp)) * ylen);

                if (k == 0)
                    path.MoveTo(currX, currY);
                else
                    path.LineTo(currX, currY);

                cnv.DrawCircle(currX, currY, margin/10, gPaint);
                cnv.DrawText(currTemp.ToString() + "ºC", currX, margin, tPain_t);
                cnv.DrawText(currHour, currX, hg, tPain_t);
            }

            cnv.DrawPath(path, gPaint);
        }
    }
}
