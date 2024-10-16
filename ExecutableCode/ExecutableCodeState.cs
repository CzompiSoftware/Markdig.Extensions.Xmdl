using System;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public class ExecutableCodeState
{
    public Exception Exception { get; set; }
    public object ReturnValue { get; set; }
}