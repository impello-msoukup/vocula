using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vocula.Server;
using Vocula.Server.Settings;

namespace Vocula.Controllers
{
    public class ImagesQuery {
        public string Path { get; set; }
    }

    [ApiController]
    [Route("Sites/{siteName}/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> Logger;
        private readonly VoculaSettings Config;
        private string BaseUrl;
        private string SitesDirectory;
        private string DefaultLanguage;
        private VoculaSites Sites; 

        public ImagesController(ILogger<ImagesController> logger, IOptions<VoculaSettings> config) {
            this.Logger = logger;
            this.Config = config.Value;
            this.BaseUrl = this.Config.Server.BaseUrl;
            this.SitesDirectory = this.Config.Data.Sites;
            this.DefaultLanguage = this.Config.Data.DefaultLanguage;
            this.Sites = new VoculaSites(this.BaseUrl, this.SitesDirectory, this.DefaultLanguage);
        }

        /// <summary>
        /// Load a specific image from the given site
        /// </summary>
        [HttpGet]
        public IActionResult GetImage(string siteName, [FromQuery] ImagesQuery query) {
            string defaultContentType = "application/octet-stream"; // Default content type
            string forbiddenPattern = @"\.\.(\\|\/)"; // Skipping to the parent directory is banned
            string imagePath = this.SitesDirectory + Path.DirectorySeparatorChar + siteName + Path.DirectorySeparatorChar + Regex.Replace(query.Path.TrimStart(Path.DirectorySeparatorChar), forbiddenPattern, (string)"");
            if (System.IO.File.Exists(imagePath)) {
                FileExtensionContentTypeProvider ctProvider = new FileExtensionContentTypeProvider();
                if (!ctProvider.TryGetContentType(imagePath, out string contentType)) {
                    contentType = defaultContentType;
                }
                return PhysicalFile(@imagePath, contentType);
            } else {
                return NotFound(null);
            }
        }
    }
}