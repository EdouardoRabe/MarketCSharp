using System;
using System.Data.OleDb;

namespace MarketCSharp.tool
{
    public class Periode
    {
        private int _idPeriode;
        private string _depuis;

        public Periode(int idPeriode, string depuis)
        {
            _idPeriode = idPeriode;
            _depuis = depuis;
        }

        public int GetIdPeriode()
        {
            return _idPeriode;
        }

        public string GetDepuis()
        {
            return _depuis;
        }

        public void SetIdPeriode(int idPeriode)
        {
            _idPeriode = idPeriode;
        }

        public void SetDepuis(string depuis)
        {
            _depuis = depuis;
        }

        public static Periode GetPeriode(OleDbConnection conn, string yearMonth)
        {
            string query = """
                           
                                           SELECT TOP 1 idperiode, depuis 
                                           FROM periodes 
                                           WHERE FORMAT(depuis, 'yyyy-MM') <= ? 
                                           ORDER BY depuis DESC
                                       
                           """;
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@yearMonth", yearMonth);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Periode(
                            reader.GetInt32(0),
                            reader.GetDateTime(1).ToString("yyyy-MM")
                        );
                    }
                }
            }
            return new Periode(1, null);
        }
    }
}