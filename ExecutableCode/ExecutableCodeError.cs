using Microsoft.Extensions.Logging;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public class ExecutableCodeError
{
    public ExecutableCodeError(string severity, object codeLanguage, string message)
    {
        Severity = severity;
        CodeLanguage = codeLanguage;
        Message = message;
    }

    public string Severity { get; internal set; }
    public object CodeLanguage { get; internal set; }
    public string Message { get; internal set; }
}