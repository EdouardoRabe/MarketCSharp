using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace MarketCSharp.tool
{
    public class Paiement
    {
        private int _idPaiement;
        private int _idBox;
        private double _montant;
        private DateTime _paied;
        private DateTime _datePaiement;

        public Paiement(int idPaiement, int idBox, double montant, DateTime paied, DateTime datePaiement)
        {
            _idPaiement = idPaiement;
            _idBox = idBox;
            _montant = montant;
            _paied = paied;
            _datePaiement = datePaiement;
        }

        public int GetIdPaiement()
        {
            return _idPaiement;
        }

        public void SetIdPaiement(int value)
        {
            _idPaiement = value;
        }

        public int GetIdBox()
        {
            return _idBox;
        }

        public void SetIdBox(int value)
        {
            _idBox = value;
        }

        public double GetMontant()
        {
            return _montant;
        }

        public void SetMontant(double value)
        {
            _montant = value;
        }

        public DateTime GetPaied()
        {
            return _paied;
        }

        public void SetPaied(DateTime value)
        {
            _paied = value;
        }

        public DateTime GetDatePaiement()
        {
            return _datePaiement;
        }

        public void SetDatePaiement(DateTime value)
        {
            _datePaiement = value;
        }

        public static List<Paiement> GetPaiements(OleDbConnection conn)
        {
            List<Paiement> paiements = new List<Paiement>();
            string query = "SELECT * FROM paiements";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paiements.Add(new Paiement(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetDouble(2),
                            reader.GetDateTime(3),
                            reader.GetDateTime(4)
                        ));
                    }
                }
            }

            return paiements;
        }

        public static Paiement GetPaiement(OleDbConnection conn, string yearMonth, int idBox)
        {
            string query = @"
                SELECT idbox, SUM(montant) as montant2, paied 
                FROM paiements 
                WHERE FORMAT(paied, 'yyyy-MM') = ? AND idbox = ? 
                GROUP BY idbox, paied
            ";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@yearMonth", yearMonth);
                cmd.Parameters.AddWithValue("@idBox", idBox);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Paiement(
                            0,
                            reader.GetInt32(0),
                            reader.GetDouble(1),
                            reader.GetDateTime(2),
                            DateTime.Now
                        );
                    }
                }
            }

            return null;
        }
    }
}    