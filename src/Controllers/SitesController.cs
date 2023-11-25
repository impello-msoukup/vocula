using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vocula.Server;
using Vocula.Server.Settings;

namespace Vocula.Controllers;

/// <summary>
/// Sites query
/// </summary>
public class SitesQuery
{
    /// <summary>
    /// Language
    /// </summary>
    /// <value></value>
    public string Lang { get; set; }
}

/// <summary>
/// Sites controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class SitesController : ControllerBase
{
    private readonly ILogger<SitesController> Logger;
    private readonly VoculaSettings Config;
    private string BaseUrl;
    private string SitesDirectory;
    private string DefaultLanguage;
    private VoculaSites Sites;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public SitesController(ILogger<SitesController> logger, IOptions<VoculaSettings> config)
    {
        this.Logger = logger;
        this.Config = config.Value;
        this.BaseUrl = this.Config.Server.BaseUrl;
        this.SitesDirectory = this.Config.Data.Sites;
        this.DefaultLanguage = this.Config.Data.DefaultLanguage;
        this.Sites = new VoculaSites(this.BaseUrl, this.SitesDirectory, this.DefaultLanguage);
    }

    /// <summary>
    /// Listing of available sites and their detail
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    public List<VoculaSite> GetSites([FromQuery] SitesQuery query)
    {
        if (query.Lang != null) Sites.SetLanguage(query.Lang);
        return Sites.GetSites();
    }

    /// <summary>
    /// Load detail of specific sites
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [Route("{siteName}")]
    public VoculaSite GetSite(string siteName, [FromQuery] SitesQuery query)
    {
        if (query.Lang != null) Sites.SetLanguage(query.Lang);
        return Sites.GetSite(siteName);
    }
}
