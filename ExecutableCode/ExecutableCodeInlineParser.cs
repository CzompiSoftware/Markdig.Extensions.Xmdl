﻿using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

/// <summary>
/// An inline parser for <see cref="ExecutableCodeInline"/>.
/// </summary>
/// <seealso cref="InlineParser" />
/// <seealso cref="IPostInlineProcessor" />
public class ExecutableCodeInlineParser : InlineParser
{
    public string OpeningCharacterString { get; }
    public string LanguageName { get; }
    public string ClosingCharacterString { get; }
    public char[] ClosingCharacters { get; }

    /// <summary>
    /// Gets or sets the default class to use when creating a math inline block.
    /// </summary>
    public string DefaultClass { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutableCodeParser"/> class.
    /// </summary>
    public ExecutableCodeInlineParser(string langId, string langName)
    {
        OpeningCharacterString = $"@{langId}{{#";
        LanguageName = langName;
        //OpeningCharacters = new char[] { OpeningCharacterString[0] };
        OpeningCharacters = OpeningCharacterString.ToCharArray();
        ClosingCharacterString = "#}";
        ClosingCharacters = ClosingCharacterString.ToArray();
        DefaultClass = "math";
    }



    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        if (slice.PeekCharExtra(1) != OpeningCharacterString[1] ||
            slice.PeekCharExtra(2) != OpeningCharacterString[2] ||
            slice.PeekCharExtra(3) != OpeningCharacterString[3] ||
            slice.PeekCharExtra(4) != OpeningCharacterString[4])
        {
            return false;
        }
        string text = slice.Text[slice.Start..];
        if (string.IsNullOrEmpty(text)) return false;
        if (!text.Contains(OpeningCharacterString)) return false;
        if (!text.Contains(ClosingCharacterString)) return false;

        var start = text.IndexOf(OpeningCharacterString);
        var end = text.IndexOf(ClosingCharacterString) + ClosingCharacterString.Length;

        if (start == -1 || end == -1) return false;
        string sourceCode = text[start..end].Replace(OpeningCharacterString, "").Replace(ClosingCharacterString, "");
        int length = sourceCode.Length;
        for (int i = 0; i < $"{OpeningCharacterString}{sourceCode}{ClosingCharacterString}".Length; i++)
        {
            slice.NextChar();
        }
        processor.Inline = new ExecutableCodeInline() { SourceCode = sourceCode };
        processor.Inline.Span = new SourceSpan() { Start = processor.GetSourcePosition(slice.Start, out int line, out int column) };
        processor.Inline.Line = line;
        processor.Inline.Span.End = processor.Inline.Span.Start + (start - end - 1);
        return true;
    }
}