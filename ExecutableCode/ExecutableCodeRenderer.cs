using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public abstract class ExecutableCodeRenderer
{
    private readonly IExecutableCodeOptions _options;
    private readonly MarkdownPipeline _pipeline;

    public ExecutableCodeRenderer(IExecutableCodeOptions options, MarkdownPipeline pipeline)
    {
        _options = options;
        _pipeline = pipeline;
    }

    public abstract Task<(ExecutableCodeState, ExecutableCodeResult, List<string>)> ExecuteAsync(string script, string previousScript = null);

    public async Task<(List<string>, string)> WriteAsync(string script, bool isInline, string previousScript = null)
    {
        List<string> errors = new();
        string content = null;
        var (state, compilationContext, executeErrors) = await ExecuteAsync(script, previousScript);

        // Display errors
        if (compilationContext.Errors.Any())
        {
            foreach (var error in compilationContext.Errors)
            {
                errors.Add(Markdown.ToHtml(BuildMarkdownExceptionMessage(error), _pipeline));
            }
        }

        if (executeErrors.Any())
        {
            errors.AddRange(executeErrors);
        }

        // Render code result
        if (XmdlDocument.Instance.ReportObject != null)
        {
            if (XmdlDocument.Instance.ReportObject is HtmlReportObject htmlReportObject)
            {
                content = htmlReportObject.Html;
            }
            else if (XmdlDocument.Instance.ReportObject is MarkdownReportObject markdownReportObject)
            {
                var markdown = Markdown.ToHtml(markdownReportObject.Markdown, _pipeline);
                if (isInline)
                {
                    if (markdown.StartsWith("<p>"))
                        markdown = markdown[3..];
                    if (markdown.EndsWith("</p>\n"))
                        markdown = markdown[..^5];
                }

                content = markdown;
            }
        }
        XmdlDocument.Instance.Reset();
        return (errors, content);
    }

    internal string BuildMarkdownExceptionMessage(ExecutableCodeError codeError)
    {
        //var minLevel = (int)_options.MinimumLogLevel - 1;
        //if (minLevel < (int)codeError.Severity) return "";
        var level = codeError.Severity.ToString().ToLowerInvariant();
        var lang = codeError.CodeLanguage;
        string message = $"]>xmdlexecode-{level}<\r\n";
        message += $"#### XmdlExeCode({lang}): *{level}* severity error occurred\r\n```\r\n";
        message += codeError.Message;

        return $"{message.Replace(Environment.NewLine, $"\r\n] ")}\r\n```\r\n";
    }

    public string BuildMarkdownExceptionMessage(string codeLanguage, Exception exception, bool stackTrace)
    {
        var minLevel = (int)_options.MinimumLogLevel;
        if (minLevel > (int)LogLevel.Error) return "";

        string message = $"]>xmdlexecode-fatal<\r\n";
        message += $"#### XmdlExeCode({codeLanguage}): *fatal* severity error occurred\r\n```\r\n";
        message += exception.Message;

        if (exception is FileNotFoundException fileNotFoundException)
        {
            message += " (" + fileNotFoundException.FileName + ")";
        }
        if (stackTrace)
        {
            message += Environment.NewLine;
            message += exception.StackTrace;
        }

        return $"{message.Replace(Environment.NewLine, $"\r\n] ")}\r\n```\r\n";
    }

    internal string BuildMarkdownExceptionMessage(object codeLanguage, Exception ex, bool isDebugMode)
    {
        throw new NotImplementedException();
    }
}