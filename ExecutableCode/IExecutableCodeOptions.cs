using Microsoft.Extensions.Logging;

namespace Markdig.Extensions.Xmdl.ExecutableCode;

public interface IExecutableCodeOptions
{
    public bool IsDebugMode => (int)MinimumLogLevel <= (int)LogLevel.Debug;

    public LogLevel MinimumLogLevel { get; init; }

    public string WorkingDirectory { get; init; }

    public string[] Arguments { get; init; }
}
