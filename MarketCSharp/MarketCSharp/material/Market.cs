using System;
using System.Collections.Generic;
using System.Data.OleDb;
using MarketCSharp.tool;

namespace MarketCSharp.material
{
    public class Market
    {
        private int _idMarket;
        private double _longueur;
        private double _largeur;
        private int _x;
        private int _y;
        private string _nomMarket;

        public Market(int idMarket, double longueur, double largeur, int x, int y, string nomMarket)
        {
            _idMarket = idMarket;
            _longueur = longueur;
            _largeur = largeur;
            _x = x;
            _y = y;
            _nomMarket = nomMarket;
        }

        public int GetIdMarket()
        {
            return _idMarket;
        }

        public void SetIdMarket(int value)
        {
            _idMarket = value;
        }

        public double GetLongueur()
        {
            return _longueur;
        }

        public void SetLongueur(double value)
        {
            _longueur = value;
        }

        public double GetLargeur()
        {
            return _largeur;
        }

        public void SetLargeur(double value)
        {
            _largeur = value;
        }

        public int GetX()
        {
            return _x;
        }

        public void SetX(int value)
        {
            _x = value;
        }

        public int GetY()
        {
            return _y;
        }

        public void SetY(int value)
        {
            _y = value;
        }

        public string GetNomMarket()
        {
            return _nomMarket;
        }

        public void SetNomMarket(string value)
        {
            _nomMarket = value;
        }

        public static List<Market> GetMarkets(OleDbConnection conn)
        {
            List<Market> markets = new List<Market>();
            string query = "SELECT * FROM markets";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idMarket = reader.GetInt32(0);
                        double longueur = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                        double largeur = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                        int x = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        int y = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        string nomMarket = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);

                        markets.Add(new Market(idMarket, longueur, largeur, x, y, nomMarket));
                    }
                }
            }
            return markets;
        }

        public List<Box> GetBoxs(OleDbConnection conn)
        {
            List<Box> boxs = new List<Box>();
            string query = "SELECT * FROM boxs WHERE idmarket = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idMarket", _idMarket);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boxs.Add(new Box(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDouble(3),
                            reader.GetDouble(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6))
                        );
                    }
                }
            }
            return boxs;
        }

        public static void InsertMarket(OleDbConnection conn, double longueur, double largeur, int x, int y, string nomMarket)
        {
            string query = @"
                INSERT INTO markets (longueur, largeur, x, y, nommarket) 
                VALUES (?, ?, ?, ?, ?)
            ";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@longueur", longueur);
                cmd.Parameters.AddWithValue("@largeur", largeur);
                cmd.Parameters.AddWithValue("@x", x);
                cmd.Parameters.AddWithValue("@y", y);
                cmd.Parameters.AddWithValue("@nomMarket", nomMarket);
                cmd.ExecuteNonQuery();
            }
        }
    }
}