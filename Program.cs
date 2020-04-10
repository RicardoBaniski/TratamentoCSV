using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Globalization;
using System.Threading;

namespace TratamentoCSV
{
    class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo CultureInfo1 = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            CultureInfo1.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy HH:mm";
            Thread.CurrentThread.CurrentCulture = CultureInfo1;

            string linha = "";
            string[] linhaseparada = null;
            StreamReader reader = new StreamReader(@"C:\Users\rbani\Documents\OPET\TDS5_N_Data Science\ETL\DS_aula06\COVID-19\csse_covid_19_data\csse_covid_19_daily_reports\01-22-2020.csv", Encoding.UTF8, true);
            Daily daily = new Daily();

            int count = 0;

            while (true)
            {
                linha = reader.ReadLine();

                if (linha == null)
                {
                    break;
                }
                else
                {
                    linhaseparada = linha.Split(',');

                    if (count >= 1)
                    {
                        //Province / State
                        if (linhaseparada[0] == "" || linhaseparada[0] == null)
                        {
                            daily.ProvinceState = "NAO INFORMADO";
                        }
                        else
                        {
                            daily.ProvinceState = linhaseparada[0];
                        }

                        daily.CountryRegion = linhaseparada[1];

                        daily.LastUpdate = Convert.ToDateTime((linhaseparada[2]).ToString(CultureInfo1));

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
                    count++;
                }

                string resultado = string.Format(
                @"Linha - 
                    Campo 1: {0}
                    Campo 2: {1}
                    Campo 3: {2}
                    Campo 4: {3}
                    Campo 5: {4}
                    Campo 6: {5}", linhaseparada[0], linhaseparada[1], linhaseparada[2], linhaseparada[3], linhaseparada[4], linhaseparada[5]);
                Console.WriteLine(resultado);
                Console.WriteLine(daily.ProvinceState + " - " + daily.CountryRegion + " - " + daily.LastUpdate + " - " + daily.Confirmed + " - " + daily.Deaths + " - " + daily.Recovered);
            }
            Console.ReadKey();
        }
    }
}


