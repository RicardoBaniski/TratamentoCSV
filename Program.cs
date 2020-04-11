﻿using System;
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

            string path = @"C:\TEMP\csse_covid_19_daily_reports\";
            string[] files = Directory.GetFiles(path, "*.csv");

            foreach (var file in files)
            {
                int count = 1;

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
                            string[] linhaseparada = linha.Split(',');

                            if (linhaseparada.Length == 6 && count > 1)
                            {
                                daily.City = null;

                                if (linhaseparada[0] == "" || linhaseparada[1] == null)
                                {
                                    daily.ProvinceState = null;
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
                            }

                            if (linhaseparada.Length == 7 && count > 1)
                            {
                                if (linhaseparada[0] == "" || linhaseparada[0] == null)
                                {
                                    daily.City = null;
                                }
                                else
                                {
                                    daily.City = linhaseparada[0].Substring(1);
                                }

                                if (linhaseparada[1] == "" || linhaseparada[1] == null)
                                {
                                    daily.ProvinceState = null;
                                }
                                else
                                {
                                    daily.ProvinceState = linhaseparada[1].Substring(0, linhaseparada[1].Length - 1);
                                }

                                daily.CountryRegion = linhaseparada[2].ToString().Trim();

                                daily.LastUpdate = linhaseparada[3].ToString().Trim();

                                if (linhaseparada[4] == "")
                                {
                                    daily.Confirmed = 0;
                                }
                                else
                                {
                                    daily.Confirmed = Convert.ToInt32(linhaseparada[4]);
                                }

                                if (linhaseparada[5] == "")
                                {
                                    daily.Deaths = 0;
                                }
                                else
                                {
                                    daily.Deaths = Convert.ToInt32(linhaseparada[5]);
                                }

                                if (linhaseparada[6] == "")
                                {
                                    daily.Recovered = 0;
                                }
                                else
                                {
                                    daily.Recovered = Convert.ToInt32(linhaseparada[6]);
                                }

                                //Console.WriteLine(daily.ProvinceState + " - " + daily.CountryRegion + " - " + daily.LastUpdate + " - " + daily.Confirmed + " - " + daily.Deaths + " - " + daily.Recovered);

                                conn.Open();
                                cmd = new SqlCommand("spInsereDaily", conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = daily.City;
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
    }
}
