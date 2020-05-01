using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Globalization;

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
                        Console.WriteLine("Linha: " + count + " - Arquivo: " + file);
                    }
                }
            }
            Console.WriteLine(">>>>>>>>>>>>>  CONCLUIDO  <<<<<<<<<<<<<");
        }

        public static void FormataColunas(string[] linhaSeparada)
        {
            var lista = linhaSeparada.ToList();

            for (int i = 0; i < linhaSeparada.Length; i++)
            {
                if (lista[i].Contains('"'))
                {
                    lista[i] = RemoveAspasDuplas(lista[i]);
                }
            }

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
                    lista[3] = lista[3] + " " + lista[2];
                    lista.RemoveAt(2);
                }

                if (lista[1].Contains("Bonaire"))
                {
                    lista[1] = lista[1] + ", " + lista[2];
                    lista.RemoveAt(2);
                }
            }

            if (lista[1].Contains("Korea") || lista[1].Contains("Bahamas") || lista[1].Contains("Gambia"))
            {
                lista[2] = lista[2] + " " + lista[1];
                lista[1] = "";
            }

            colunaFormatada = lista.ToArray();
        }

        public static void InsereObj(string[] colunaFormatada)
        {
            daily.City = colunaFormatada[0] == "" ? "" : colunaFormatada[0];
            daily.ProvinceState = colunaFormatada[1] == "" ? "" : FormataEstado(colunaFormatada[1]);
            daily.CountryRegion = colunaFormatada[2] == "" ? "" : FormataPais(colunaFormatada[2]);
            daily.LastUpdate = Convert.ToDateTime(FormataData(colunaFormatada[3]));
            daily.Lat = FormataCoordenada(colunaFormatada[4]);
            daily.Long = FormataCoordenada(colunaFormatada[5]);
            daily.Confirmed = colunaFormatada[6] == "" ? 0 : Convert.ToInt32(colunaFormatada[6]);
            daily.Deaths = colunaFormatada[7] == "" ? 0 : Convert.ToInt32(colunaFormatada[7]);
            daily.Recovered = colunaFormatada[8] == "" ? 0 : Convert.ToInt32(colunaFormatada[8]);
            daily.Active = colunaFormatada[9] == "" ? 0 : Convert.ToInt32(colunaFormatada[9]);
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
            cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = daily.LastUpdate;
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

        public static String FormataData(string LastUpdate)
        {
            CultureInfo MyCultureInfo = new CultureInfo("en-US");
            DateTime MyDateTime = DateTime.Parse(LastUpdate, MyCultureInfo);
            return MyDateTime.ToString();
        }

        public static String FormataPais(string CountryRegion)
        {
            string pais;
            switch (CountryRegion)
            {
                case "UK":
                    pais = "United Kingdom";
                    break;
                case "US":
                    pais = "United States";
                    break;
                case "Taiwan*":
                    pais = "Taiwan";
                    break;
                default:
                    pais = CountryRegion;
                    break;
            }
            return pais.Trim();
        }

        public static String RemoveAspasDuplas(string item)
        {
            item = item.Replace('"', ' ');
            return item.Trim();
        }

        public static String FormataEstado(string ProvinceState)
        {
            string estado;

            switch (ProvinceState)
            {
                case "IL":
                    estado = "Illinois";
                    break;
                case "CA":
                    estado = "California";
                    break;
                case "CA (From Diamond Princess)":
                    estado = "California (From Diamond Princess)";
                    break;
                case "CO":
                    estado = "Colorado";
                    break;
                case "D.C.":
                    estado = "District of Columbia";
                    break;
                case "FL":
                    estado = "Florida";
                    break;
                case "GA":
                    estado = "Georgia";
                    break;
                case "NV":
                    estado = "Nevada";
                    break;
                case "NY":
                    estado = "New York";
                    break;
                case "OR":
                    estado = "Oregon";
                    break;
                case "PA":
                    estado = "Pennsylvania";
                    break;
                case "QC":
                    estado = "Quebec";
                    break;
                case "RI":
                    estado = "Rhode Island";
                    break;
                case "SC":
                    estado = "South Carolina";
                    break;
                case "TN":
                    estado = "Tennessee";
                    break;
                case "UT":
                    estado = "Utah";
                    break;
                case "VA":
                    estado = "Virginia";
                    break;
                case "WA":
                    estado = "Washington";
                    break;
                case "WI":
                    estado = "Wisconsin";
                    break;
                case "CT":
                    estado = "Connecticut";
                    break;
                case "HI":
                    estado = "Havai";
                    break;
                case "IN":
                    estado = "Indiana";
                    break;
                case "KS":
                    estado = "Kansas";
                    break;
                case "KY":
                    estado = "Kentucky";
                    break;
                case "OK":
                    estado = "Oklahoma";
                    break;
                case "LA":
                    estado = "Luisiana";
                    break;
                case "MA":
                    estado = "Massachusetts";
                    break;
                case "MD":
                    estado = "Maryland";
                    break;
                case "MN":
                    estado = "Minnesota";
                    break;
                case "MO":
                    estado = "Missouri";
                    break;
                case "NC":
                    estado = "North Carolina";
                    break;
                case "NE":
                    estado = "Nebraska";
                    break;
                case "NE (From Diamond Princess)":
                    estado = "Nebraska (From Diamond Princess)";
                    break;
                case "NH":
                    estado = "New Hampshire";
                    break;
                case "NJ":
                    estado = "New Jersey";
                    break;
                case "ON":
                    estado = "Ontario";
                    break;
                case "TX":
                    estado = "Texas";
                    break;
                case "TX (From Diamond Princess)":
                    estado = "Texas (From Diamond Princess)";
                    break;
                case "VT":
                    estado = "Vermont";
                    break;
                case "AZ":
                    estado = "Arizona";
                    break;
                case "IA":
                    estado = "Iowa";
                    break;
                case "U.S.":
                    estado = "";
                    break;
                default:
                    estado = ProvinceState;
                    break;
            }
            return estado.Trim();
        }

        public static String FormataCoordenada(string coordenada)
        {
            if (coordenada != "")
            {
                if (coordenada.Contains("."))
                {
                    string[] coordenadaSeparada = coordenada.Split('.');

                    if (coordenadaSeparada[1].Length > 8)
                    {
                        var str = coordenadaSeparada[1].Remove(7, coordenadaSeparada[1].Length - 8);
                        coordenadaSeparada[1] = str;
                    }

                    while (coordenadaSeparada[1].Length < 8)
                    {
                        coordenadaSeparada[1] += "0";
                    }
                    coordenada = coordenadaSeparada[0] + "," + coordenadaSeparada[1];
                }
                else
                {
                    coordenada += ",0000000";
                }
            }
            return coordenada.Trim();
        }
    }
}
