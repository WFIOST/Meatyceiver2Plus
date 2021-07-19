using System;

namespace Meatyceiver2Plus
{
    public static class Common
    {
        private static Random _random = new Random();

        public static float GetRandomValue(int min, int max) => _random.Next(min, max);
    }
}