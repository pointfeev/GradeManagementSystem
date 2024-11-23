using System.Data;
using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Student
{
    public const string Table = "zma_student";

    public readonly int ID;

    private string? _name;

    public string? Name
    {
        get => _name;
        set
        {
            if (_name == value)
            {
                return;
            }

            _name = value;
            NeedsCommit = true;
        }
    }

    private double? _gpa;

    public double? GPA
    {
        get => _gpa;
        set
        {
            if (_gpa == value)
            {
                return;
            }

            _gpa = value;
            NeedsCommit = true;
        }
    }

    public bool NeedsCommit;

    public readonly List<Grade> Grades = [];

    /// <summary>
    ///     Does the current <see cref="GradeManagementSystem.Student" /> instance
    ///     <see cref="GradeManagementSystem.Student.ID" /> exist in the database.
    /// </summary>
    public bool Existing;

    /// <summary>
    ///     Creates a new <see cref="GradeManagementSystem.Student" /> instance
    ///     from the passed <see cref="GradeManagementSystem.Student.ID" />,
    ///     and populates the <see cref="GradeManagementSystem.Student.Name" />,
    ///     <see cref="GradeManagementSystem.Student.GPA" /> and <see cref="GradeManagementSystem.Student.Grades" />
    ///     fields from the database if the student exists in the database.
    ///     If the student exists in the database, <see cref="GradeManagementSystem.Student.Existing" />
    ///     will be set to <c>true</c>, otherwise it will be <c>false</c>.
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

                if (reader.GetValue("name").GetType() != typeof(DBNull))
                {
                    Name = reader.GetString("name");
                }

                if (reader.GetValue("gpa").GetType() != typeof(DBNull))
                {
                    GPA = reader.GetDouble("gpa");
                }

                NeedsCommit = false;
                Existing = true;
            }) || !Existing)
        {
            return;
        }

        GetGrades();
    }

    /// <summary>
    ///     Commits the current <see cref="GradeManagementSystem.Student" /> instance data to the database,
    ///     replacing the existing row if it exists.
    ///     Also commits the linked <see cref="GradeManagementSystem.Student.Grades" /> instances,
    ///     see <see cref="GradeManagementSystem.Grade.Commit" />.
    /// </summary>
    /// <returns>Boolean indicating if all the commits were successful</returns>
    public bool Commit()
    {
        if (NeedsCommit)
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

            NeedsCommit = false;
        }

        Existing = true;

        return Grades.Aggregate(true, (current, grade) => current && grade.Commit());
    }

    /// <summary>
    ///     Deletes the current <see cref="GradeManagementSystem.Student" /> instance
    ///     <see cref="GradeManagementSystem.Student.ID" /> from the database if it exists.
    ///     Due to foreign key constraints, will only delete if no <see cref="GradeManagementSystem.Grade" /> instances
    ///     reference the <see cref="GradeManagementSystem.Student.ID" /> in the database; this is intended behavior.
    /// </summary>
    /// <returns>Boolean indicating if the deletion was successful</returns>
    public bool Delete()
    {
        MySqlCommand command = new($"DELETE IGNORE FROM {Table} WHERE id = @id;");
        command.Parameters.AddWithValue("@id", ID);
        return command.Execute();
    }

    /// <summary>
    ///     Populates the <see cref="GradeManagementSystem.Student.Grades" /> list from the database.
    /// </summary>
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
                        Semester = reader.GetString("semester"),
                        NeedsCommit = false
                    },
                    NeedsCommit = false
                };
            }
        });
    }

    /// <summary>
    ///     Calculates the grade point average from the current <see cref="GradeManagementSystem.Student.Grades" /> list
    ///     and sets <see cref="GradeManagementSystem.Student.GPA" /> to the result.
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
    ///     TODO
    /// </summary>
    public void PrintTranscript()
    {
        // TODO
        throw new NotImplementedException();
    }
}