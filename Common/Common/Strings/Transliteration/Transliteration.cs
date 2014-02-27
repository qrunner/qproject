namespace Common.Strings.Transliteration
{
    /// <summary>
    /// Фабрика для провайдеров транслитерации
    /// </summary>
    public static class Transliteration
    {
        static Transliteration()
        {
            DefaultProvider = new CyrillicToLatinProvider();
        }
        /// <summary>
        /// Провайдер транслитерации по умолчанию
        /// </summary>
        public static ITransliterationProvider DefaultProvider { get; private set; }
    }
}