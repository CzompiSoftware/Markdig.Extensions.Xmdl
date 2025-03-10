﻿using System;
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
        DefaultClass = "math";
    }


    public override BlockState TryOpen(BlockProcessor processor)
    {
        // Check if the current line starts with the opening marker.
        var slice = processor.Line;
        if (slice.PeekCharExtra(1) != OpeningMarker[1] ||
            slice.PeekCharExtra(2) != OpeningMarker[2] ||
            slice.PeekCharExtra(3) != OpeningMarker[3] ||
            slice.PeekCharExtra(4) != OpeningMarker[4])
        {
            // Create and push a new LuaBlock.
            var block = new ExecutableCodeBlock(this)
            {
                Column = processor.Column,
                Span = new SourceSpan()
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
        var slice = processor.Line;
        // Check if the line starts with the closing marker.
        string currentLine = slice.Text;
        if (currentLine.StartsWith(ClosingMarker))
        {
            // Finalize the block by updating its end.
            block.UpdateSpanEnd(processor.Line.End);
            // Return BreakDiscard so that the closing marker line is not included.
            return BlockState.BreakDiscard;
        }
        else
        {
            // Append the current line to the block content.
            block.Lines.Add(slice.Slice());
            return BlockState.Continue;
        }
    }
}