using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace MarketCSharp.tool
{
    public class Owner
    {
        private int _idOwner;
        private string _name;
        private string _firstName;

        public Owner(int idOwner, string name, string firstName)
        {
            _idOwner = idOwner;
            _name = name;
            _firstName = firstName;
        }

        public int GetIdOwner()
        {
            return _idOwner;
        }

        public void SetIdOwner(int value)
        {
            _idOwner = value;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string value)
        {
            _name = value;
        }

        public string GetFirstName()
        {
            return _firstName;
        }

        public void SetFirstName(string value)
        {
            _firstName = value;
        }

        public static Owner GetOwnerById(OleDbConnection conn, int idOwner)
        {
            string query = "SELECT * FROM owners WHERE idowner = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idOwner", idOwner);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Owner(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                        );
                    }
                }
            }
            return null;
        }

        public static List<Owner> GetOwners(OleDbConnection conn)
        {
            List<Owner> owners = new List<Owner>();
            string query = "SELECT * FROM owners";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        owners.Add(new Owner(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                        ));
                    }
                }
            }
            return owners;
        }
    }
}