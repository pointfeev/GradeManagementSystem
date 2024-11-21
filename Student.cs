using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Student
{
    public const string Table = "zma_student";

    public readonly int ID;
    public string? Name;
    public double? GPA;

    public readonly List<Grade> Grades = [];

    /// <summary>
    /// Does the current <see cref="GradeManagementSystem.Student"/> instance
    /// <see cref="GradeManagementSystem.Student.ID"/> exist in the database.
    /// </summary>
    public bool Existing;

    /// <summary>
    /// Creates a new <see cref="GradeManagementSystem.Student"/> instance
    /// from the passed <see cref="GradeManagementSystem.Student.ID"/>,
    /// and populates the <see cref="GradeManagementSystem.Student.Name"/>,
    /// <see cref="GradeManagementSystem.Student.GPA"/> and <see cref="GradeManagementSystem.Student.Grades"/>
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
        if (!command.Execute(reader =>
            {
                if (!reader.Read())
                {
                    return;
                }

                Existing = true;

                Name = reader.GetString("name");
                GPA = reader.GetDouble("gpa");
            }) || !Existing)
        {
            return;
        }

        GetGrades();
    }

    /// <summary>
    /// Commits the current <see cref="GradeManagementSystem.Student"/> instance data to the database,
    /// replacing the existing row if it exists.
    ///
    /// Also commits the linked <see cref="GradeManagementSystem.Student.Grades"/> instances,
    /// see <see cref="GradeManagementSystem.Grade.Commit"/>.
    /// </summary>
    ///
    /// <returns>Boolean indicating if all the commits were successful</returns>
    public bool Commit()
    {
        MySqlCommand command = new($"""
                                    INSERT INTO {Table} (id, name, gpa)
                                    VALUES (@id, @name, @gpa)
                                    ON DUPLICATE KEY UPDATE
                                        name = VALUES(name),
                                        gpa = VALUES(gpa);
                                    """);
        command.Parameters.AddWithValue("@id", ID);
        command.Parameters.AddWithValue("@name", Name);
        command.Parameters.AddWithValue("@gpa", GPA);
        if (!command.Execute())
        {
            return false;
        }

        Existing = true;

        return Grades.Aggregate(true, (current, grade) => current && grade.Commit());
    }

    /// <summary>
    /// Populates the <see cref="GradeManagementSystem.Student.Grades"/> list from the database.
    /// </summary>
    ///
    /// <returns>Boolean indicating if the get was successful</returns>
    private bool GetGrades()
    {
        MySqlCommand command = new($"""
                                    SELECT id, letter, crn, prefix, number, year, semester
                                    FROM {Grade.Table} JOIN {Course.Table} ON course_crn = crn
                                    WHERE student_id = @id;
                                    """);
        command.Parameters.AddWithValue("@id", ID);
        return command.Execute(reader =>
        {
            while (reader.Read())
            {
                _ = new Grade
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
            }
        });
    }

    /// <summary>
    /// Calculates the grade point average from the current <see cref="GradeManagementSystem.Student.Grades"/> list
    /// and sets <see cref="GradeManagementSystem.Student.GPA"/> to the result.
    /// </summary>
    public void CalculateGPA()
    {
        double gradePoints = 0;
        int gradeCount = 0;
        foreach (Grade grade in Grades)
        {
            gradePoints += grade.Letter switch
            {
                'A' => 4.0,
                'B' => 3.0,
                'C' => 2.0,
                'D' => 1.0,
                _ => 0.0
            };
            gradeCount++;
        }

        GPA = gradeCount == 0 ? 0 : gradePoints / gradeCount;
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