using System.Diagnostics;
using System.Text;
public class SetupCmdRunner
{
    private readonly string _exePath;
    private readonly string _logFilePath;

    public SetupCmdRunner(string exePath, string logFilePath = "SetupCmd.log")
    {
        _exePath = exePath;
        _logFilePath = logFilePath;
    }

    public (int ExitCode, string Output) RunConfigureDatabaseCommand(DbConnections testCase)
    {
        var sb = new StringBuilder("configureDatabaseConnection");

        void AddArg(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                sb.Append($" --{name}={value}");
        }

        AddArg("databaseType", testCase.DatabaseType);
        AddArg("databaseServer", testCase.Server);
        AddArg("databasePort", testCase.Port);
        AddArg("databaseUser", testCase.User);
        AddArg("databasePassword", testCase.Password);
        AddArg("databaseName", testCase.DatabaseName);

        return RunProcess(sb.ToString());
    }

    public (int ExitCode, string Output) RunHelpCommand()
    {
        return RunProcess("configureDatabaseConnection --help");
    }

    public string GetLogContents()
    {
        return File.Exists(_logFilePath) ? File.ReadAllText(_logFilePath) : string.Empty;
    }

    public void ClearLog()
    {
        if (File.Exists(_logFilePath))
            File.Delete(_logFilePath);
    }

    private (int ExitCode, string Output) RunProcess(string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _exePath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
        process.WaitForExit();
        return (process.ExitCode, output);
    }
}
