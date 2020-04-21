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
        public static string identificador;
        public static string[] linhaSeparadaSemAspas;
        public static string[] colunaInserida;
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
                                identificador = linhaSeparada[0];
                            }

                            if (count > 1)
                            {
                                //TrataCampoComAspas(linhaSeparada);
                                InsereColunas(linhaSeparada);
                                InsereObj(colunaInserida);
                            }
                        }
                        count++;
                        Console.WriteLine(count + " - " + file);
                    }
                }
            }
        }

        public static void InsereColunas(string[] linhaSeparada)
        {
            var lista = linhaSeparada.ToList();

            switch (identificador)
            {
                case "Province/State":
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
                    break;
                default:
                    break;
            }
            colunaInserida = lista.ToArray();
        }

        //public static void TrataCampoComAspas(string[] linhaSeparada)
        //{
        //    int inicio = 0, fim = 0, count = 0;
        //    var lista = linhaSeparada.ToList();

        //    for (int i = 0; i < linhaSeparada.Length; i++)
        //    {
        //        if (linhaSeparada[i].Contains('"'))
        //        {
        //            count++;
        //            switch (count)
        //            {
        //                case 1:
        //                    inicio = i;
        //                    break;
        //                case 2:
        //                    fim = i;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        if (inicio != 0 && fim != 0)
        //        {
        //            int d = 1;
        //            while ((fim - inicio) >= d)
        //            {
        //                linhaSeparada[inicio] += linhaSeparada[inicio + d];
        //                d++;
        //            }

        //            lista.RemoveRange(inicio + 1, (fim - inicio));
        //            fim = 0;
        //            inicio = 0;
        //        }
        //    }
        //    linhaSeparadaSemAspas = lista.ToArray();
        //}


        //public static void InsereColunas(string[] linhaSeparadaSemAspas)
        //{
        //    var lista = linhaSeparadaSemAspas.ToList();
        //    switch (linhaSeparadaSemAspas.Length)
        //    {
        //        case 6:
        //            lista.Insert(6, "");
        //            lista.Insert(3, "");
        //            lista.Insert(4, "");
        //            lista.Insert(0, "");
        //            lista.Insert(0, "");
        //            colunaInserida = lista.ToArray();
        //            break;
        //        case 8:
        //            lista.Insert(0, "");
        //            lista.Insert(0, "");
        //            lista.Insert(5, lista[8]);
        //            lista.Insert(6, lista[10]);
        //            lista[10] = "";
        //            colunaInserida = lista.ToArray();
        //            break;
        //        default:
        //            colunaInserida = lista.ToArray();
        //            break;
        //    }
        //}

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
