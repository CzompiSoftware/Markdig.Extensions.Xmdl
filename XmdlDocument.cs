using System;

namespace Markdig.Extensions.Xmdl;

public class XmdlDocument
{
    private static readonly Lazy<XmdlDocument> lazy;

    public static XmdlDocument Instance => lazy.Value;

#nullable enable
    /// <summary>
    /// Gets the current uri of the request, that invoked this instance
    /// </summary>
    public Uri? CurrentUri { get; internal set; } = null;
#nullable restore

    private XmdlDocument() { }

    static XmdlDocument()
    {
        lazy = new Lazy<XmdlDocument>(() => new XmdlDocument());
    }

    public object ReportObject { get; private set; }

    public void InsertHtml(string html)
    {
        ReportObject = new HtmlReportObject() { Html = html };
    }

    public void InsertMarkdown(string markdown)
    {
        ReportObject = new MarkdownReportObject() { Markdown = markdown };
    }

    public void Reset()
    {
        ReportObject = null;
    }
}