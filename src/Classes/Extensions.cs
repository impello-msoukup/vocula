using System.Text.RegularExpressions;

namespace Vocula.Server;

/// <summary>
/// Extension for String
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Determines whether a specific character string matches a specified pattern.
    /// A pattern can include regular characters and wildcard characters.
    /// </summary>
    /// <param name="toSearch"></param>
    /// <param name="toFind"></param>
    /// <returns></returns>
    public static bool Like(this string toSearch, string toFind)
    {
        return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
    }
}
