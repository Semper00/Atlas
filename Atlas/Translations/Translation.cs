using System.Collections.Generic;

namespace Atlas.Translations
{
    /// <summary>
    /// The base class for translations.
    /// </summary>
    public abstract class Translation
    {
        public Translation()
            => Translations = new Dictionary<string, string>();

        /// <summary>
        /// Gets the language of this translation.
        /// </summary>
        public abstract string Language { get; set; }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        public Dictionary<string, string> Translations { get; set; }

        /// <summary>
        /// Formats the string with translations provided in <see cref="Translations"/>.
        /// </summary>
        public string Format(string str)
        {
            foreach (string st in str.Split(' '))
            {
                if (Translations.TryGetValue(st, out string translation))
                    str = str.Replace(st, translation);
            }

            return str;
        }

        /// <summary>
        /// Formats the string with translations provided in <see cref="Translations"/>.
        /// </summary>
        public void Format(ref string str)
        {
            foreach (string st in str.Split(' '))
            {
                if (Translations.TryGetValue(st, out string translation))
                    str = str.Replace(st, translation);
            }
        }

        /// <summary>
        /// Adds a new translation.
        /// </summary>
        /// <param name="word">The word to translate.</param>
        /// <param name="translation">The translation.</param>
        /// <param name="delimiter">The delimiter ([delimiter]word[delimiter]), for example %word%.</param>
        public void Create(string word, string translation, char delimiter)
        {
            string delimited = $"{delimiter}{word}{delimiter}";

            if (!Translations.TryGetValue(delimited, out _))
                Translations.Add(delimited, translation);
            else
                Translations[delimited] = translation;
        }

        /// <summary>
        /// Removes a translation.
        /// </summary>
        /// <param name="word">The word to translate.</param>
        /// <param name="delimiter">The delimiter ([delimiter]word[delimiter]), for example %word%.</param>
        public bool Remove(string word, char delimiter)
            => Translations.Remove($"{delimiter}{word}{delimiter}");
    }
}
