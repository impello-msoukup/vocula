using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vocula.Server;

/// <summary>
/// Tools class
/// </summary>
public class VoculaTools
{
    string Lang;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="language"></param>
    public VoculaTools(string language)
    {
        this.Lang = language;
    }

    /// <summary>
    /// Set language
    /// </summary>
    /// <param name="lang"></param>
    public void SetLanguage(string lang)
    {
        this.Lang = lang;
    }

    /// <summary>
    /// Normalize path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string NormalizePath(string path)
    {
        path = path ?? $"{Path.DirectorySeparatorChar}";
        string forbiddenPattern = @"\.\.(\\|\/)"; // Skipping to the parent directory is banned
        path = Regex.Replace(path, forbiddenPattern, (string)"");
        string dirSeparators = @"(\\|\/)"; // Normalize directory separators
        path = Regex.Replace(path, dirSeparators, $"{Path.DirectorySeparatorChar}").Trim(Path.DirectorySeparatorChar);
        return path.Length == 0 ? $"{Path.DirectorySeparatorChar}" : $"{Path.DirectorySeparatorChar}{path}{Path.DirectorySeparatorChar}";
    }

    /// <summary>
    /// Get alternatives
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public string[] GetAlternatives(string[] files)
    {
        List<string> alternatives = new List<string>();
        foreach (string file in files)
        {
            var alt = Path.GetFileNameWithoutExtension(file).Replace("site.", "").Replace("page.", "");
            if (alt != this.Lang) alternatives.Add(alt);
        }
        return alternatives.ToArray();
    }

    /// <summary>
    /// Normalize keywords
    /// </summary>
    /// <param name="keywords"></param>
    /// <returns></returns>
    public List<string> NormalizeKeywords(List<string> keywords)
    {
        List<string> normalizedKeywords = new List<string>();
        foreach (string keyword in keywords)
        {
            normalizedKeywords.Add(keyword.Replace("\"", ""));
        }
        return normalizedKeywords;
    }

    /// <summary>
    /// Count occurrences
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public int CountOccurrences(string text, string pattern)
    {
        int count = 0;
        if (text == null) return count;
        int i = 0;
        while ((i = text.IndexOf(pattern, i, StringComparison.InvariantCultureIgnoreCase)) != -1)
        {
            i += pattern.Length;
            count++;
        }
        return count;
    }

    /// <summary>
    /// Get words
    /// </summary>
    /// <param name="text"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public string GetWords(string text, int count)
    {
        string[] words = text.Split(' ');
        return string.Join(" ", words, 0, count);
    }
}
