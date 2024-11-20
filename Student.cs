using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public class Student
{
    public const string Table = "zma_student";

    public readonly int ID;
    public string? Name;
    public double? GPA;

    public bool Existing; // TODO: make sure this gets set to true when committed

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

    public List<Grade> GetGrades()
    {
        List<Grade> grades = [];
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
                grades.Add(grade);
            }
        });
        return grades;
    }

    public void CalculateGPA()
    {
        // TODO

        throw new NotImplementedException();
    }

    public void AddGrade()
    {
        // TODO

        throw new NotImplementedException();
    }

    public void EditGrade(int id)
    {
        // TODO

        throw new NotImplementedException();
    }

    public void DeleteGrade(int id)
    {
        // TODO

        throw new NotImplementedException();
    }

    public void ImportGrades()
    {
        // TODO

        throw new NotImplementedException();
    }

    public void PrintTranscript()
    {
        // TODO

        throw new NotImplementedException();
    }
}