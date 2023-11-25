using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Vocula.Server;

/// <summary>
/// Pages class
/// </summary>
public class VoculaPages
{
    string SiteUri;
    string SiteDirectory;
    string Lang;
    VoculaTools Tools;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="siteUri"></param>
    /// <param name="siteDirectory"></param>
    /// <param name="language"></param>
    public VoculaPages(string siteUri, string siteDirectory, string language)
    {
        this.SiteUri = siteUri.TrimEnd('/');
        this.SiteDirectory = siteDirectory;
        this.Lang = language;
        this.Tools = new VoculaTools(language);
    }

    /// <summary>
    /// Get page
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private VoculaPage GetPage(string file)
    {
        VoculaPage page = new VoculaPage();
        if (File.Exists(file))
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .Build();
            string text = File.ReadAllText(file);
            using (StringReader input = new StringReader(text))
            {
                Parser parser = new Parser(input);
                parser.Consume<StreamStart>();
                parser.Consume<DocumentStart>();
                page = deserializer.Deserialize<VoculaPage>(parser);
                parser.Consume<DocumentEnd>();
            }
            page.Lang = this.Lang;
        }
        return page;
    }

    /// <summary>
    /// Get breadcrumb
    /// </summary>
    /// <param name="site"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private List<VoculaBreadcrumbItem> GetBreadcrumb(string site, string path)
    {
        List<VoculaBreadcrumbItem> breadcrumb = new List<VoculaBreadcrumbItem>();
        string[] dirs = path.TrimEnd('/').Split("/");
        for (int offset = 1; offset < dirs.Length + 1; offset++)
        {
            string partPath = string.Join("/", dirs, 0, offset);
            string pageFile = this.SiteDirectory + Path.DirectorySeparatorChar + site + partPath + Path.DirectorySeparatorChar + $"page.{this.Lang}.md";
            VoculaPage page = this.GetPage(pageFile);
            VoculaBreadcrumbItem item = new VoculaBreadcrumbItem();
            item.Path = partPath + Path.DirectorySeparatorChar;
            item.Title = page.Title;
            breadcrumb.Add(item);
        }
        return breadcrumb;
    }

    /// <summary>
    /// Get page body
    /// </summary>
    /// <param name="site"></param>
    /// <param name="path"></param>
    /// <param name="file"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    private string GetPageBody(string site, string path, string file, BodyFormat format)
    {
        string body = null;
        if (File.Exists(file))
        {
            MarkdownRenderer md = new MarkdownRenderer();
            string markdown = File.ReadAllText(file);
            markdown = markdown
                        .Replace("{!api}", this.SiteUri + "/sites/" + site + "/images/?path=" + path)
                        .Replace("{ !api }", "{!api}"); // Escape step to be able placed as text onto page
            switch (format)
            {
                case BodyFormat.Html:
                    body = md.RenderToHtml(markdown);
                    break;
                case BodyFormat.Text:
                    body = md.RenderToPlainText(markdown);
                    break;
            }
        }
        return body;
    }

    /// <summary>
    /// Set language
    /// </summary>
    /// <param name="lang"></param>
    public void SetLanguage(string lang)
    {
        this.Lang = lang;
        this.Tools.SetLanguage(lang);
    }

    /// <summary>
    /// Get page
    /// </summary>
    /// <param name="site"></param>
    /// <param name="path"></param>
    /// <param name="recursion"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public VoculaPage GetPage(string site, string path, bool recursion, BodyFormat format)
    {
        path = this.Tools.NormalizePath(path);
        string pageFile = this.SiteDirectory + Path.DirectorySeparatorChar + site + path + $"page.{this.Lang}.md";
        VoculaPage page = this.GetPage(pageFile);
        page.Path = path;
        page.Breadcrumb = this.GetBreadcrumb(site, path);
        page.Body = this.GetPageBody(site, path, pageFile, format);
        if (Directory.Exists(this.SiteDirectory + Path.DirectorySeparatorChar + site + path))
            page.Alternatives = this.Tools.GetAlternatives(Array.FindAll(Directory.GetFiles(this.SiteDirectory + Path.DirectorySeparatorChar + site + path, "*.md"), x => x.Like($"%{Path.DirectorySeparatorChar}page.%.md")));

        // Load subpages
        page.Children = new List<VoculaPage>();
        string fullPath = this.SiteDirectory + Path.DirectorySeparatorChar + site + path;
        if (recursion)
        {
            if (Directory.Exists(@fullPath))
            {
                string[] dirs = Directory.GetDirectories(@fullPath);
                foreach (string dir in dirs)
                {
                    page.Children.Add(this.GetPage(site, dir.Replace(this.SiteDirectory + Path.DirectorySeparatorChar + site, "") + Path.DirectorySeparatorChar, recursion, BodyFormat.None));
                }
                page.Children.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            }
        }
        return page;
    }

    /// <summary>
    /// Search page
    /// </summary>
    /// <param name="site"></param>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public List<VoculaSearchResult> SearchPages(string site, string searchTerm)
    {
        List<VoculaSearchResult> searchResult = new List<VoculaSearchResult>();
        if (!Directory.Exists(this.SiteDirectory + Path.DirectorySeparatorChar + site)) return searchResult;
        if (searchTerm == null) return searchResult;

        double maxRank = 0;
        List<string> keywords = Regex.Matches(searchTerm, @"[\""].+?[\""]|[^ ]+")
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        string[] files = Directory.GetFiles(this.SiteDirectory + Path.DirectorySeparatorChar + site, $"page.{this.Lang}.md", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            VoculaPage page = this.GetPage(file);
            page.Path = this.Tools.NormalizePath(file.Replace(this.SiteDirectory + Path.DirectorySeparatorChar + site, "").Replace($"page.{this.Lang}.md", ""));
            page.Body = this.GetPageBody(site, page.Path, file, BodyFormat.Text);
            page.Alternatives = this.Tools.GetAlternatives(Array.FindAll(Directory.GetFiles(file.Replace($"page.{this.Lang}.md", ""), "*.md"), x => x.Like($"%{Path.DirectorySeparatorChar}page.%.md")));
            bool finding = false;
            double pageRank = 0;
            foreach (string keyword in this.Tools.NormalizeKeywords(keywords))
            {
                double keywordRank = (this.Tools.CountOccurrences(page.Title, keyword) * 1.3)
                    + (this.Tools.CountOccurrences(page.Metadata != null ? page.Metadata.Description : null, keyword) * 1.2)
                    + this.Tools.CountOccurrences(page.Body, keyword);
                if (keywordRank > 0)
                {
                    finding = true;
                    pageRank += keywordRank;
                }
            }
            if (finding)
            {
                VoculaSearchResult searchRecord = new VoculaSearchResult();
                searchRecord.Path = page.Path;
                searchRecord.Title = page.Title;
                searchRecord.Date = page.Date;
                searchRecord.Preview = this.Tools.GetWords(page.Body, 30) + " ...";
                searchRecord.PageRank = pageRank;
                searchResult.Add(searchRecord);
                if (pageRank > maxRank) maxRank = pageRank;
            }
        }
        searchResult = new List<VoculaSearchResult>(searchResult.OrderByDescending(x => x.PageRank).ThenByDescending(x => x.Date));
        foreach (VoculaSearchResult searchRecord in searchResult) searchRecord.PageRank = Math.Round(searchRecord.PageRank / (maxRank / 100), 2);
        return searchResult;
    }
}
