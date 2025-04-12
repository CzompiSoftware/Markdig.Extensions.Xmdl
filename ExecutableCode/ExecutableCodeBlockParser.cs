using System;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Linq;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

/// <summary>
/// An inline parser for <see cref="ExecutableCodeBlock"/>.
/// </summary>
/// <seealso cref="InlineParser" />
/// <seealso cref="IPostInlineProcessor" />
public class ExecutableCodeBlockParser : BlockParser
{
    internal string OpeningMarker { get; }
    internal string ClosingMarker { get; }


    /// <summary>
    /// Gets or sets the default class to use when creating a math inline block.
    /// </summary>
    public string DefaultClass { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutableCodeBlockParser"/> class.
    /// </summary>
    public ExecutableCodeBlockParser(string langId, string langName)
    {
        OpeningMarker = "@" + langId + "{>";
        OpeningCharacters = [OpeningMarker[0]];
        ClosingMarker = "<}";
        DefaultClass = "math";
    }


    public override BlockState TryOpen(BlockProcessor processor)
    {
        // Check if the current line starts with the opening marker.
        var slice = processor.Line;
        if (slice.Match(OpeningMarker))
        {
            // Create and push a new LuaBlock.
            var block = new ExecutableCodeBlock(this)
            {
                Column = processor.Column,
                Span = new SourceSpan
                {
                    Start = slice.Start,
                    End = slice.End
                }
            };
            processor.NewBlocks.Push(block);

            // Optionally, advance the slice past the opening marker if you want to allow trailing content.
            slice.Start += OpeningMarker.Length;
            // We let the parser continue with the content lines.
            return BlockState.Continue;
        }

        return BlockState.None;
    }

    public override BlockState TryContinue(BlockProcessor processor, Block block)
    {
        var codeBlock = (ExecutableCodeBlock)block;
        // Convert current line to string.
        if (processor.Line.Length <= 0) return BlockState.Continue;
        
        string line = processor.Line.Text.Substring(processor.Line.Start, processor.Line.Length);
        
        // If the closing marker is detected, update the block's span and stop processing.
        if (line.Contains(ClosingMarker))
        {
            codeBlock.SourceCode.AppendLine(line.Replace(ClosingMarker, ""));
            block.UpdateSpanEnd(processor.Line.End);
            return BlockState.Break;
        }

        // Append the current line to the block's content, followed by a newline.
        codeBlock.SourceCode.AppendLine(line);
        return BlockState.Continue;
    }
}