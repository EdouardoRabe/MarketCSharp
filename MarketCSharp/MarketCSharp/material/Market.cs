using System;
using System.Collections.Generic;
using System.Data.OleDb;
using MarketCSharp.Repository;

namespace MarketCSharp.material
{
    public class Market
    {
        public int Idmarket { get; set; }
        public double Longueur { get; set; }
        public double Largeur { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Nommarket { get; set; }

        public Market(int idmarket, double longueur, double largeur, int x, int y, string nommarket)
        {
            Idmarket = idmarket;
            Longueur = longueur;
            Largeur = largeur;
            X = x;
            Y = y;
            Nommarket = nommarket;
        }

        public static List<Market> getMarkets(OleDbConnection conn)
        {
            string query = "SELECT * FROM markets";
            OleDbCommand command = new OleDbCommand(query, conn);
            OleDbDataReader reader = command.ExecuteReader();
            List<Market> markets = new List<Market>();

            while (reader.Read())
            {
                int idmarket = reader.GetInt32(0);
                double longueur = reader.GetDouble(1);
                double largeur = reader.GetDouble(2);
                int x = reader.GetInt32(3);
                int y = reader.GetInt32(4);
                string nommarket = reader.GetString(5);

                Market market = new Market(idmarket, longueur, largeur, x, y, nommarket);
                markets.Add(market);
            }

            reader.Close();
            return markets;
        }
    }
}