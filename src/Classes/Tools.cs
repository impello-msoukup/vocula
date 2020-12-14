using System;
using System.IO;
using System.Collections.Generic;

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
            path = path ?? "/";
            path = path.Replace("\\", "/");
            if (path.Substring(0,1) != "/") path = $"/{path}";
            if (path.Substring(path.Length-1,1) != "/") path = $"{path}/";
            return path;
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