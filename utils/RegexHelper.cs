using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021.utils
{
    public class RegexHelper
    {
        private readonly Regex _regex;

        public RegexHelper(string expression)
        {
            _regex = new Regex(expression, RegexOptions.Compiled);
        }

        public IEnumerable<Match> GetMatches(string input)
        {
            return _regex.Matches(input).ToList();
        }

        public static IEnumerable<Match> GetMatches(string input, string expression)
        {
            var regex = new Regex(expression);
            var mc = regex.Matches(input);

            return mc.ToList();
        }

        public bool IsMatch(string input)
        {
            return _regex.IsMatch(input);
        }

        public static bool IsMatch(string input, string expression)
        {
            var regex = new Regex(expression);
            return regex.IsMatch(input);
        }
    }
}