﻿using Markdig.Renderers;
using Markdig.Renderers.Html;
using System;
using System.Linq;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public class ExecutableCodeBlockRenderer : HtmlObjectRenderer<ExecutableCodeBlock>
{
    private readonly ExecutableCodeRenderer _codeRenderer;
    private readonly IExecutableCodeOptions _options;
    private readonly MarkdownPipeline _pipeline;

    public ExecutableCodeBlockRenderer(ExecutableCodeRenderer codeRenderer, IExecutableCodeOptions options, MarkdownPipeline pipeline)
    {
        _codeRenderer = codeRenderer;
        _options = options;
        _pipeline = pipeline;
    }

    protected override async void Write(HtmlRenderer renderer, ExecutableCodeBlock obj)
    {
        try
        {
            var (errors, content) = await _codeRenderer.WriteAsync(obj.SourceCode.ToString(), false, obj.Context);
            renderer.Write($"{content}");

            if (errors?.Any() ?? false)
            {
                renderer.Write(string.Join("<br>\r\n", errors));
            }
        }
        catch (Exception ex)
        {
            renderer.Write(Markdown.ToHtml(_codeRenderer.BuildMarkdownExceptionMessage(obj.CodeLanguage, ex, _options.IsDebugMode), _pipeline));
        }
    }
}