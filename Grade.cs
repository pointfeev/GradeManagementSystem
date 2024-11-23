using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Grade
{
    public const string Table = "zma_grade";

    public static readonly HashSet<char> ValidLetters = ['A', 'B', 'C', 'D', 'F'];

    /// <summary>
    ///     <see cref="GradeManagementSystem.Grade.ID" /> is auto-incremented in the database,
    ///     so this field is not required unless editing or deleting.
    /// </summary>
    public int ID;

    private readonly Student _student = null!;

    public required Student Student
    {
        get => _student;
        init
        {
            _student = value;
            _student.Grades.Add(this);
        }
    }

    private char _letter;

    public required char Letter
    {
        get => _letter;
        set
        {
            if (_letter == value)
            {
                return;
            }

            _letter = value;
            NeedsCommit = true;
        }
    }

    public required Course Course;

    public bool NeedsCommit;

    /// <summary>
    ///     Commits the current <see cref="GradeManagementSystem.Grade" /> instance data to the database,
    ///     replacing the existing row if it exists.
    ///     Also commits the linked <see cref="GradeManagementSystem.Grade.Course" /> instance,
    ///     see <see cref="GradeManagementSystem.Course.Commit" />.
    /// </summary>
    /// <returns>Boolean indicating if all the commits were successful</returns>
    public bool Commit()
    {
        if (!Course.Commit())
        {
            return false;
        }

        if (!NeedsCommit)
        {
            return true;
        }

        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (id, student_id, letter, course_crn)
                                    VALUES (@id, @student_id, @letter, @course_crn)
                                    ON DUPLICATE KEY UPDATE
                                        student_id = VALUES(student_id),
                                        letter = VALUES(letter),
                                        course_crn = VALUES(course_crn);
                                    """);
        command.Parameters.AddWithValue("@id", ID);
        command.Parameters.AddWithValue("@student_id", Student.ID);
        command.Parameters.AddWithValue("@letter", Letter);
        command.Parameters.AddWithValue("@course_crn", Course.CRN);
        if (!command.Execute())
        {
            return false;
        }

        ID = (int)command.LastInsertedId;

        NeedsCommit = false;
        return true;
    }

    /// <summary>
    ///     Deletes the current <see cref="GradeManagementSystem.Grade" /> instance
    ///     <see cref="GradeManagementSystem.Grade.ID" /> from the database if it exists.
    ///     Also deletes the linked <see cref="GradeManagementSystem.Course" /> instance,
    ///     see <see cref="GradeManagementSystem.Course.Delete" />.
    /// </summary>
    /// <returns>Boolean indicating if all the deletions were successful</returns>
    public bool Delete()
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE id = @id;");
        command.Parameters.AddWithValue("@id", ID);
        if (!command.Execute() || !Course.Delete())
        {
            return false;
        }

        Student.Grades.Remove(this);
        return true;
    }
}