using Ganss.Xss;

namespace Markdig.Extensions.Xmdl.Sanitization;

public static class HtmlContentSanitizer
{
    private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

    public static string Sanitize(string html) => Sanitizer.Sanitize(html);

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedCssProperties.Clear();

        string[] safeTags =
        [
            "html", "head", "body", "article", "section", "nav", "aside", "header", "footer", "main",
            "h1", "h2", "h3", "h4", "h5", "h6", "p", "span", "blockquote", "pre", "code",
            "b", "strong", "i", "em", "u", "s", "sub", "sup", "br", "hr", "mark", "small", "abbr",
            "ul", "ol", "li", "dl", "dt", "dd",
            "a", "img",
            "table", "thead", "tbody", "tfoot", "tr", "th", "td", "caption",
            "figure", "figcaption", "details", "summary"
        ];
        string[] safeAttributes =
        [
            "href", "src", "alt", "title", "width", "height", "colspan", "rowspan", "align", "cite"
        ];
        sanitizer.AllowedTags.UnionWith(safeTags);
        sanitizer.AllowedAttributes.UnionWith(safeAttributes);

        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.Add("http");
        sanitizer.AllowedSchemes.Add("https");
        sanitizer.AllowedSchemes.Add("mailto");

        sanitizer.AllowDataAttributes = false;

        return sanitizer;
    }
}