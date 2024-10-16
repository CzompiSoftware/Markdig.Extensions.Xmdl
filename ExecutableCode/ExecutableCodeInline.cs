using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

/// <summary>
/// A math inline element.
/// </summary>
/// <seealso cref="EmphasisInline" />
public class ExecutableCodeInline : LeafInline
{
    /// <summary>
    /// Gets or sets the delimiter character used by this code inline.
    /// </summary>
    public char Delimiter { get; set; }

    /// <summary>
    /// Gets or sets the delimiter count.
    /// </summary>
    public int DelimiterCount { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string CodeLanguage { get; internal set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string SourceCode { get; internal set; }

    /// <summary>
    /// The content as a <see cref="StringSlice"/>.
    /// </summary>
    public StringSlice Content;
}