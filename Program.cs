using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace TratamentoCSV
{
    class Program
    {
        public static string arquivo;
        public static string[] cabecalho;
        public static string[] colunaFormatada;
        public static SqlConnection conn = new SqlConnection(@"Data Source=AVELL\SQLEXPRESS;Initial Catalog=covid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public static SqlCommand cmd = new SqlCommand();
        public static Daily daily = new Daily();
        public static void Main(string[] args)
        {
            string path = @"C:\TEMP\csse_covid_19_daily_reports\";
            string[] files = Directory.GetFiles(path, "*.csv");

            foreach (var file in files)
            {
                int count = 1;
                arquivo = file;

                if (File.Exists(file))
                {
                    var reader = new StreamReader(file, Encoding.UTF8, true);

                    while (true)
                    {
                        string linha = reader.ReadLine();

                        if (linha == null)
                        {
                            break;
                        }
                        else
                        {
                            string[] linhaSeparada = linha.Split(',');

                            if (count == 1)
                            {
                                cabecalho = linhaSeparada;
                            }

                            if (count > 1)
                            {
                                FormataColunas(linhaSeparada);
                                InsereObj(colunaFormatada);
                            }
                        }

                        count++;
                        Console.WriteLine(count + " - " + file);
                    }
                }
            }
        }

        public static void FormataColunas(string[] linhaSeparada)
        {
            var lista = linhaSeparada.ToList();

            if (cabecalho[0].Equals("Province/State") && cabecalho[cabecalho.Length - 1].Equals("Recovered"))
            {
                if (linhaSeparada.Length < 7)
                {
                    lista.Insert(0, "");
                    lista.Insert(4, "");
                    lista.Insert(5, "");
                    lista.Insert(9, "");
                }
                else
                {
                    lista.Insert(4, "");
                    lista.Insert(5, "");
                    lista.Insert(9, "");
                }
            }

            if (cabecalho[0].Equals("Province/State") && cabecalho[cabecalho.Length - 1].Equals("Longitude"))
            {
                if (linhaSeparada.Length < 9)
                {
                    lista.Insert(0, "");
                    lista.Insert(4, "");
                    lista.Insert(5, "");
                    lista[4] = lista[9];
                    lista[5] = lista[10];
                    lista[9] = "";
                    lista.RemoveAt(10);
                }
                else
                {
                    lista.Insert(4, "");
                    lista.Insert(5, "");
                    lista[4] = lista[9];
                    lista[5] = lista[10];
                    lista[9] = "";
                    lista.RemoveAt(10);
                }
            }

            if (cabecalho[0].Equals("FIPS"))
            {
                lista.RemoveAt(0);

                if (lista[2].Contains("Korea"))
                {
                    lista[3] += lista[2] + " ";
                    lista.RemoveAt(2);
                }

                if (lista[1].Contains("Bonaire"))
                {
                    lista[1] += lista[2] + ", ";
                    lista.RemoveAt(2);
                }
            }

            colunaFormatada = lista.ToArray();
        }

        public static void InsereObj(string[] colunaInserida)
        {
            daily.City = colunaInserida[0] == "" ? "" : colunaInserida[0].Replace('"', ' ').Trim();
            daily.ProvinceState = colunaInserida[1] == "" ? "" : colunaInserida[1].Replace('"', ' ').Trim();
            daily.CountryRegion = colunaInserida[2] == "" ? "" : colunaInserida[2].Replace('"', ' ').Trim();
            daily.LastUpdate = colunaInserida[3];
            daily.Lat = colunaInserida[4];
            daily.Long = colunaInserida[5];
            daily.Confirmed = colunaInserida[6] == "" ? 0 : Convert.ToInt32(colunaInserida[6]);
            daily.Deaths = colunaInserida[7] == "" ? 0 : Convert.ToInt32(colunaInserida[7]);
            daily.Recovered = colunaInserida[8] == "" ? 0 : Convert.ToInt32(colunaInserida[8]);
            daily.Active = colunaInserida[9] == "" ? 0 : Convert.ToInt32(colunaInserida[9]);
            daily.Arquivo = arquivo;
            InsereSQL(conn, ref cmd, daily);
        }

        public static void InsereSQL(SqlConnection conn, ref SqlCommand cmd, Daily daily)
        {
            conn.Open();
            cmd = new SqlCommand("spInsereDaily", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = daily.City;
            cmd.Parameters.Add("@ProvinceState", SqlDbType.VarChar).Value = daily.ProvinceState;
            cmd.Parameters.Add("@CountryRegion", SqlDbType.VarChar).Value = daily.CountryRegion;
            cmd.Parameters.Add("@LastUpdate", SqlDbType.VarChar).Value = daily.LastUpdate;
            cmd.Parameters.Add("@Lat", SqlDbType.VarChar).Value = daily.Lat;
            cmd.Parameters.Add("@Long", SqlDbType.VarChar).Value = daily.Long;
            cmd.Parameters.Add("@Confirmed", SqlDbType.Int).Value = daily.Confirmed;
            cmd.Parameters.Add("@Deaths", SqlDbType.Int).Value = daily.Deaths;
            cmd.Parameters.Add("@Recovered", SqlDbType.Int).Value = daily.Recovered;
            cmd.Parameters.Add("@Active", SqlDbType.Int).Value = daily.Active;
            cmd.Parameters.Add("@Arquivo", SqlDbType.VarChar).Value = daily.Arquivo;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
