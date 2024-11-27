using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public static class Course
{
    public const string Table = "z_course";

    /// <summary>
    ///     Commits the course data to the database, replacing the existing row if it exists.
    /// </summary>
    /// <returns>Boolean indicating if the commit was successful</returns>
    public static bool Commit(int crn, string prefix, int number, int year, string semester)
    {
        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (crn, prefix, number, year, semester)
                                    VALUES (@crn, @prefix, @number, @year, @semester)
                                    ON DUPLICATE KEY UPDATE
                                        prefix = VALUES(prefix),
                                        number = VALUES(number),
                                        year = VALUES(year),
                                        semester = VALUES(semester);
                                    """);
        command.Parameters.AddWithValue("@crn", crn);
        command.Parameters.AddWithValue("@prefix", prefix);
        command.Parameters.AddWithValue("@number", number);
        command.Parameters.AddWithValue("@year", year);
        command.Parameters.AddWithValue("@semester", semester);
        return command.Execute();
    }

    /// <summary>
    ///     Deletes the course from the database if it exists.
    ///     Due to foreign key constraints, will only delete if no grades
    ///     reference the CRN in the database; this is intended behavior.
    /// </summary>
    /// <returns>Boolean indicating if the deletion was successful</returns>
    public static bool Delete(int crn)
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE crn = @crn;");
        command.Parameters.AddWithValue("@crn", crn);
        return command.Execute(displayExecutionErrors: false);
    }
}