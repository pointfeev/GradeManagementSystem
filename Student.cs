using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Student
{
    public const string Table = "zma_student";

    public readonly int ID;
    public string? Name;
    public double? GPA;

    /// <summary>
    /// This array is intended to be used for committing new <see cref="GradeManagementSystem.Grade"/>
    /// and <see cref="GradeManagementSystem.Course"/> instances to the database
    /// with <see cref="GradeManagementSystem.Student.Commit"/>.
    ///
    /// Use <see cref="GradeManagementSystem.Student.GetGrades"/> to get a list of grades
    /// freshly populated from the database.
    /// </summary>
    public readonly List<Grade> Grades = [];

    /// <summary>
    /// Does the current <see cref="GradeManagementSystem.Student"/> instance
    /// <see cref="GradeManagementSystem.Student.ID"/> exist in the database.
    /// </summary>
    public bool Existing;

    /// <summary>
    /// Creates a new <see cref="GradeManagementSystem.Student"/> instance
    /// from the passed <see cref="GradeManagementSystem.Student.ID"/>,
    /// and populates the <see cref="GradeManagementSystem.Student.Name"/>
    /// and <see cref="GradeManagementSystem.Student.GPA"/>
    /// fields from the database if the student exists in the database.
    ///
    /// If the student exists in the database, <see cref="GradeManagementSystem.Student.Existing"/>
    /// will be set to <c>true</c>, otherwise it will be <c>false</c>.
    /// </summary>
    public Student(int id)
    {
        ID = id;

        MySqlCommand command = new($@"SELECT name, gpa FROM {Table} WHERE id = @id;");
        command.Parameters.AddWithValue("@id", ID);
        command.Execute(reader =>
        {
            if (!reader.Read())
            {
                return;
            }

            Existing = true;

            Name = reader.GetString("name");
            GPA = reader.GetDouble("gpa");
        });
    }

    /// <summary>
    /// Commits the current <see cref="GradeManagementSystem.Student"/> instance data to the database,
    /// replacing the existing row if it exists.
    ///
    /// Also commits the linked <see cref="GradeManagementSystem.Student.Grades"/> instances,
    /// see <see cref="GradeManagementSystem.Grade.Commit"/>.
    /// </summary>
    public void Commit()
    {
        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (id, name, gpa)
                                    VALUES (@id, @name, @gpa)
                                    ON DUPLICATE KEY UPDATE
                                        name=VALUES(name),
                                        gpa=VALUES(gpa);
                                    """);
        command.Parameters.AddWithValue("@id", ID);
        command.Parameters.AddWithValue("@name", Name);
        command.Parameters.AddWithValue("@gpa", GPA);
        command.Execute();

        Existing = true;

        foreach (Grade grade in Grades)
        {
            grade.Commit();
        }
    }

    /// <summary>
    /// Clears the <see cref="GradeManagementSystem.Student.Grades"/> list and populates it from the database.
    /// </summary>
    ///
    /// <returns>The populated <see cref="GradeManagementSystem.Student.Grades"/> list.</returns>
    public List<Grade> GetGrades()
    {
        Grades.Clear();
        MySqlCommand command = new($"""
                                    SELECT id, letter, crn, prefix, number, year, semester
                                    FROM {Grade.Table} JOIN {Course.Table} ON course_crn = crn
                                    WHERE student_id = @id;
                                    """);
        command.Parameters.AddWithValue("@id", ID);
        command.Execute(reader =>
        {
            while (reader.Read())
            {
                Grade grade = new()
                {
                    Student = this,
                    ID = reader.GetInt32("id"),
                    Letter = reader.GetChar("letter"),
                    Course = new()
                    {
                        CRN = reader.GetInt32("crn"),
                        Prefix = reader.GetString("prefix"),
                        Number = reader.GetInt32("number"),
                        Year = reader.GetInt32("year"),
                        Semester = reader.GetString("semester")
                    }
                };
                Grades.Add(grade);
            }
        });
        return Grades;
    }

    /// <summary>
    /// TODO
    /// </summary>
    public void CalculateGPA()
    {
        // TODO

        throw new NotImplementedException();
    }

    /// <summary>
    /// TODO
    /// </summary>
    public void PrintTranscript()
    {
        // TODO

        throw new NotImplementedException();
    }
}