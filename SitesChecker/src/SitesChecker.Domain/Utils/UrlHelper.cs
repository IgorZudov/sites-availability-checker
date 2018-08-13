using System.Text.RegularExpressions;

namespace SitesChecker.Domain.Utils
{
    public static class UrlHelper
    {
        private const string Pattern = @"^(http|https|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";

        
        public static bool IsUrlValid(string url)
        {

           var reg = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
    }
}