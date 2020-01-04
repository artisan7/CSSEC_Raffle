using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

public class DatabaseConnect
{
    private readonly MySqlConnection connection;
    public bool Connected { get; set; }

    public DatabaseConnect(string server, string port, string db, string user, string pass)
    {
        connection = new MySqlConnection($@"server={server};port={port};database={db};user={user};password={pass}");
        Connected = OpenConnection();
        CloseConnection();
    }

    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException e)
        {
            MessageBox.Show($"ERROR {e.Number} : {e.Message}");
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException e)
        {
            MessageBox.Show(e.Message);
            return false;
        }
    }

    // this shit emulates a SQL SELECT * statement
    public List<List<String>> Select(string tableName)
    {
        if (OpenConnection())
        {
            string query = $"SELECT * FROM {tableName}";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            List<List<String>> result = new List<List<String>>();
            
            while (reader.Read())   // save the query result as a List<List<String>>
            {
                List<String> row = new List<String>();
                for (int i = 0; i < reader.FieldCount; i++)
                    try { row.Add(reader[i].ToString()); } catch { }    // Some bullshit

                result.Add(row);
            }

            reader.Close();
            CloseConnection();

            return result;
        }

        return null;
    }

}
