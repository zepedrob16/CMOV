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
        private String conditions = "";
        private String description = "";
       
        // Animation Parameters
        // Sun
        private float sunRotation;

        // Rain
        private float rainIntensity = 1;
       
        private List<WeatherDroplet> rain = new List<WeatherDroplet>();

        // Clouds
        private float windSpeed; 
        
        private List<WeatherCloud> clouds = new List<WeatherCloud>();
       
        private SKPaint sunP, cloudLightP, cloudGreyP, cloudDarkP, rainP, thunderP;
        public DrawingWeatherView()
        {
            InitializeComponent();
            CreatePaints();
            sunRotation = 0;
            stopwatch = new Stopwatch();
        }

        public void setConditions(String conds, String desc, float wind, float rain)
        {
            conditions = conds;
            description = desc;
            windSpeed = wind;
            rainIntensity = rain;
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
                Color = SKColors.LightCyan,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };

            thunderP = new SKPaint
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

            if (conditions == "")
                return;
            
            if (conditions == "Rain" || conditions == "Thunderstorm" || conditions == "Drizzle")
                DrawRain();

            if (conditions == "Clouds" || conditions == "Thunderstorm" || conditions == "Drizzle" || conditions == "Rain" || conditions == "Snow")
                DrawClouds();
            else 
                DrawSun();           
        }

        private void DrawSun()
        {
            sunRotation += 1 / 60f;
            float sunRadius = Math.Min(wd / 5, hg / 5);
            cnv.DrawCircle(wd/2, hg/2, sunRadius * 0.8f, sunP);
            
            float bladeRadius = Math.Min(wd / 5, hg / 5);
            double currPos = sunRotation;
            int blades = 8;
            float startX, startY , endX, endY;
            for (int i = 0; i < blades; i++)
            {
                startX = wd / 1.5f + sunRadius * (float) Math.Cos(currPos);
                endX = startX + bladeRadius * (float) Math.Cos(currPos);

                startY = hg / 1.5f + sunRadius * (float) Math.Sin(currPos);
                endY = startY + bladeRadius * (float)Math.Sin(currPos);
                
                currPos += Math.PI * 2 / blades;

                cnv.DrawLine(startX, startY, endX, endY, sunP);
            }
        }
        
        private void CreateClouds()
        {
            int numClouds = 20;
            
            for (int i = 0; i < numClouds; i++)
            {
                clouds.Add(new WeatherCloud(wd, hg, windSpeed, i , numClouds));
            }
        }

 
        private void DrawClouds()
        {
            if (clouds.Count == 0)
                CreateClouds();

            SKPaint paint = cloudGreyP;

            if (conditions == "Thunderstorm")
                paint = cloudDarkP;
            else if (conditions == "Drizzle")
                paint = cloudLightP;

            for (int i = 0; i < clouds.Count; i++)
            {
                clouds[i].Update();
                cnv.DrawCircle(clouds[i].posX, clouds[i].posY, clouds[i].size, paint);
            }
            
        }

        private void CreateRain()
        {
            for (int i = 0; i < rainIntensity; i++)            
                rain.Add(new WeatherDroplet(wd, hg, windSpeed));
            
        }
        private void DrawRain()
        {
            if (rain.Count == 0)
                CreateRain();

            for (int i = 0; i < rain.Count; i++)
                rain[i].UpdateAndDraw(rainP, cnv);
        }
    }
}