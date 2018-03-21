using System;

namespace ChatHelper
{
    public static class NameRepository
    {
        private static string[] names;

        static NameRepository()
        {
            names = new[] {"Nataly", "Jim Beam", "Johnnie Walker", "Evan Williams", "Jose Cuervo" };
        }

        public static string GetRandomName()
        {
            var random = new Random();
            var randomIndex = random.Next(names.Length);
            return names[randomIndex];
        }
    }
}
