using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Vocula.Server;

namespace Vocula.Controllers
{
    public class SitesQuery {
        public string Lang { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class SitesController : ControllerBase
    {
        private readonly ILogger<SitesController> Logger;
        private IConfiguration Config;
        private string BaseUrl;
        private string SitesDirectory;
        private string DefaultLanguage;
        private VoculaSites Sites; 

        public SitesController(ILogger<SitesController> logger, IConfiguration config) {
            this.Logger = logger;
            this.Config = config;
            this.BaseUrl = this.Config.GetSection("Server").GetSection("BaseUrl").Value;
            this.SitesDirectory = this.Config.GetSection("Data").GetSection("Sites").Value;
            this.DefaultLanguage = this.Config.GetSection("Data").GetSection("DefaultLanguage").Value;
            this.Sites = new VoculaSites(this.BaseUrl, this.SitesDirectory, this.DefaultLanguage);
        }

        /// <summary>
        /// Listing of available sites and their detail
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        public List<VoculaSite> GetSites([FromQuery] SitesQuery query) {
            if (query.Lang != null) Sites.SetLanguage(query.Lang);
            return Sites.GetSites();
        }

        /// <summary>
        /// Load detail of specific sites
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [Route("{siteName}")]
        public VoculaSite GetSite(string siteName, [FromQuery] SitesQuery query) {
            if (query.Lang != null) Sites.SetLanguage(query.Lang);
            return Sites.GetSite(siteName);
        }
    }
}