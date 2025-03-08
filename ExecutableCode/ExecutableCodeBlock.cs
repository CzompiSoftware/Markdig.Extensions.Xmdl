using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.Xmdl.ExecutableCode;


/// <summary>
/// A C# source code inline element.
/// </summary>
public class ExecutableCodeBlock : CodeBlock, IFencedBlock
{
    public ExecutableCodeBlock(BlockParser parser) : base(parser)
    {
        
    }

    /// <summary>
    /// The trimmed source code.
    /// </summary>
    public string CodeLanguage { get; set; }
    public string SourceCode { get; set; }
    public MarkdownParserContext Context { get; set; }
    
    
    public char FencedChar { get; set; }
    public int OpeningFencedCharCount { get; set; }
    public StringSlice TriviaAfterFencedChar { get; set; }
    public string Info { get; set; }
    public StringSlice UnescapedInfo { get; set; }
    public StringSlice TriviaAfterInfo { get; set; }
    public string Arguments { get; set; }
    public StringSlice UnescapedArguments { get; set; }
    public StringSlice TriviaAfterArguments { get; set; }
    public NewLine InfoNewLine { get; set; }
    public StringSlice TriviaBeforeClosingFence { get; set; }
    public int ClosingFencedCharCount { get; set; }
}