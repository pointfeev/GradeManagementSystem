using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Course
{
    public const string Table = "zma_course";

    public required int CRN;
    public required string Prefix;
    public required int Number;
    public required int Year;
    public required string Semester;

    /// <summary>
    /// Commits the current <see cref="GradeManagementSystem.Course"/> instance data to the database,
    /// replacing the existing row if it exists.
    /// </summary>
    ///
    /// <returns>Boolean indicating if the commit was successful</returns>
    public bool Commit()
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
        command.Parameters.AddWithValue("@crn", CRN);
        command.Parameters.AddWithValue("@prefix", Prefix);
        command.Parameters.AddWithValue("@number", Number);
        command.Parameters.AddWithValue("@year", Year);
        command.Parameters.AddWithValue("@semester", Semester);
        return command.Execute();
    }

    /// <summary>
    /// Deletes the current <see cref="GradeManagementSystem.Course"/> instance
    /// <see cref="GradeManagementSystem.Course.CRN"/> from the database if it exists.
    ///
    /// Due to foreign key constraints, will only delete if no <see cref="GradeManagementSystem.Grade"/> instances
    /// reference the <see cref="GradeManagementSystem.Course.CRN"/> in the database; this is intended behavior.
    /// </summary>
    ///
    /// <returns>Boolean indicating if the deletion was successful</returns>
    public bool Delete()
    {
        MySqlCommand command = new($"DELETE IGNORE FROM {Table} WHERE crn = @crn;");
        command.Parameters.AddWithValue("@crn", CRN);
        return command.Execute();
    }
}