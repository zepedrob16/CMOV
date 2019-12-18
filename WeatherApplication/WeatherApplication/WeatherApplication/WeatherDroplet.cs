using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApplication
{
    class WeatherDroplet
    {
        public float posX, posY, speed, currPos, size;

        // Ripple
        public bool isDisappearing;
        public float timeSinceDisappearing;
        public float rippleAnimationTime;

        public WeatherDroplet(float pX, float pY, float s, float cP, float sZ)
        {
            posX = pX;
            posY = pY;
            speed = s;
            currPos = cP;
            size = sZ;

            isDisappearing = false;
            timeSinceDisappearing = 0;
            rippleAnimationTime = 0.5f;
        }
    }
}
