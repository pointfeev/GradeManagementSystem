using System.Data;
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

    private static MySqlConnection? _connection;

    public static bool Connect()
    {
        if (_connection?.State is ConnectionState.Open)
        {
            return true;
        }

        Disconnect();

        bool success = true;
        _connection = new(ConnectionString);
        try
        {
            _connection.Open();
        }
        catch (MySqlException ex)
        {
            MainForm.DisplayError(ex.Message);
            success = false;
        }

        return success;
    }

    public static void Disconnect()
    {
        _connection?.Close();
        _connection?.Dispose();
        _connection = null;
    }

    /// <summary>
    /// Executes the passed <see cref="MySql.Data.MySqlClient.MySqlCommand"/>.
    ///
    /// If a callback is passed, a <see cref="MySql.Data.MySqlClient.MySqlDataReader"/>
    /// will be executed and passed to the callback.
    /// </summary>
    ///
    /// <returns>Boolean indicating if the command ran successfully</returns>
    public static bool Execute(this MySqlCommand command, Action<MySqlDataReader>? callback = null)
    {
        if (!Connect())
        {
            return false;
        }

        bool success = true;
        try
        {
            command.Connection = _connection;
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
            MainForm.DisplayError(ex.Message);
            success = false;
        }

        return success;
    }
}