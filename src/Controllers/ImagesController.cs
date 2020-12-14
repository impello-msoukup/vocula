using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Vocula.Server;

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
        private IConfiguration Config;
        private string BaseUrl;
        private string SitesDirectory;
        private string DefaultLanguage;
        private VoculaSites Sites; 

        public ImagesController(ILogger<ImagesController> logger, IConfiguration config) {
            this.Logger = logger;
            this.Config = config;
            this.BaseUrl = this.Config.GetSection("Server").GetSection("BaseUrl").Value;
            this.SitesDirectory = this.Config.GetSection("Data").GetSection("Sites").Value;
            this.DefaultLanguage = this.Config.GetSection("Data").GetSection("DefaultLanguage").Value;
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