using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Grade
{
    public const string Table = "zma_grade";

    /// <summary>
    /// <see cref="GradeManagementSystem.Grade.ID"/> is auto-incremented in the database,
    /// so this field is not required unless editing or deleting.
    /// </summary>
    public int ID;

    public required Student Student;
    public required char Letter;
    public required Course Course;

    /// <summary>
    /// Commits the current <see cref="GradeManagementSystem.Grade"/> instance data to the database,
    /// replacing the existing row if it exists.
    ///
    /// Also commits the linked <see cref="GradeManagementSystem.Grade.Course"/> instance,
    /// see <see cref="GradeManagementSystem.Course.Commit"/>.
    /// </summary>
    public void Commit()
    {
        Course.Commit();

        // TODO: edit this query to change existing rows with matching student_id and course_crn if they exist instead
        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (student_id, letter, course_crn)
                                    VALUES (@student_id, @letter, @course_crn)
                                    ON DUPLICATE KEY UPDATE
                                        student_id=VALUES(student_id),
                                        letter=VALUES(letter),
                                        course_crn=VALUES(course_crn);
                                    """);
        command.Parameters.AddWithValue("@student_id", Student.ID);
        command.Parameters.AddWithValue("@letter", Letter);
        command.Parameters.AddWithValue("@course_crn", Course.CRN);
        command.Execute();
    }

    /// <summary>
    /// Deletes the current <see cref="GradeManagementSystem.Grade"/> instance
    /// <see cref="GradeManagementSystem.Grade.ID"/> from the database if it exists.
    ///
    /// Also deletes the linked <see cref="GradeManagementSystem.Course"/> instance,
    /// see <see cref="GradeManagementSystem.Course.Delete"/>.
    /// </summary>
    public void Delete()
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE id=@id;");
        command.Parameters.AddWithValue("@id", ID);
        command.Execute();

        Course.Delete();
    }
}