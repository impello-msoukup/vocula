using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vocula.Server;
using Vocula.Server.Settings;

namespace Vocula.Controllers;

/// <summary>
/// Pages query
/// </summary>
public class PagesQuery
{
    /// <summary>
    /// Entry path
    /// </summary>
    /// <value></value>
    public string Path { get; set; }

    /// <summary>
    /// Recursion index
    /// </summary>
    /// <value></value>
    public bool Recursion { get; set; }

    /// <summary>
    /// Bopdy format
    /// </summary>
    /// <value></value>
    public BodyFormat Format { get; set; }

    /// <summary>
    /// Language
    /// </summary>
    /// <value></value>
    public string Lang { get; set; }

    /// <summary>
    /// Class constructor
    /// </summary>
    public PagesQuery()
    {
        Path = "/";
        Recursion = false;
        Format = BodyFormat.Html;
    }
}

/// <summary>
/// Search query
/// </summary>
public class SearchQuery
{
    /// <summary>
    /// Keyword
    /// </summary>
    /// <value></value>
    public string Keyword { get; set; }

    /// <summary>
    /// Language
    /// </summary>
    /// <value></value>
    public string Lang { get; set; }
}

/// <summary>
/// Pages controller
/// </summary>
[ApiController]
[Route("Sites/{siteName}/[controller]")]
public class PagesController : ControllerBase
{
    private readonly ILogger<PagesController> Logger;
    private readonly VoculaSettings Config;
    private string BaseUrl;
    private string SitesDirectory;
    private string DefaultLanguage;
    private VoculaPages Pages;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public PagesController(ILogger<PagesController> logger, IOptions<VoculaSettings> config)
    {
        this.Logger = logger;
        this.Config = config.Value;
        this.BaseUrl = this.Config.Server.BaseUrl;
        this.SitesDirectory = this.Config.Data.Sites;
        this.DefaultLanguage = this.Config.Data.DefaultLanguage;
        this.Pages = new VoculaPages(this.BaseUrl, this.SitesDirectory, this.DefaultLanguage);
    }

    /// <summary>
    /// Listing of pages from the given site
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    public VoculaPage GetPages(string siteName, [FromQuery] PagesQuery query)
    {
        if (query.Lang != null) Pages.SetLanguage(query.Lang);
        return Pages.GetPage(siteName, query.Path, query.Recursion, query.Format);
    }

    /// <summary>
    /// Search pages by keyword from the given site
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [Route("Search")]
    public List<VoculaSearchResult> SearchPages(string siteName, [FromQuery] SearchQuery query)
    {
        if (query.Lang != null) Pages.SetLanguage(query.Lang);
        return Pages.SearchPages(siteName, query.Keyword);
    }
}
