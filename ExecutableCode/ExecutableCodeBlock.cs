using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.Xmdl.ExecutableCode;


/// <summary>
/// A C# source code inline element.
/// </summary>
/// <seealso cref="FencedCodeBlock" />
public class ExecutableCodeBlock : LeafInline
{
    public ExecutableCodeBlock()
    {
    }

    /// <summary>
    /// The trimmed source code.
    /// </summary>
    public string CodeLanguage { get; set; }
    public string SourceCode { get; set; }
}
