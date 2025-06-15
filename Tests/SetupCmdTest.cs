using System.Text.Json;


public class SetupCmdTest
{

    private static readonly SetupCmdRunner _runner =
        new SetupCmdRunner(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Fixtures", "SetupCmd.exe"));

    public static IEnumerable<object[]> GetTestCases()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Fixtures", "DbConnections.json");
        var json = File.ReadAllText(jsonPath);
        var cases = JsonSerializer.Deserialize<List<DbConnections>>(json);
        foreach (var testCase in cases!)
        {
            yield return new object[] { testCase };
        }
    }

    [Theory(DisplayName = "Database Connection Scenarios")]
    [MemberData(nameof(GetTestCases))]
    public void TestDatabaseConnections(DbConnections testCase)
    {
        _runner.ClearLog();

        var (exitCode, output) = _runner.RunConfigureDatabaseCommand(testCase);

        if (testCase.ConnectionStatus.Equals("Success"))
        {
            Assert.Equal(0, exitCode);
        }
        else
        {
            Assert.NotEqual(0, exitCode);
            Assert.Contains(testCase.ExpectedOutput, output);

            var logContent = _runner.GetLogContents();
            Assert.Contains(testCase.ExpectedOutput, logContent);
        }
    }

    [Fact]
    public void HelpCommand_ShouldReturnUsage()
    {
        var (exitCode, output) = _runner.RunHelpCommand();

        Assert.Equal(0, exitCode);
        Assert.Contains("--databaseType", output);
        Assert.Contains("--databaseServer", output);
        Assert.Contains("--databasePort", output);
        Assert.Contains("--databaseUser", output);
        Assert.Contains("--databasePassword", output);
    }
}
