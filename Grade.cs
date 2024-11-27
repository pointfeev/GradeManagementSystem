using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public static class Grade
{
    public const string Table = "z_grade";

    public static readonly HashSet<char> ValidLetters = ['A', 'B', 'C', 'D', 'F'];

    /// <summary>
    ///     Commits the grade data to the database, replacing the existing row if it exists.
    ///     Also commits the passed course data, see <see cref="GradeManagementSystem.Course.Commit" />.
    /// </summary>
    /// <returns>Boolean indicating if all the commits were successful</returns>
    public static bool Commit(int? id, char letter,
        (int crn, string prefix, int number, int year, string semester) course)
    {
        if (!Course.Commit(course.crn, course.prefix, course.number, course.year, course.semester))
        {
            return false;
        }

        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (id, student_id, letter, course_crn)
                                    VALUES (@id, @student_id, @letter, @course_crn)
                                    ON DUPLICATE KEY UPDATE
                                        student_id = VALUES(student_id),
                                        letter = VALUES(letter),
                                        course_crn = VALUES(course_crn);
                                    """);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@student_id", Student.ID);
        command.Parameters.AddWithValue("@letter", letter);
        command.Parameters.AddWithValue("@course_crn", course.crn);
        return command.Execute();
    }

    /// <summary>
    ///     Deletes the grade from the database if it exists.
    ///     Also deletes the passed course, see <see cref="GradeManagementSystem.Course.Delete" />.
    /// </summary>
    /// <returns>Boolean indicating if all the deletions were successful</returns>
    public static bool Delete(int id, int crn)
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE id = @id;");
        command.Parameters.AddWithValue("@id", id);
        if (!command.Execute())
        {
            return false;
        }

        Course.Delete(crn);

        return true;
    }
}