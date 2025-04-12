using Markdig.Extensions.Xmdl.ExecutableCode;
using Markdig.Parsers;
using Markdig.Syntax;

public class ExecutableCodeBlockParser : BlockParser
{
    private readonly string _langId;
    private readonly string _langName;
    private readonly string _prefix;

    public ExecutableCodeBlockParser(string langId, string langName)
    {
        _langId = langId.ToLowerInvariant();
        _langName = langName;
        _prefix = $"@{_langId}{{>";
        OpeningCharacters = new[] { '@' };
    }

    public override BlockState TryOpen(BlockProcessor processor)
    {
        var line = processor.Line.ToString().TrimStart();

        if (!line.StartsWith(_prefix))
            return BlockState.None;

        var block = new ExecutableCodeBlock(this)
        {
            Line = processor.LineIndex,
            Column = processor.Column,
            Span = new SourceSpan(processor.Start, processor.Line.End)
        };

        processor.NewBlocks.Push(block);

        return BlockState.Continue;
    }

    public override BlockState TryContinue(BlockProcessor processor, Block block)
    {
        var executableBlock = (ExecutableCodeBlock)block;
        var currentLine = processor.Line.ToString();

        if (currentLine.Trim() == "<}")
        {
            return BlockState.Break;
        }

        executableBlock.SourceCode.AppendLine(currentLine);
        return BlockState.Continue;
    }
}