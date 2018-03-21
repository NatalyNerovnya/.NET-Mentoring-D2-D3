using System;

namespace ChatHelper
{
    public static class MessageRepository
    {
        private static string[] messages;
        private static Random random;

        static MessageRepository()
        {
            messages = new[] { "Hello!",
            "I drink to make other people more interesting.",
            "Next to music, beer was best.",
            "In wine there is wisdom, in beer there is Freedom, in water there is bacteria.",
            "First you take a drink, then the drink takes a drink, then the drink takes you.",
            "How are you?",
            "Great!",
            "I cook with wine, sometimes I even add it to the food.",
            "Alcohol may be man's worst enemy, but the bible says love your enemy.",
            "I don't have a drinking problem 'Cept when I can't get a drink.",
            "I like to see the glass as half full, hopefully of jack daniels.",
            "There is no bad whiskey. There are only some whiskeys that aren't as good as others.",
            "The only cure for a real hangover is death.",
            "Now is the time to drink!"};
            random = new Random();
        }

        public static string GetRandomMessage()
        {
            var randomNumber = random.Next(messages.Length);
            return messages[randomNumber];
        }

        public static int GetRandomMessageNumber()
        {
            return random.Next(1, 11);
        }
    }
}
