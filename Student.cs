using System.Data;
using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public static class Student
{
    public const string Table = "z_student";

    public static int? ID;
    public static string? Name;
    public static double? GPA;

    public static List<(int? id, char letter,
        (int crn, string prefix, int number, int year, string semester) course)> Grades = [];

    /// <summary>
    ///     Does the student exist in the database.
    /// </summary>
    public static bool Existing;

    /// <summary>
    ///     Populates <see cref="GradeManagementSystem.Student.ID" /> with the passed ID,
    ///     and populates the <see cref="GradeManagementSystem.Student.Name" />,
    ///     <see cref="GradeManagementSystem.Student.GPA" /> and <see cref="GradeManagementSystem.Student.Grades" />
    ///     fields from the database if the student exists in the database.
    ///     If the student exists in the database, <see cref="GradeManagementSystem.Student.Existing" />
    ///     will be set to <c>true</c>, otherwise it will be <c>false</c>.
    /// </summary>
    public static void Initialize(int? id, bool getData = true)
    {
        ID = id;
        Name = null;
        GPA = null;
        Existing = false;
        Grades.Clear();

        if (!getData)
        {
            return;
        }

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

                Existing = true;
            }) || !Existing)
        {
            return;
        }

        GetGrades();
    }

    /// <summary>
    ///     Commits the student data to the database, replacing the existing row if it exists.
    ///     Also commits the grade data, see <see cref="GradeManagementSystem.Grade.Commit" />.
    /// </summary>
    /// <returns>Boolean indicating if all the commits were successful</returns>
    public static bool Commit(bool refreshData = true)
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

        if (!Grades.All(grade => Grade.Commit(grade.id, grade.letter, grade.course)))
        {
            return false;
        }

        if (!refreshData)
        {
            return true;
        }

        GetGrades();
        return true;
    }

    /// <summary>
    ///     Deletes the student from the database if it exists.
    ///     Due to foreign key constraints, will only delete if grades
    ///     reference the ID in the database; this is intended behavior.
    /// </summary>
    /// <returns>Boolean indicating if the deletion was successful</returns>
    public static bool Delete()
    {
        MySqlCommand command = new($"DELETE FROM {Table} WHERE id = @id;");
        command.Parameters.AddWithValue("@id", ID);
        if (!command.Execute(displayExecutionErrors: false))
        {
            return false;
        }

        Name = null;
        GPA = null;
        Existing = false;
        Grades.Clear();
        return true;
    }

    /// <summary>
    ///     Populates the <see cref="GradeManagementSystem.Student.Grades" /> list from the database.
    /// </summary>
    /// <returns>Boolean indicating if the get was successful</returns>
    public static bool GetGrades()
    {
        Grades.Clear();

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
                Grades.Add((reader.GetInt32("id"),
                    reader.GetChar("letter"),
                    (reader.GetInt32("crn"),
                        reader.GetString("prefix"),
                        reader.GetInt32("number"),
                        reader.GetInt32("year"),
                        reader.GetString("semester"))));
            }
        });
    }

    /// <summary>
    ///     Calculates the grade point average from the current <see cref="GradeManagementSystem.Student.Grades" /> list
    ///     and sets <see cref="GradeManagementSystem.Student.GPA" /> to the result.
    /// </summary>
    public static void CalculateGPA()
    {
        double gradePoints = 0;
        int gradeCount = 0;
        foreach ((_, char letter, _) in Grades)
        {
            gradePoints += letter switch
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
}