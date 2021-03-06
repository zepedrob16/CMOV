﻿using SkiaSharp;
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
using System.Diagnostics;

namespace WeatherApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextDay : ContentPage
    {
        private int margin;
        private List<JToken> forecast;
        private float maxTempG;
        private float minTempG;
        private int ylen;
        
        private SKCanvas cnv;

        public NextDay()
        {
            InitializeComponent();
            canvas.PaintSurface += OnPaint;
        }

        public void setInfo(District district)
        {
            cityName.Text = district.getCityName().ToString();
            minTempG = Math.Min(float.Parse(district.getMainNextDayMinTemp().ToString()),0);
            maxTempG = Math.Max(float.Parse(district.getMainNextDayMaxTemp().ToString()),25);
            temperature.Text = "Min: " + district.getMainNextDayMinTemp().ToString() + "ºC | " +
                " Max: " + district.getMainNextDayMaxTemp().ToString() + "ºC";
            humidity.Text = district.getMainNextDayHumidity().ToString() + " %";
            precipitation.Text = district.getRainNextDayVolume().ToString() + " mm";
            pressure.Text = district.getMainNextDayPressure().ToString() + " hpa";
            wind.Text = district.getWindNextDaySpeed().ToString() + " m/s";
            forecast = district.getNextDayObjs();
            
            drawingView.setConditions(district.getConditions(), district.getDescription(), (float)district.getWindSpeed(), (float)district.getRainVolume());
        }


        // Handle animation code
        protected override void OnAppearing()
        {
            base.OnAppearing();
            drawingView.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            drawingView.OnDisappearing();
        }

        public void OnPaint(object sender, SKPaintSurfaceEventArgs args)
        {
            int wd = args.Info.Width;
            int hg = args.Info.Height;
            cnv = args.Surface.Canvas;

            cnv.Clear();
            
            DrawAxis(wd, hg);
            DrawGraph(wd, hg);
        }

        void DrawAxis(int wd, int hg)
        {
            SKPaint coorPaint = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Coral,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square,
                TextSize = Math.Min(2 * margin, 50f)
            };

            margin = Math.Min(hg / 4, wd / 4);

            ylen = hg - 2 * margin;   

            cnv.DrawLine(margin, hg - ylen - margin, margin, hg - margin, coorPaint);  // draw the Y axis

            float factor = ((0 - minTempG) / (maxTempG - minTempG));

            cnv.DrawLine(margin, margin + ylen - factor*ylen, wd - margin, margin + ylen - factor * ylen, coorPaint); // draw the X axis
        }

        void DrawGraph(int wd, int hg)
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
                TextSize = margin/5
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
                currY = margin + ylen - (((currTemp - minTempG) / (maxTempG - minTempG)) * ylen);

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
