using System;
using System.Data.OleDb;

namespace MarketCSharp.Repository
{
    public class Connexion
    {
        private static string driver = "{Microsoft Access Driver (*.mdb, *.accdb)}";
        private static string databasePath = @"D:\ITU\Lecon\S4\Prog\Market\Base\base.accdb";

        public static OleDbConnection getConnexion()
        {
            try
            {
                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};Persist Security Info=False;";
                OleDbConnection connection = new OleDbConnection(connStr);
                connection.Open();
                Console.WriteLine("connexion reussit");
                return connection;
            }
            catch (OleDbException e)
            {
                Console.WriteLine($"Error connecting to database: {e.Message}");
                return null;
            }
        }
    }
}