namespace FoxCultGames.Utilities
{
    public static class StringExtensions
    {
        public static string ToHumanReadable(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}