namespace GradeManagementSystem;

public class Course
{
    public const string Table = "zma_course";

    public int CRN;
    public string Prefix;
    public int Number;
    public int Year;
    public string Semester;

    /// <summary>
    /// Commits the current <see cref="GradeManagementSystem.Course"/> instance data to the database,
    /// replacing the existing row if it exists.
    /// </summary>
    public void Commit()
    {
        // TODO: commit the current instance data to the database here, replacing existing data
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes the current <see cref="GradeManagementSystem.Course"/> instance
    /// <see cref="GradeManagementSystem.Course.CRN"/> from the database if it exists.
    ///
    /// Will only delete if no <see cref="GradeManagementSystem.Grade"/> instances
    /// reference the <see cref="GradeManagementSystem.Course.CRN"/> in the database.
    /// </summary>
    public void Delete()
    {
        // TODO: delete the current instance ID from the database here
        //       ONLY if no grades reference it's CRN in the DB
        throw new NotImplementedException();
    }
}