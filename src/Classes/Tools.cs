using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vocula.Server
{
    public class VoculaTools
    {
        string Lang;

        public VoculaTools(string language) {
            this.Lang = language;
        }

        public void SetLanguage(string lang) {
            this.Lang = lang;
        }

        public string NormalizePath(string path) {
            path = path ?? $"{Path.DirectorySeparatorChar}";
            string forbiddenPattern = @"\.\.(\\|\/)"; // Skipping to the parent directory is banned
            path = Regex.Replace(path, forbiddenPattern, (string)"");
            string dirSeparators = @"(\\|\/)"; // Normalize directory separators
            path = Regex.Replace(path, dirSeparators, $"{Path.DirectorySeparatorChar}").Trim(Path.DirectorySeparatorChar);
            return path.Length == 0 ? $"{Path.DirectorySeparatorChar}" : $"{Path.DirectorySeparatorChar}{path}{Path.DirectorySeparatorChar}";
        }

        public string[] GetAlternatives(string[] files) {
            List<string> alternatives = new List<string>();
            foreach (string file in files) {
                var alt = Path.GetFileNameWithoutExtension(file).Replace("site.", "").Replace("page.", "");
                if (alt != this.Lang) alternatives.Add(alt);
            }
            return alternatives.ToArray();
        }

        public List<string> NormalizeKeywords(List<string> keywords) {
            List<string> normalizedKeywords = new List<string>();
            foreach (string keyword in keywords) {
                normalizedKeywords.Add(keyword.Replace("\"", ""));
            }
            return normalizedKeywords;
        }

        public int CountOccurrences(string text, string pattern) {
            int count = 0;
            if (text == null) return count;
            int i = 0;
            while ((i = text.IndexOf(pattern, i, StringComparison.InvariantCultureIgnoreCase)) != -1) {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        public string GetWords(string text, int count) {
            string[] words = text.Split(' ');
            return string.Join(" ", words, 0, count);
        }
    }
}