using Markdig.Extensions.Xmdl;
using System;

namespace Markdig.Extensions.Xmdl;

public static class XmdlExtensions
{
    public static MarkdownPipelineBuilder UseSimpleXmdl(this MarkdownPipelineBuilder pipeline, Uri currentUri)
    {
        pipeline.Extensions.Add(new XmdlBaseExtension(null, currentUri));
        return pipeline;
    }
}
