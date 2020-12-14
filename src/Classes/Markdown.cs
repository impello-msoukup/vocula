using System;
using System.IO;
using Markdig;
using Markdig.Syntax;
using Markdig.Renderers.Html;
using Markdig.Parsers;
using Markdig.Renderers;

namespace Vocula.Server
{
    public class MarkdownRenderer
    {
        private readonly MarkdownPipeline pipeline;

        public MarkdownRenderer() {
            pipeline = CreateMarkdownPipeline();
        }

        public string RenderToHtml(string markdown) {
            var writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);
            var document = MarkdownParser.Parse(markdown, pipeline);
            renderer.Render(document);
            writer.Flush();
            return writer.ToString();
        }

        public string RenderToPlainText(string markdown) {
            var pipeline = new Markdig.MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseMediaLinks()
                .UseAdvancedExtensions()
                .Build();
            var result = Markdig.Markdown.ToPlainText(markdown, pipeline);
            return result;
        }

        private static MarkdownPipeline CreateMarkdownPipeline() {
            var builder = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseCustomContainers()
                .UseEmphasisExtras()
                .UseGridTables()
                .UseMediaLinks()
                .UsePipeTables()
                .UseGenericAttributes(); // Must be last as it is one parser that is modifying other parsers
            builder.DocumentProcessed += document => {
                foreach (var node in document.Descendants())
                {
                    if (node is Markdig.Syntax.Block)
                    {
                        if (node is Markdig.Extensions.Tables.Table)
                        {
                            node.GetAttributes().AddClass("md-table");
                        }
                    }
                }
            };
            return builder.Build();
        }
    }
}