using System.Text.RegularExpressions;

namespace Common.Strings
{
    public static class RegexValidator
    {
        public static bool IsMatch(string regex, string value)
        {
            Regex rgx = new Regex(regex);
            return rgx.IsMatch(value);
        }
    }
}