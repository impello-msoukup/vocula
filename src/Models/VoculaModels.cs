using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Vocula.Server
{
    public enum BodyFormat {
        None, Html, Text
    }

    public class VoculaSiteAuthor {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class VoculaSiteMetadata {
        public string Description { get; set; }
        public VoculaSiteAuthor Author { get; set; }
        public string[] Keywords { get; set; }
    }

    public class VoculaTaxonomy {
        public string[] Category { get; set; }
        public string[] Tag { get; set; }
    }

    public class VoculaSite {
        public string Name { get; set; }
        public string Lang { get; set; }
        public string Title { get; set; }
        public VoculaSiteMetadata Metadata { get; set; }
        public string[] Alternatives { get; set; }
    }

    public class VoculaPage {
        public string Path { get; set; }
        public string Lang { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public VoculaSiteMetadata Metadata { get; set; }
        public VoculaTaxonomy Taxonomy { get; set; }
        public string[] Alternatives { get; set; }
        public string Body { get; set; }
        public List<VoculaPage> Children { get; set; }
        public VoculaPage() {
            Children = new List<VoculaPage>();
        }
    }

    public class VoculaSearchResult {
        public string Path { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Preview { get; set; }
        public double PageRank { get; set; }
    }
}