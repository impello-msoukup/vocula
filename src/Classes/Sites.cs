using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Vocula.Server;

/// <summary>
/// Sites class
/// </summary>
public class VoculaSites
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
    public VoculaSites(string siteUri, string siteDirectory, string language)
    {
        this.SiteUri = siteUri.TrimEnd('/');
        this.SiteDirectory = siteDirectory;
        this.Lang = language;
        this.Tools = new VoculaTools(this.Lang);
    }

    /// <summary>
    /// Get YAML of site
    /// </summary>
    /// <param name="yamlFilePath"></param>
    /// <returns></returns>
    private VoculaSite GetYamlOfSite(string yamlFilePath)
    {
        VoculaSite site;
        if (File.Exists(yamlFilePath))
        {
            var deserializer = new DeserializerBuilder()
                .Build();
            site = deserializer.Deserialize<VoculaSite>(File.OpenText(yamlFilePath));
            site.Lang = this.Lang;
        }
        else
        {
            site = new VoculaSite();
        }
        return site;
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
    /// Get Sites
    /// </summary>
    /// <returns></returns>
    public List<VoculaSite> GetSites()
    {
        List<VoculaSite> sites = new List<VoculaSite>();
        try
        {
            if (Directory.Exists(this.@SiteDirectory))
            {
                string[] dirs = Directory.GetDirectories(this.@SiteDirectory);
                foreach (string dir in dirs)
                {
                    VoculaSite site = new VoculaSite();
                    site = GetYamlOfSite(dir + Path.DirectorySeparatorChar + $"site.{this.Lang}.yaml");
                    site.Name = dir.Replace(this.SiteDirectory, "").TrimStart(Path.DirectorySeparatorChar);
                    site.Alternatives = this.Tools.GetAlternatives(Array.FindAll(Directory.GetFiles(dir + Path.DirectorySeparatorChar, "*.yaml"), x => x.Like($"%{Path.DirectorySeparatorChar}site.%.yaml")));
                    sites.Add(site);
                }
                sites.Sort((x, y) => String.Compare(x.Name, y.Name));
            }
        }
        catch { }
        return sites;
    }

    /// <summary>
    /// Get site
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public VoculaSite GetSite(string name)
    {
        VoculaSite site = new VoculaSite();
        try
        {
            if (Directory.Exists(this.SiteDirectory + Path.DirectorySeparatorChar + name))
            {
                site = GetYamlOfSite(this.SiteDirectory + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar + $"site.{this.Lang}.yaml");
                site.Name = name;
                site.Alternatives = this.Tools.GetAlternatives(Array.FindAll(Directory.GetFiles(this.SiteDirectory + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar, "*.yaml"), x => x.Like($"%{Path.DirectorySeparatorChar}site.%.yaml")));
            }
        }
        catch { }
        return site;
    }
}
