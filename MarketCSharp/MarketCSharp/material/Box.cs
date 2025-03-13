using System;
using System.Collections.Generic;
using System.Data.OleDb;
using MarketCSharp.tool;

namespace MarketCSharp.material
{
    public class Box
    {
        private int _idBox;
        private int _idMarket;
        private int _num;
        private double _longueur;
        private double _largeur;
        private int _x;
        private int _y;

        public Box(int idBox, int idMarket, int num, double longueur, double largeur, int x, int y)
        {
            _idBox = idBox;
            _idMarket = idMarket;
            _num = num;
            _longueur = longueur;
            _largeur = largeur;
            _x = x;
            _y = y;
        }

        public int GetIdBox()
        {
            return _idBox;
        }

        public int GetIdMarket()
        {
            return _idMarket;
        }

        public int GetNum()
        {
            return _num;
        }

        public double GetLongueur()
        {
            return _longueur;
        }

        public double GetLargeur()
        {
            return _largeur;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public static Box GetBoxById(OleDbConnection conn, int idBox)
        {
            string query = "SELECT * FROM boxs WHERE idbox = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idBox", idBox);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Box(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDouble(3),
                            reader.GetDouble(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6)
                        );
                    }
                }
            }
            return null;
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
                        int idBox = reader.GetInt32(0);
                        int idMarket = reader.GetInt32(1);
                        int num = reader.GetInt32(2);
                        double longueur = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                        double largeur = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                        int x = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        int y = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);

                        boxs.Add(new Box(idBox, idMarket, num, longueur, largeur, x, y));
                    }
                }
            }
            return boxs;
        }

        public static List<Box> GetBoxByIdOwner(OleDbConnection conn, int idowner)
        {
            List<Box> boxs = new List<Box>();
            string query = "SELECT * FROM boxs WHERE idowner = ?";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idowner", idowner);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idBox = reader.GetInt32(0);
                        int idMarket = reader.GetInt32(1);
                        int num = reader.GetInt32(2);
                        double longueur = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                        double largeur = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                        int x = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        int y = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);

                        boxs.Add(new Box(idBox, idMarket, num, longueur, largeur, x, y));
                    }
                }
            }
            return boxs;
        }

        public double CalculRent(OleDbConnection conn, string yearMonth)
        {
            Periode periode = Periode.GetPeriode(conn, yearMonth);
            Rent rentPerSqm = Rent.GetRent(conn, _idMarket, periode.GetIdPeriode());
            double area = _longueur * _largeur;
            double totalRent = area * rentPerSqm.GetMontant();
            return totalRent;
        }

        public double GetPourcent(OleDbConnection conn, string yearMonth)
        {
            Paiement paiement = Paiement.GetPaiement(conn, yearMonth, _idBox);
            if (paiement == null)
            {
                return 0;
            }
            double totalRent = CalculRent(conn, yearMonth);
            Console.WriteLine($"Total rent: {paiement.GetMontant() / totalRent}");
            return paiement.GetMontant() / totalRent;
        }

        public double InsertPaiement(OleDbConnection conn, DateTime datePaiement, double montant, string yearMonth, DateTime? finLocation = null)
        {
            if (montant == 0)
            {
                return montant;
            }
            montant = Convert.ToDouble(montant);
            if (finLocation.HasValue && DateTime.ParseExact(yearMonth, "yyyy-MM", null) > finLocation.Value)
            {
                return montant;
            }
            double rent = CalculRent(conn, yearMonth);
            Paiement paiement = Paiement.GetPaiement(conn, yearMonth, _idBox);
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = conn;
                if (paiement != null)
                {
                    double remainingRent = rent - paiement.GetMontant();
                    if (remainingRent == 0)
                    {
                        return montant;
                    }
                    if (montant >= remainingRent)
                    {
                        cmd.CommandText = "INSERT INTO paiements (idbox, montant, paied, datepaiement) VALUES (?, ?, ?, ?)";
                        cmd.Parameters.AddWithValue("@idBox", _idBox);
                        cmd.Parameters.AddWithValue("@montant", remainingRent);
                        cmd.Parameters.AddWithValue("@paied", yearMonth + "-01");
                        cmd.Parameters.AddWithValue("@datePaiement", datePaiement);
                        cmd.ExecuteNonQuery();
                        montant -= remainingRent;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO paiements (idbox, montant, paied, datepaiement) VALUES (?, ?, ?, ?)";
                        cmd.Parameters.AddWithValue("@idBox", _idBox);
                        cmd.Parameters.AddWithValue("@montant", montant);
                        cmd.Parameters.AddWithValue("@paied", yearMonth + "-01");
                        cmd.Parameters.AddWithValue("@datePaiement", datePaiement);
                        cmd.ExecuteNonQuery();
                        montant = 0;
                    }
                }
                else
                {
                    if (montant >= rent)
                    {
                        cmd.CommandText = "INSERT INTO paiements (idbox, montant, paied, datepaiement) VALUES (?, ?, ?, ?)";
                        cmd.Parameters.AddWithValue("@idBox", _idBox);
                        cmd.Parameters.AddWithValue("@montant", rent);
                        cmd.Parameters.AddWithValue("@paied", yearMonth + "-01");
                        cmd.Parameters.AddWithValue("@datePaiement", datePaiement);
                        cmd.ExecuteNonQuery();
                        montant -= rent;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO paiements (idbox, montant, paied, datepaiement) VALUES (?, ?, ?, ?)";
                        cmd.Parameters.AddWithValue("@idBox", _idBox);
                        cmd.Parameters.AddWithValue("@montant", montant);
                        cmd.Parameters.AddWithValue("@paied", yearMonth + "-01");
                        cmd.Parameters.AddWithValue("@datePaiement", datePaiement);
                        cmd.ExecuteNonQuery();
                        montant = 0;
                    }
                }
            }
            return montant;
        }

        public bool IsBoxRented(OleDbConnection conn, string yearMonth)
        {
            Location location = Location.GetLocationByBoxAndYearMonth(conn, _idBox, yearMonth);
            return location != null;
        }

        public static void InsertBox(OleDbConnection conn, int idMarket, int num, double longueur, double largeur, int x, int y)
        {
            string query = @"
                INSERT INTO boxs (idmarket, num, longueur, largeur, x, y) 
                VALUES (?, ?, ?, ?, ?, ?)
            ";
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idMarket", idMarket);
                cmd.Parameters.AddWithValue("@num", num);
                cmd.Parameters.AddWithValue("@longueur", longueur);
                cmd.Parameters.AddWithValue("@largeur", largeur);
                cmd.Parameters.AddWithValue("@x", x);
                cmd.Parameters.AddWithValue("@y", y);
                cmd.ExecuteNonQuery();
            }
        }
    }


}