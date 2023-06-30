using System;
using System.Collections.Generic;

namespace ExampleService.Model
{
    public class ColoredAlienCat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ColoredAlienCat(int id)
        {
            Random randomColor = new Random();
            Random randomLetter = new Random();

            List<string> colors = new List<string> { "Green", "Red", "Blue", "Orange", "Yellow", "Purple", "Pink", "Black", "White" };
            string letters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < 10; i++)
            {
                string name = string.Empty;
                int colorID = randomColor.Next(colors.Count);

                for (var j = 0; j < 7; j++)
                {
                    int num = randomLetter.Next(0, letters.Length);
                    name += letters[num];
                }

                Id = id;
                Color = colors[colorID];
                Name = name;
            }
        }
    }
}
