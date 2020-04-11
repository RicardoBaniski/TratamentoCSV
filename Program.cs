using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace TratamentoCSV
{
    class Program
    {
        public static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=AVELL\SQLEXPRESS;Initial Catalog=covid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand cmd = new SqlCommand();
            Daily daily = new Daily();

            string path = @"C:\TEMP\csse_covid_19_daily_reports\01-22-2020.csv";
            int count = 1;
            var reader = new StreamReader(path, Encoding.UTF8, true);

            while (true)
            {
                string linha = reader.ReadLine();

                if (linha == null)
                {
                    break;
                }
                else
                {
                    string[] linhaseparada = linha.Split(',');
                    
                    if (count > 1)
                    {

                        if (linhaseparada[0] == "" || linhaseparada[0] == null)
                        {
                            daily.ProvinceState = "NAO INFORMADO";
                        }
                        else
                        {
                            daily.ProvinceState = linhaseparada[0].ToString().Trim();
                        }

                        daily.CountryRegion = linhaseparada[1].ToString().Trim();

                        daily.LastUpdate = linhaseparada[2].ToString().Trim();

                        if (linhaseparada[3] == "")
                        {
                            daily.Confirmed = 0;
                        }
                        else
                        {
                            daily.Confirmed = Convert.ToInt32(linhaseparada[3]);
                        }

                        if (linhaseparada[4] == "")
                        {
                            daily.Deaths = 0;
                        }
                        else
                        {
                            daily.Deaths = Convert.ToInt32(linhaseparada[4]);
                        }

                        if (linhaseparada[5] == "")
                        {
                            daily.Recovered = 0;
                        }
                        else
                        {
                            daily.Recovered = Convert.ToInt32(linhaseparada[5]);
                        }

                        //Console.WriteLine(daily.ProvinceState + " - " + daily.CountryRegion + " - " + daily.LastUpdate + " - " + daily.Confirmed + " - " + daily.Deaths + " - " + daily.Recovered);
                        conn.Open();
                        cmd = new SqlCommand("spInsereDaily", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProvinceState", SqlDbType.VarChar).Value = daily.ProvinceState;
                        cmd.Parameters.Add("@CountryRegion", SqlDbType.VarChar).Value = daily.CountryRegion;
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.VarChar).Value = daily.LastUpdate;
                        cmd.Parameters.Add("@Confirmed", SqlDbType.Int).Value = daily.Confirmed;
                        cmd.Parameters.Add("@Deaths", SqlDbType.Int).Value = daily.Deaths;
                        cmd.Parameters.Add("@Recovered", SqlDbType.Int).Value = daily.Recovered;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                count++;
            }
        }
    }
}
