namespace GradeManagementSystem;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.ApplicationExit += ApplicationExit;
        Application.Run(new MainForm());
    }

    private static void ApplicationExit(object? sender, EventArgs e)
    {
        DatabaseHelper.Disconnect();
    }
}