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

        private float sunRotation = 0;
       
        private SKPaint sunP, cloudLightP, cloudGreyP, cloudDarkP, rainP, thunderP;
        public DrawingWeatherView()
        {
            InitializeComponent();
            CreatePaints();
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
                canvas.InvalidateSurface();

                // Do animation here
                globalScale += 1 / 60f;
                if (conditions == "Clear")
                    DrawSun();
                else if (conditions == "Clouds")
                    DrawClouds();
                else if (conditions == "Rain")
                    DrawRain();

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
            cnv.DrawCircle(wd/2, hg/2, Math.Min(wd/2,hg/2), sunP);
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