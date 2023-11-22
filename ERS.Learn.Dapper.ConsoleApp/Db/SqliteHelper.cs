using Dapper;
using ERS.Learn.Dapper.ConsoleApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERS.Learn.Dapper.ConsoleApp.Db
{
    public class SqliteHelper
    {
        /* https://www.learndapper.com/parameters */

        //private readonly string _fullPathExe = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _fullDatabaseFilePath;

        public SqliteHelper(string databaseName)
        {
            _fullDatabaseFilePath = databaseName + ExtractFileExtension(databaseName);

            CreateDatabase();
            SeedDatabase();
        }

        public Foo? GetFooById(int id)
        {
            using (var conn = new SQLiteConnection("Data Source=" + _fullDatabaseFilePath))
            {
                conn.Open();

                var commandText = "SELECT Id, Description FROM Foo WHERE Id = @Id";
                var parameter = new { Id = id };


                var foo = conn.QueryFirst<Foo>(
                    commandText,
                    parameter
                );

                if (foo is null)
                    return null;

                return foo;
            }
        }

        private void SeedDatabase()
        {
            if (SeedExecuted())
                return;
            
            using (var conn  = new SQLiteConnection("Data Source=" + _fullDatabaseFilePath))
            { 
                conn.Open();

                var command = new SQLiteCommand(conn);

                command.CommandText = "CREATE TABLE Foo (Id INT, Description VARCHAR(100))";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Foo (Id, Description) VALUES (1, 'One'), (2, 'Two')";
                command.ExecuteNonQuery();

                Console.WriteLine("Table 'Foo' filled.");
            }
        }

        private bool SeedExecuted()
        {
            using (var conn = new SQLiteConnection("Data Source=" + _fullDatabaseFilePath))
            {
                conn.Open();

                var command = new SQLiteCommand(conn);

                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table'";
                var qtt = command.ExecuteScalar();

                return qtt is not null && qtt != DBNull.Value && ((long)qtt) > 0;
            }
        }

        private void CreateDatabase()
        {
            try
            {
                if (File.Exists(_fullDatabaseFilePath))
                {
                    Console.WriteLine("Database already exists.");
                    return;
                }

                SQLiteConnection.CreateFile(_fullDatabaseFilePath);
                Console.WriteLine("Create database sucessufully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create database failed. Msg: " + ex.Message);
            }
        }

        private static string ExtractFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(ext))
                ext = ".db";

            return ext;
        }
    }
}
