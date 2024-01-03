using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KonyvtarAsztali
{
    public class Statisztika : IStatisztika
    {
        private List<Konyv> konyvek;
        private MySqlConnectionStringBuilder connectStringBuilder;

        public Statisztika()
        {
            connectStringBuilder = new MySqlConnectionStringBuilder();
            konyvek = new List<Konyv>();
        }

        public List<Konyv> Konyvek { get => konyvek; set => konyvek = value; }

        public void ConsoleFuttat()
        {
            if (Beolvas("vizsga-2022-14s-wip-db"))
            {
                tobbMint500();
                vanE1950();
                legHosszabb();
                legtobbKonyvesSzerzo();
                konyvCim();
            }
            else
            {
                Console.WriteLine("A csatlakozás vagy beolvasás közben hiba történt.");
            }
            
        }

        public void AdatbazisbolTorles(int torlendoId)
        {
            konyvek.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectStringBuilder.ConnectionString))
            {
                connection.Open();
                string sql = $"DELETE FROM books WHERE id = {torlendoId};";
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = sql;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Beolvas("vizsga-2022-14s-wip-db");
                }
            }
        }

        public bool Beolvas(string databaseName)
        {
            connectStringBuilder.Server = "localhost";
            connectStringBuilder.Port = 3306;
            connectStringBuilder.Database = databaseName;
            connectStringBuilder.UserID = "root";
            connectStringBuilder.Password = "";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectStringBuilder.ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM books";
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = sql;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           
                            int id = reader.GetInt32("id");
                            string title = reader.GetString("title");
                            string author = reader.GetString("author");
                            int publish_year = reader.GetInt32("publish_year");
                            int page_count = reader.GetInt32("page_count");
                            Konyv konyv = new Konyv(id, title, author, publish_year, page_count);
                            konyvek.Add(konyv);
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void tobbMint500()
        {
            List<Konyv> tobb500nal = konyvek.FindAll(x => x.Page_count > 500);
            Console.WriteLine("500 oldalnál hosszabb könyvek száma: " + tobb500nal.Count);
        }

        private void vanE1950()
        {
            if (konyvek.FindAll(x => x.Publish_year < 1950).Count > 0)
            {
                Console.WriteLine("Van 1950-nél régebbi könyv");              
            }
            else
            {
                Console.WriteLine("Nincs 1950-nél régebbi könyv");
            }
        }

        private void legHosszabb()
        {
            Konyv leghosszabb = konyvek.Find(x => x.Page_count == konyvek.Max(y => y.Page_count));
            Console.WriteLine($"A leghosszabb könyv:\n\tSzerző: {leghosszabb.Author}\n\tCím: {leghosszabb.Title}\n\tKiadás éve: {leghosszabb.Publish_year}\n\tOldalszám: {leghosszabb.Page_count}");
        }

        private void legtobbKonyvesSzerzo()
        {
            var legtobbSzerzo = konyvek.GroupBy(x => x.Author)
                .Select(x => new { Author = x.Key, BookCount = x.Count() })
                .OrderByDescending(x => x.BookCount)
                .FirstOrDefault();

            Console.WriteLine("A legtöbb könyvvel rendelkező szerző: " + legtobbSzerzo.Author);
        }

        private void konyvCim()
        {
            Console.Write("Adjon meg egy könyv címet: ");
            string megadottCim = Console.ReadLine();

            Console.WriteLine($"A megadott könyv {konyvek.FindAll(x => x.Title == megadottCim).Count}x lett kikölcsönözve");
        }
    }
}
