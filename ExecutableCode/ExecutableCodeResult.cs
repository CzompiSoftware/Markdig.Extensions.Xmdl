using System.Collections.Generic;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public class ExecutableCodeResult
{
    public List<ExecutableCodeError> Errors { get; init; }
}