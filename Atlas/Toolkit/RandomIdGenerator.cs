using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Toolkit
{
    /// <summary>
    /// Used to generate random identificators.
    /// </summary>
    public class RandomIdGenerator
    {
        /// <summary>
        /// A list of all numbers included in the generation.
        /// </summary>
        public static readonly IReadOnlyList<char> Numbers = new List<char>(10) { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// A list of all letters included in the generation.
        /// </summary>
        public static readonly IReadOnlyList<char> Letters = new List<char>(26) { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
                                                                                  'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        internal Random random;

        internal int length;
        internal bool includeLetters;
        internal bool includeNumbers;

        /// <summary>
        /// Creates a new random ID generator.
        /// </summary>
        /// <param name="length">The length of the generation.</param>
        /// <param name="includeLetters">Whether or not to include letters.</param>
        /// <param name="includeNumbers">Whether or not to include numbers.</param>
        public RandomIdGenerator(int length = 10, bool includeLetters = true, bool includeNumbers = true)
        {
            this.length = length;
            this.includeLetters = includeLetters;
            this.includeNumbers = includeNumbers;

            random = new Random();
        }

        ~RandomIdGenerator()
        {
            random = null;
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        /// <returns>The generated identifier.</returns>
        public string Next()
        {
            string str = "";

            while (str.Length != length)
            {
                bool isNumber = random.Next(1) == 0;

                if (isNumber)
                {
                    if (!includeNumbers)
                        continue;

                    str += Numbers[random.Next(Numbers.Count)];
                }
                else
                {
                    if (!includeLetters)
                        continue;

                    str += Letters[random.Next(Letters.Count)];
                }
            }

            return str;
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        /// <param name="excludeFrom">Make sure that the generated identifier is not in this list.</param>
        /// <returns>The generated identifier.</returns>
        public string Next(IEnumerable<string> excludeFrom)
        {
            if (excludeFrom == null)
                return null;

            string str = "";

            while (str.Length != length || excludeFrom.Contains(str))
            {
                str = Next();
            }

            return str;
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        /// <returns>The generated identifier.</returns>
        public ulong NextNumber()
        {
            string str = "";

            while (str.Length != length)
            {
                str += Numbers[random.Next(Numbers.Count)];
            }

            return ulong.Parse(str);
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        /// <param name="excludeFrom">Make sure that the generated identifier is not in this list.</param>
        /// <returns>The generated identifier.</returns>
        public ulong NextNumber(IEnumerable<ulong> excludeFrom)
        {
            string str = "";

            IEnumerable<string> stringValues = excludeFrom.Select(x => x.ToString());

            while (str.Length != length || stringValues.Contains(str))
            {
                str += Numbers[random.Next(Numbers.Count)];
            }

            return ulong.Parse(str);
        }
    }
}
