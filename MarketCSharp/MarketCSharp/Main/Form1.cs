using System;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using MarketCSharp.Repository;
using MarketCSharp.material;
using System.Collections.Generic;

namespace MarketCSharp.Main
{       
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            OleDbConnection conn = Connexion.getConnexion();
            if (conn != null)
            {
                List<Market> markets = Market.getMarkets(conn);
                foreach (var market in markets)
                {
                    Console.WriteLine($"ID: {market.Idmarket}, Name: {market.Nommarket}, X: {market.X}, Y: {market.Y}, Length: {market.Longueur}, Width: {market.Largeur}");
                }
                conn.Close();
            }
        }
    }
}