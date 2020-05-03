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
        public static string path = @"C:\TEMP\csse_covid_19_daily_reports\TST";
        public static SqlConnection conn = new SqlConnection(@"Data Source=AVELL\SQLEXPRESS;Initial Catalog=covid;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public static SqlCommand cmd = new SqlCommand();
        public static Daily daily = new Daily();
        public static string[] header;
        public static string[] formattedColumn;
        public static string archive;
        public static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(path, "*.csv");

            foreach (var file in files)
            {
                int count = 1;
                archive = file;

                if (File.Exists(file))
                {
                    var reader = new StreamReader(file, Encoding.UTF8, true);

                    while (true)
                    {
                        string line = reader.ReadLine();

                        if (line == null)
                        {
                            break;
                        }
                        else
                        {
                            string[] separationColumns = line.Split(',');

                            if (count == 1)
                            {
                                header = separationColumns;
                            }

                            if (count > 1)
                            {
                                FormatsColumns(separationColumns);
                                InsertObj(formattedColumn);
                            }
                        }
                        count++;
                        Console.WriteLine("Line: " + count + " - File: " + file);
                    }
                }
            }
            Console.WriteLine(">>>>>>>>>>>>>  SUCCESSFULLY COMPLETED  <<<<<<<<<<<<<");
        }

        public static void FormatsColumns(string[] separationColumns)
        {
            var list = separationColumns.ToList();

            for (int i = 0; i < separationColumns.Length; i++)
            {
                if (list[i].Contains('"'))
                {
                    list[i] = RemovesQuotes(list[i]);
                }
            }

            if (header[0].Equals("Province/State") && header[header.Length - 1].Equals("Recovered"))
            {
                if (separationColumns.Length < 7)
                {
                    list.Insert(0, "");
                    list.Insert(4, "");
                    list.Insert(5, "");
                    list.Insert(9, "");
                }
                else
                {
                    list.Insert(4, "");
                    list.Insert(5, "");
                    list.Insert(9, "");
                }
            }

            if (header[0].Equals("Province/State") && header[header.Length - 1].Equals("Longitude"))
            {
                if (separationColumns.Length < 9)
                {
                    list.Insert(0, "");
                    list.Insert(4, "");
                    list.Insert(5, "");
                    list[4] = list[9];
                    list[5] = list[10];
                    list[9] = "";
                    list.RemoveAt(10);
                }
                else
                {
                    list.Insert(4, "");
                    list.Insert(5, "");
                    list[4] = list[9];
                    list[5] = list[10];
                    list[9] = "";
                    list.RemoveAt(10);
                }
            }

            if (header[0].Equals("FIPS"))
            {
                list.RemoveAt(0);

                if (list[2].Contains("Korea"))
                {
                    list[3] = list[3] + " " + list[2];
                    list.RemoveAt(2);
                }

                if (list[1].Contains("Bonaire"))
                {
                    list[1] = list[1] + ", " + list[2];
                    list.RemoveAt(2);
                }
            }

            if (list[1].Contains("Korea") || list[1].Contains("Bahamas") || list[1].Contains("Gambia"))
            {
                list[2] = list[2] + " " + list[1];
                list[1] = "";
            }

            if (list[2].Contains("Faroe Islands") || list[2].Contains("Greenland"))
            {
                list[1] = list[2];
                list[2] = "Denmark";
            }

            if (list[1].Contains("Hong Kong") || list[1].Contains("Macau"))
            {
                list[2] = "China";
            }

            if (list[2].Contains("Cayman Islands") || list[2].Contains("Gibraltar") || list[2].Contains("Channel Islands"))
            {
                list[1] = list[2];
                list[2] = "United Kingdom";
            }

            if (list[2].Contains("Saint Barthelemy") || list[2].Contains("Guadeloupe") || list[2].Contains("Reunion") || list[2].Contains("French Guiana") || list[2].Contains("Mayotte") || list[2].Contains("Martinique"))
            {
                list[1] = list[2];
                list[2] = "France";
            }

            if (list[2].Contains("Aruba") || list[2].Contains("Curacao"))
            {
                list[1] = list[2];
                list[2] = "Netherlands";
            }

            if (list[2].Contains("Diamond Princess"))
            {
                list[1] = list[2];
                list[2] = "Cruise Ship";
            }

            if (list[2].Contains("Georgia") || list[2].Contains("Guam") || list[2].Contains("Puerto Rico"))
            {
                list[1] = list[2];
                list[2] = "United States";
            }
            if (list[2].Contains("Taipei"))
            {
                list[1] = "Taipei";
                list[2] = "Taiwan";
            }

            if (list[1].Contains("Chicago"))
            {
                list[0] = list[1];
                list[1] = "Illinois";
            }

            if (list[0].Contains("Virgin Islands"))
            {
                list[1] = list[0];
                list[0] = "";
            }

            formattedColumn = list.ToArray();
        }

        public static void InsertObj(string[] formattedColumn)
        {
            daily.City = formattedColumn[0] == "" ? "" : formattedColumn[0];
            daily.ProvinceState = formattedColumn[1] == "" ? "" : FormatsState(formattedColumn[1]);
            daily.CountryRegion = formattedColumn[2] == "" ? "" : FormatsCountry(formattedColumn[2]);
            daily.LastUpdate = Convert.ToDateTime(FormatsDate(formattedColumn[3]));
            daily.Lat = FormatsCoordinate(formattedColumn[4]);
            daily.Long_ = FormatsCoordinate(formattedColumn[5]);
            daily.Confirmed = formattedColumn[6] == "" ? 0 : Convert.ToInt32(formattedColumn[6]);
            daily.Deaths = formattedColumn[7] == "" ? 0 : Convert.ToInt32(formattedColumn[7]);
            daily.Recovered = formattedColumn[8] == "" ? 0 : Convert.ToInt32(formattedColumn[8]);
            daily.Active = formattedColumn[9] == "" ? 0 : Convert.ToInt32(formattedColumn[9]);
            daily.Archive = archive;
            InsertSQL(conn, ref cmd, daily);
        }

        public static void InsertSQL(SqlConnection conn, ref SqlCommand cmd, Daily daily)
        {
            conn.Open();
            cmd = new SqlCommand("spInsereDaily", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = daily.City;
            cmd.Parameters.Add("@ProvinceState", SqlDbType.VarChar).Value = daily.ProvinceState;
            cmd.Parameters.Add("@CountryRegion", SqlDbType.VarChar).Value = daily.CountryRegion;
            cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = daily.LastUpdate;
            cmd.Parameters.Add("@Lat", SqlDbType.VarChar).Value = daily.Lat;
            cmd.Parameters.Add("@Long", SqlDbType.VarChar).Value = daily.Long_;
            cmd.Parameters.Add("@Confirmed", SqlDbType.Int).Value = daily.Confirmed;
            cmd.Parameters.Add("@Deaths", SqlDbType.Int).Value = daily.Deaths;
            cmd.Parameters.Add("@Recovered", SqlDbType.Int).Value = daily.Recovered;
            cmd.Parameters.Add("@Active", SqlDbType.Int).Value = daily.Active;
            cmd.Parameters.Add("@Archive", SqlDbType.VarChar).Value = daily.Archive;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static String FormatsDate(string LastUpdate)
        {
            CultureInfo MyCultureInfo = new CultureInfo("en-US");
            DateTime MyDateTime = DateTime.Parse(LastUpdate, MyCultureInfo);
            return MyDateTime.ToString();
        }

        public static String FormatsCountry(string CountryRegion)
        {
            string country;
            switch (CountryRegion)
            {
                case "UK":
                    country = "United Kingdom";
                    break;
                case "US":
                    country = "United States";
                    break;
                case "Taiwan*":
                    country = "Taiwan";
                    break;
                case "Others":
                    country = "Cruise Ship";
                    break;
                default:
                    country = CountryRegion;
                    break;
            }
            return country.Trim();
        }

        public static String RemovesQuotes(string item)
        {
            item = item.Replace('"', ' ');
            return item.Trim();
        }

        public static String FormatsState(string ProvinceState)
        {
            string state;

            switch (ProvinceState)
            {
                case "IL":
                    state = "Illinois";
                    break;
                case "CA":
                    state = "California";
                    break;
                case "CA (From Diamond Princess)":
                    state = "California (From Diamond Princess)";
                    break;
                case "CO":
                    state = "Colorado";
                    break;
                case "D.C.":
                    state = "District of Columbia";
                    break;
                case "FL":
                    state = "Florida";
                    break;
                case "GA":
                    state = "Georgia";
                    break;
                case "NV":
                    state = "Nevada";
                    break;
                case "NY":
                    state = "New York";
                    break;
                case "OR":
                    state = "Oregon";
                    break;
                case "PA":
                    state = "Pennsylvania";
                    break;
                case "QC":
                    state = "Quebec";
                    break;
                case "RI":
                    state = "Rhode Island";
                    break;
                case "SC":
                    state = "South Carolina";
                    break;
                case "TN":
                    state = "Tennessee";
                    break;
                case "UT":
                    state = "Utah";
                    break;
                case "VA":
                    state = "Virginia";
                    break;
                case "WA":
                    state = "Washington";
                    break;
                case "WI":
                    state = "Wisconsin";
                    break;
                case "CT":
                    state = "Connecticut";
                    break;
                case "HI":
                    state = "Havai";
                    break;
                case "IN":
                    state = "Indiana";
                    break;
                case "KS":
                    state = "Kansas";
                    break;
                case "KY":
                    state = "Kentucky";
                    break;
                case "OK":
                    state = "Oklahoma";
                    break;
                case "LA":
                    state = "Luisiana";
                    break;
                case "MA":
                    state = "Massachusetts";
                    break;
                case "MD":
                    state = "Maryland";
                    break;
                case "MN":
                    state = "Minnesota";
                    break;
                case "MO":
                    state = "Missouri";
                    break;
                case "NC":
                    state = "North Carolina";
                    break;
                case "NE":
                    state = "Nebraska";
                    break;
                case "NE (From Diamond Princess)":
                    state = "Nebraska (From Diamond Princess)";
                    break;
                case "NH":
                    state = "New Hampshire";
                    break;
                case "NJ":
                    state = "New Jersey";
                    break;
                case "ON":
                    state = "Ontario";
                    break;
                case "TX":
                    state = "Texas";
                    break;
                case "TX (From Diamond Princess)":
                    state = "Texas (From Diamond Princess)";
                    break;
                case "VT":
                    state = "Vermont";
                    break;
                case "AZ":
                    state = "Arizona";
                    break;
                case "IA":
                    state = "Iowa";
                    break;
                case "U.S.":
                    state = "";
                    break;
                case "Denmark":
                    state = "";
                    break;
                case "Netherlands":
                    state = "";
                    break;
                case "United Kingdom":
                    state = "";
                    break;
                case "UK":
                    state = "";
                    break;
                case "France":
                    state = "";
                    break;
                case "Taiwan":
                    state = "";
                    break;
                default:
                    state = ProvinceState;
                    break;
            }
            return state.Trim();
        }

        public static String FormatsCoordinate(string coordinate)
        {
            if (coordinate != "")
            {
                if (coordinate.Contains("."))
                {
                    string[] separateCoordinate = coordinate.Split('.');

                    if (separateCoordinate[1].Length > 8)
                    {
                        var str = separateCoordinate[1].Remove(7, separateCoordinate[1].Length - 8);
                        separateCoordinate[1] = str;
                    }

                    while (separateCoordinate[1].Length < 8)
                    {
                        separateCoordinate[1] += "0";
                    }
                    coordinate = separateCoordinate[0] + "," + separateCoordinate[1];
                }
                else
                {
                    coordinate += ",0000000";
                }
            }

            if (coordinate != "")
            {
                decimal intCoordinate = Convert.ToDecimal(coordinate);
                
                if(intCoordinate == 0)
                {
                    coordinate = "";
                }
            }
            return coordinate.Trim();
        }
    }
}
