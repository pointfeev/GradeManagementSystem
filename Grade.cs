namespace GradeManagementSystem;

public class Grade
{
    public const string Table = "zma_grade";

    public int ID;
    public char Letter;
    public Course Course;

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

        // TODO: commit the current instance data to the database here, replacing existing data
        throw new NotImplementedException();
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
        // TODO: delete the current instance ID from the database here
        throw new NotImplementedException();

        Course.Delete();
    }
}