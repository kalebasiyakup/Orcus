using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orcus.Core.Extension
{
    public static class OrcusIntegerExtension
    {
        public static string GenerateRandom(this int length)
        {
            StringBuilder generated = new StringBuilder();

            Random randomizer = new Random();
            List<int> possibleChars = new List<int>();
            possibleChars.AddRange(Enumerable.Range(48, 10)); //0-9
            possibleChars.AddRange(Enumerable.Range(65, 26)); //A-Z
            possibleChars.AddRange(Enumerable.Range(97, 26)); //a-z

            for (int i = 1; i <= length; i++)
            {
                generated.Append((char)possibleChars[randomizer.Next(possibleChars.Count)]);
            }

            return generated.ToString();
        }
    }
}
