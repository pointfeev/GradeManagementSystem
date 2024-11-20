using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public static class DatabaseHelper
{
    private const string ConnectionString =
        "server=csitmariadb.eku.edu;" +
        "port=3306;" +
        "database=csc340_db;" +
        "user=student;" +
        "password=Maroon@21?;";

    /// <summary>
    /// Executes the passed <see cref="MySql.Data.MySqlClient.MySqlCommand"/>.
    ///
    /// If a callback is passed, a <see cref="MySql.Data.MySqlClient.MySqlDataReader"/>
    /// will be executed and passed to the callback.
    /// </summary>
    public static void Execute(this MySqlCommand command, Action<MySqlDataReader>? callback = null)
    {
        MySqlConnection connection = new(ConnectionString);
        try
        {
            connection.Open();
            command.Connection = connection;
            if (callback != null)
            {
                MySqlDataReader reader = command.ExecuteReader();
                callback(reader);
                reader.Close();
            }
            else
            {
                command.ExecuteNonQuery();
            }
        }
        catch (MySqlException ex)
        {
            MessageBox.Show(ex.Message, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        connection.Close();
    }
}