using ERS.Learn.Dapper.ConsoleApp.Db;
using System;

namespace ERS.Learn.Dapper.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sqliteConn = new SqliteHelper("sqliteTest");

            var foo1 = sqliteConn.GetFooById(1);

            if (foo1 is not null)
                Console.WriteLine(foo1.Description);

            var foo2 = sqliteConn.GetFooById(2);

            if (foo2 is not null)
                Console.WriteLine(foo2.Description);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}