using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Strings.Transliteration
{
    /// <summary>
    /// Базовый класс для простой транслитерации на основе словаря символов
    /// </summary>
    public abstract class DictionaryTransliteProvider : ITransliterationProvider
    {
        protected abstract IDictionary<char, string> TranslitDictionary { get; }

        public string Translite(IEnumerable<char> text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
                sb.Append(Translite(c));

            return sb.ToString();
        }

        public string Translite(char c)
        {
            return TranslitDictionary.ContainsKey(c) ? TranslitDictionary[c] : c.ToString();
        }

        public int TransliteLength(IEnumerable<char> text)
        {
            return text.Sum(c => TransliteLength(c));
        }

        public int TransliteLength(char c)
        {
            return TranslitDictionary.ContainsKey(c) ? TranslitDictionary[c].Length : 1;
        }
    }
}
