using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawingWeatherView : ContentView
    {
        int wd, hg;
        private Stopwatch stopwatch;
        bool pageIsActive;
        public float globalScale = 1;
        private SKCanvas cnv;
        private String conditions = "Clear";

        private float sunRotation;
       
        private SKPaint sunP, cloudLightP, cloudGreyP, cloudDarkP, rainP, thunderP;
        public DrawingWeatherView()
        {
            InitializeComponent();
            CreatePaints();
            sunRotation = 0;
            stopwatch = new Stopwatch();
        }

        private void CreatePaints()
        {
            sunP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Orange,
                StrokeWidth = 10,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            cloudLightP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.LightGray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            cloudGreyP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.Gray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            cloudDarkP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.DarkGray,
                IsAntialias = true,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };

            rainP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };
        }

        public void OnAppearing()
        {
            pageIsActive = true;
            AnimationLoop();
        }

        public void OnDisappearing()
        {
            pageIsActive = false;
        }

        async Task AnimationLoop()
        {
            stopwatch.Start();

            while (pageIsActive)
            {
                canvasView.InvalidateSurface();
               
                // Do animation here
                globalScale += 1 / 60f;
               
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 60));
            }

            stopwatch.Stop();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            wd = args.Info.Width;
            hg = args.Info.Height;
            cnv = args.Surface.Canvas;

            cnv.Clear();

            if (conditions == "Clear")
                DrawSun();
            else if (conditions == "Clouds")
                DrawClouds();
            else if (conditions == "Rain")
                DrawRain();
        }

        private void DrawSun()
        {
            sunRotation += 1 / 60f;
            float sunRadius = Math.Min(wd / 5, hg / 5);
            cnv.DrawCircle(wd/2, hg/2, sunRadius, sunP);
            
            float bladeRadius = Math.Min(wd / 5, hg / 5);
            double currPos = sunRotation;
            int blades = 8;
            float startX = wd/2, startY = hg/2, endX = 0, endY = 0;
            for (int i = 0; i < blades; i++)
            {
                startX = wd / 2 + sunRadius * (float) Math.Cos(currPos);
                endX = startX + bladeRadius * (float) Math.Cos(currPos);

                startY = hg / 2 + sunRadius * (float) Math.Sin(currPos);
                endY = startY + bladeRadius * (float)Math.Sin(currPos);
                
                currPos += Math.PI * 2 / blades;

                cnv.DrawLine(startX, startY, endX, endY, sunP);
            }
        }
        private void DrawClouds()
        {
            cnv.DrawCircle(0, 0, 100, cloudGreyP);
        }
        private void DrawRain()
        {
            cnv.DrawCircle(0, 0, 100, rainP);
        }
    }
}