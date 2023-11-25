using System;
using System.Collections.Generic;

namespace Vocula.Server;

/// <summary>
/// Page body formats
/// </summary>
public enum BodyFormat
{
    ///<summary>Format not set</summary>
    None,
    ///<summary>HTML format</summary>
    Html,
    ///<summary>Plain text format</summary>
    Text
}

/// <summary>
/// Breadcrumb item
/// </summary>
public class VoculaBreadcrumbItem
{
    /// <summary>
    /// Path
    /// </summary>
    /// <value></value>
    public string Path { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    /// <value></value>
    public string Title { get; set; }
}

/// <summary>
/// Author
/// </summary>
public class VoculaSiteAuthor
{
    /// <summary>
    /// Name
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    /// <value></value>
    public string Email { get; set; }
}

/// <summary>
/// Metadata
/// </summary>
public class VoculaSiteMetadata
{
    /// <summary>
    /// Description
    /// </summary>
    /// <value></value>
    public string Description { get; set; }

    /// <summary>
    /// Athor name
    /// </summary>
    /// <value></value>
    public VoculaSiteAuthor Author { get; set; }

    /// <summary>
    /// Keyword list
    /// </summary>
    /// <value></value>
    public string[] Keywords { get; set; }
}

/// <summary>
/// Taxonomy
/// </summary>
public class VoculaTaxonomy
{
    /// <summary>
    /// Category
    /// </summary>
    /// <value></value>
    public string[] Category { get; set; }

    /// <summary>
    /// Tag
    /// </summary>
    /// <value></value>
    public string[] Tag { get; set; }
}

/// <summary>
/// Site
/// </summary>
public class VoculaSite
{
    /// <summary>
    /// Site name
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// Site language
    /// </summary>
    /// <value></value>
    public string Lang { get; set; }

    /// <summary>
    /// Site title
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// Site metadata
    /// </summary>
    /// <value></value>
    public VoculaSiteMetadata Metadata { get; set; }

    /// <summary>
    /// Site alternatives
    /// </summary>
    /// <value></value>
    public string[] Alternatives { get; set; }
}

/// <summary>
/// Page
/// </summary>
public class VoculaPage
{
    /// <summary>
    /// Page path
    /// </summary>
    /// <value></value>
    public string Path { get; set; }

    /// <summary>
    /// Breadcrumb of page path
    /// </summary>
    /// <value></value>
    public List<VoculaBreadcrumbItem> Breadcrumb { get; set; }

    /// <summary>
    /// Page language
    /// </summary>
    /// <value></value>
    public string Lang { get; set; }

    /// <summary>
    /// Page title
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// Date and time of page creation
    /// </summary>
    /// <value></value>
    public DateTime Date { get; set; }

    /// <summary>
    /// Page metadata
    /// </summary>
    /// <value></value>
    public VoculaSiteMetadata Metadata { get; set; }

    /// <summary>
    /// Page taxonomy
    /// </summary>
    /// <value></value>
    public VoculaTaxonomy Taxonomy { get; set; }

    /// <summary>
    /// Alternative pages
    /// </summary>
    /// <value></value>
    public string[] Alternatives { get; set; }

    /// <summary>
    /// Page body
    /// </summary>
    /// <value></value>
    public string Body { get; set; }

    /// <summary>
    /// Child pages
    /// </summary>
    /// <value></value>
    public List<VoculaPage> Children { get; set; }

    /// <summary>
    /// Class constructor
    /// </summary>
    public VoculaPage()
    {
        Breadcrumb = new List<VoculaBreadcrumbItem>();
        Children = new List<VoculaPage>();
    }
}

/// <summary>
/// Search result
/// </summary>
public class VoculaSearchResult
{
    /// <summary>
    /// Page path
    /// </summary>
    /// <value></value>
    public string Path { get; set; }

    /// <summary>
    /// Page title
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// Date and time of page creation
    /// </summary>
    /// <value></value>
    public DateTime Date { get; set; }

    /// <summary>
    /// Page preview
    /// </summary>
    /// <value></value>
    public string Preview { get; set; }

    /// <summary>
    /// Page rank index
    /// </summary>
    /// <value></value>
    public double PageRank { get; set; }
}
