using System.Collections.Generic;

namespace Vocula.Server.Settings
{
    public class CorsSettings {
        public List<string> AllowOrigins { get; set; }
        public CorsSettings() {
            this.AllowOrigins = new List<string>();
        }
    }

    public class ServerSettings {
        public string BaseUrl { get; set; }
        public CorsSettings Cors { get; set; }
        public ServerSettings() {
            this.Cors = new CorsSettings();
        }
    }

    public class DataSettings {
        public string Sites { get; set; }
        public string DefaultLanguage { get; set; }
    }

    public class VoculaSettings {
        public ServerSettings Server { get; set; }
        public DataSettings Data { get; set; }
    }
}