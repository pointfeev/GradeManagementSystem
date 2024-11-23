using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Course
{
    public const string Table = "zma_course";

    private int _crn;

    public required int CRN
    {
        get => _crn;
        set
        {
            if (_crn == value)
            {
                return;
            }

            _crn = value;
            NeedsCommit = true;
        }
    }

    private string _prefix;

    public required string Prefix
    {
        get => _prefix;
        set
        {
            if (_prefix == value)
            {
                return;
            }

            _prefix = value;
            NeedsCommit = true;
        }
    }

    private int _number;

    public required int Number
    {
        get => _number;
        set
        {
            if (_number == value)
            {
                return;
            }

            _number = value;
            NeedsCommit = true;
        }
    }

    private int _year;

    public required int Year
    {
        get => _year;
        set
        {
            if (_year == value)
            {
                return;
            }

            _year = value;
            NeedsCommit = true;
        }
    }

    private string _semester;

    public required string Semester
    {
        get => _semester;
        set
        {
            if (_semester == value)
            {
                return;
            }

            _semester = value;
            NeedsCommit = true;
        }
    }

    public bool NeedsCommit;

    /// <summary>
    ///     Commits the current <see cref="GradeManagementSystem.Course" /> instance data to the database,
    ///     replacing the existing row if it exists.
    /// </summary>
    /// <returns>Boolean indicating if the commit was successful</returns>
    public bool Commit()
    {
        if (!NeedsCommit)
        {
            return true;
        }

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
        if (!command.Execute())
        {
            return false;
        }

        NeedsCommit = false;
        return true;
    }

    /// <summary>
    ///     Deletes the current <see cref="GradeManagementSystem.Course" /> instance
    ///     <see cref="GradeManagementSystem.Course.CRN" /> from the database if it exists.
    ///     Due to foreign key constraints, will only delete if no <see cref="GradeManagementSystem.Grade" /> instances
    ///     reference the <see cref="GradeManagementSystem.Course.CRN" /> in the database; this is intended behavior.
    /// </summary>
    /// <returns>Boolean indicating if the deletion was successful</returns>
    public bool Delete()
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE crn = @crn;");
        command.Parameters.AddWithValue("@crn", CRN);
        return command.Execute(displayExecutionErrors: false);
    }
}