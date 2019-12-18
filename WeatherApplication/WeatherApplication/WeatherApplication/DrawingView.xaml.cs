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
        private String conditions = "", description = "";
        private Random rnd;
        private bool isThundering;

        // Animation Parameters
        // Sun
        private float sunRotation;

        // Rain
        private float rainIntensity = 1;
        private float thunderIntensity = 0;
       
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
            rnd = new Random();
        }

        public void setConditions(String conds, String desc, float wind, float rain)
        {
            description = desc;
            conditions = conds;
            windSpeed = wind;
            rainIntensity = rain;

            if (description.Contains("light thunderstorm"))
                thunderIntensity = 2;
            else if (description.Contains("heavy thunderstorm"))
                thunderIntensity = 5;
            else if (description.Contains("ragged thunderstorm"))
                thunderIntensity = 8;
            else if (description.Contains("thunderstorm"))
                thunderIntensity = 3;
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
                Color = SKColors.White,
                StrokeWidth = 4,
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
            
            if (rainIntensity >= 0)
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
            isThundering = false;
            if (conditions == "Thunderstorm")
            {
                paint = cloudDarkP;
                if (rnd.Next(0,100) < thunderIntensity)
                {
                    isThundering = true;
                    paint = thunderP;
                }
            }
            else if (conditions == "Drizzle")
                paint = cloudLightP;
            
            if (isThundering)
                clouds[rnd.Next(0, clouds.Count -1)].TriggerThunder(paint, cnv);

            for (int i = 0; i < clouds.Count; i++)
            {
                clouds[i].UpdateAndDraw(paint, cnv);
            }   
        }

        private void CreateRain()
        {
            int numDrops = (int) ((rainIntensity / 100f) * wd);
            for (int i = 0; i < numDrops; i++)            
                rain.Add(new WeatherDroplet(wd, hg, windSpeed, conditions));
            
        }
        private void DrawRain()
        {
            if (rain.Count == 0)
                CreateRain();

            for (int i = 0; i < rain.Count; i++)
            {
                if (isThundering)
                    rain[i].UpdateAndDraw(thunderP, cnv);
                else
                    rain[i].UpdateAndDraw(rainP, cnv);
            }
                
        }
    }
}