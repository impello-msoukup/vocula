using System.IO;
using Microsoft.AspNetCore.Mvc;
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
            var image = this.SitesDirectory + Path.DirectorySeparatorChar + siteName + Path.DirectorySeparatorChar + query.Path;
            if (System.IO.File.Exists(image)) {
                string extension = Path.GetExtension(image).TrimStart('.').ToLower();
                if (extension == "jpg") extension = "jpeg";
                return PhysicalFile(@image, "image/" + extension);
            } else {
                return NotFound(null);
            }
        }
    }
}