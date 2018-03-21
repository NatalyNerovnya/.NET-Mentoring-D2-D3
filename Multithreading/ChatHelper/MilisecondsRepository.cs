using System;

namespace ChatHelper
{
    public static class MilisecondsRepository
    {
        public static int GetRandomMilisecondsNumber()
        {
            var random = new Random();
            return random.Next(1000, 6000);
        }
    }
}
