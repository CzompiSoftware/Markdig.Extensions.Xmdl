using Markdig.Renderers;
using System;
using Markdig.Extensions.Xmdl.Alert;
using Markdig.Extensions.Xmdl.ExecutableCode;

namespace Markdig.Extensions.Xmdl;

public class XmdlBaseExtension : IMarkdownExtension
{
    protected readonly IExecutableCodeOptions _options;

    public XmdlBaseExtension(IExecutableCodeOptions options, Uri currentUri = null)
    {
        _options = options;
        XmdlDocument.Instance.CurrentUri = currentUri;
    }

    public XmdlBaseExtension()
    {
    }

    public virtual void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.InlineParsers.ReplaceOrAdd<AlertBlockParser>(new AlertBlockParser(pipeline));
    }

    public virtual void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        renderer.ObjectRenderers.ReplaceOrAdd<AlertBlockRenderer>(new AlertBlockRenderer(pipeline));
    }

}
