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
       
        private SKPaint sunP, cloudLightP, cloudGrayP, cloudDarkP, 
            thunderCloudDarkP, thunderCloudLightP, thunderCloudGrayP, rainFarP, rainMediumP, rainCloseP, snowFarP, snowMediumP, snowCloseP, thunderP;
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

            thunderCloudLightP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.LightSlateGray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            thunderCloudGrayP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.SlateGray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            thunderCloudDarkP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.DarkSlateGray,
                IsAntialias = true,
                StrokeWidth = 2,
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

            cloudGrayP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.DarkGray,
                StrokeWidth = 2,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Square
            };

            cloudDarkP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.Gray,
                IsAntialias = true,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };
                      

            thunderP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.White,
                StrokeWidth = 4,
                StrokeCap = SKStrokeCap.Square
            };

            rainFarP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkCyan,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };

            rainMediumP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Cyan,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };
            
            
            rainCloseP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.LightCyan,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Square
            };

            snowFarP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.SlateGray,
                StrokeWidth = 4,
                StrokeCap = SKStrokeCap.Square
            };

            snowMediumP = new SKPaint
            {      // paint for the axis and text
                Style = SKPaintStyle.Stroke,
                Color = SKColors.LightGray,
                StrokeWidth = 4,
                StrokeCap = SKStrokeCap.Square
            };


            snowCloseP = new SKPaint
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
            
            if (rainIntensity >= 0 && conditions != "Clear")
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

            float margin = Math.Min(wd / 10, hg / 10);

            float bladeRadius = Math.Min(wd / 5, hg / 5);
            double currPos = sunRotation;
            int blades = 8;
            float startX, startY , endX, endY;
            for (int i = 0; i < blades; i++)
            {
                startX = wd / 2 + sunRadius * (float) Math.Cos(currPos) + margin * (float)Math.Cos(currPos);
                endX = startX + bladeRadius * (float) Math.Cos(currPos);

                startY = hg / 2 + sunRadius * (float) Math.Sin(currPos) + margin * (float)Math.Sin(currPos);
                endY = startY + bladeRadius * (float)Math.Sin(currPos);
                
                currPos += Math.PI * 2 / blades;

                cnv.DrawLine(startX, startY, endX, endY, sunP);
            }
        }
        
        private void CreateClouds()
        {
            int numClouds = 10;
            
            
            for (int i = 0; i < numClouds; i++)
            {
                if (conditions == "Thunderstorm ")
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i , numClouds, conditions, 3, thunderCloudLightP, thunderP));
                else
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i, numClouds, conditions, 3, cloudLightP, thunderP));
            }
            
            numClouds = 15;

            for (int i = 0; i < numClouds; i++)
            {
                if (conditions == "Thunderstorm")
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i, numClouds, conditions, 2, thunderCloudGrayP, thunderP));
                else
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i, numClouds, conditions, 2, cloudGrayP, thunderP));
            }

            numClouds = 20;
            for (int i = 0; i < numClouds; i++)
            {
                if (conditions == "Thunderstorm")
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i, numClouds, conditions, 1, thunderCloudDarkP, thunderP));
                else 
                    clouds.Add(new WeatherCloud(wd, hg, windSpeed, i, numClouds, conditions, 1, cloudDarkP, thunderP));
            }
        }

 
        private void DrawClouds()
        {
            if (clouds.Count == 0)
                CreateClouds();

            isThundering = false;
            if (conditions == "Thunderstorm")
            {
                if (rnd.Next(0,100) < thunderIntensity)
                    isThundering = true;
                
            }
            if (isThundering)
                clouds[rnd.Next(0, clouds.Count -1)].TriggerThunder(cnv);
            for (int i = 0; i < clouds.Count; i++)
            {
                clouds[i].UpdateAndDraw(cnv, isThundering);
            }   
        }

        private void CreateRain()
        {
            int numDrops = (int) ((rainIntensity / 30f) * wd);
            
            if (conditions == "Drizzle")
                numDrops *= 5;
            if (conditions == "Snow")
                numDrops *= 7;

            for (int i = 0; i < numDrops; i++)
            {
                if (conditions == "Snow")
                    rain.Add(new WeatherDroplet(wd, hg, windSpeed, conditions, snowFarP, snowMediumP, snowCloseP, thunderP));
                else
                    rain.Add(new WeatherDroplet(wd, hg, windSpeed, conditions, rainFarP, rainMediumP, rainCloseP, thunderP));
            }
                
            
        }
        private void DrawRain()
        {
            if (rain.Count == 0)
                CreateRain();

           for (int i = 0; i < rain.Count; i++)
            {
                rain[i].UpdateAndDraw(cnv, isThundering);
            }
                
        }
    }
}