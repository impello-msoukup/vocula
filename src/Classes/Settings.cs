using System.Collections.Generic;

namespace Vocula.Server.Settings;

/// <summary>
/// CORS settings
/// </summary>
public class CorsSettings
{
    /// <summary>
    /// Allowed origins
    /// </summary>
    /// <value></value>
    public List<string> AllowOrigins { get; set; }

    /// <summary>
    /// Class constructor
    /// </summary>
    public CorsSettings()
    {
        this.AllowOrigins = new List<string>();
    }
}

/// <summary>
/// Server settings
/// </summary>
public class ServerSettings
{
    /// <summary>
    /// Base URL
    /// </summary>
    /// <value></value>
    public string BaseUrl { get; set; }

    /// <summary>
    /// CORS settings
    /// </summary>
    /// <value></value>
    public CorsSettings Cors { get; set; }

    /// <summary>
    /// Class constructor
    /// </summary>
    public ServerSettings()
    {
        this.Cors = new CorsSettings();
    }
}

/// <summary>
/// Data settings
/// </summary>
public class DataSettings
{
    /// <summary>
    /// Sites
    /// </summary>
    /// <value></value>
    public string Sites { get; set; }

    /// <summary>
    /// Default language
    /// </summary>
    /// <value></value>
    public string DefaultLanguage { get; set; }
}

/// <summary>
/// Application settings
/// </summary>
public class VoculaSettings
{
    /// <summary>
    /// Server settings
    /// </summary>
    /// <value></value>
    public ServerSettings Server { get; set; }

    /// <summary>
    /// Data settings
    /// </summary>
    /// <value></value>
    public DataSettings Data { get; set; }
}
