using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace MarketCSharp.tool
{
    public class Location
    {
        private int _idLocation;
        private int _idBox;
        private int _idOwner;
        private DateTime _debut;
        private DateTime? _fin;

        public Location(int idLocation, int idBox, int idOwner, DateTime debut, DateTime? fin)
        {
            _idLocation = idLocation;
            _idBox = idBox;
            _idOwner = idOwner;
            _debut = debut;
            _fin = fin;
        }

        public int GetIdLocation()
        {
            return _idLocation;
        }

        public void SetIdLocation(int value)
        {
            _idLocation = value;
        }

        public int GetIdBox()
        {
            return _idBox;
        }

        public void SetIdBox(int value)
        {
            _idBox = value;
        }

        public int GetIdOwner()
        {
            return _idOwner;
        }

        public void SetIdOwner(int value)
        {
            _idOwner = value;
        }

        public DateTime GetDebut()
        {
            return _debut;
        }

        public void SetDebut(DateTime value)
        {
            _debut = value;
        }

        public DateTime? GetFin()
        {
            return _fin;
        }

        public void SetFin(DateTime? value)
        {
            _fin = value;
        }

        public static Location GetLocationByBoxAndYearMonth(OleDbConnection conn, int idBox, string yearMonth)
        {
            string query = @"
                SELECT * FROM locations 
                WHERE idbox = ? AND 
                (
                    (? BETWEEN FORMAT(debut, 'yyyy-MM') AND FORMAT(fin, 'yyyy-MM') AND fin IS NOT NULL) 
                    OR 
                    (fin IS NULL AND FORMAT(debut, 'yyyy-MM') <= ?)
                )
            ";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idBox", idBox);
                cmd.Parameters.AddWithValue("@yearMonth", yearMonth);
                cmd.Parameters.AddWithValue("@yearMonth", yearMonth);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Location(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        );
                    }
                }
            }
            return null;
        }

        public static List<Location> GetLocationByOwner(OleDbConnection conn, int idOwner)
        {
            List<Location> locations = new List<Location>();
            string query = "SELECT * FROM locations WHERE idowner = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idOwner", idOwner);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add(new Location(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        ));
                    }
                }
            }
            return locations;
        }
    }
}