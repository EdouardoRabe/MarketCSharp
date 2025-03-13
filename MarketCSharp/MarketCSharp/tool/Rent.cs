using System;
using System.Data.OleDb;

namespace MarketCSharp.tool
{
    public class Rent
    {
        private int _idRent;
        private int _idMarket;
        private int _idPeriode;
        private double _montant;

        public Rent(int idRent, int idMarket, int idPeriode, double montant)
        {
            _idRent = idRent;
            _idMarket = idMarket;
            _idPeriode = idPeriode;
            _montant = montant;
        }

        public int GetIdRent()
        {
            return _idRent;
        }

        public void SetIdRent(int value)
        {
            _idRent = value;
        }

        public int GetIdMarket()
        {
            return _idMarket;
        }

        public void SetIdMarket(int value)
        {
            _idMarket = value;
        }

        public int GetIdPeriode()
        {
            return _idPeriode;
        }

        public void SetIdPeriode(int value)
        {
            _idPeriode = value;
        }

        public double GetMontant()
        {
            return _montant;
        }

        public void SetMontant(double value)
        {
            _montant = value;
        }

        public static Rent GetRent(OleDbConnection conn, int idMarket, int idPeriode)
        {
            string query = "SELECT * FROM rents WHERE idmarket = ? AND idperiode = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idMarket", idMarket);
                cmd.Parameters.AddWithValue("@idPeriode", idPeriode);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Rent(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDouble(3)
                        );
                    }
                }
            }
            return null;
        }
    }
}