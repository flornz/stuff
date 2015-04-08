using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Web.Script.Serialization;

namespace GeoCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test");
            Application.Exit();
        }

        //        void testJson()
        //        {
        //            string stringFullOfJson = @"
        //{
        //       ""results"" : [
        //          {
        //             ""address_components"" : [
        //                {
        //                   ""long_name"" : ""1600"",
        //                   ""short_name"" : ""1600"",
        //                   ""types"" : [ ""street_number"" ]
        //                },
        //                {
        //                   ""long_name"" : ""Amphitheatre Pkwy"",
        //                   ""short_name"" : ""Amphitheatre Pkwy"",
        //                   ""types"" : [ ""route"" ]
        //                },
        //                {
        //                   ""long_name"" : ""Mountain View"",
        //                   ""short_name"" : ""Mountain View"",
        //                   ""types"" : [ ""locality"", ""political"" ]
        //                },
        //                {
        //                   ""long_name"" : ""Santa Clara"",
        //                   ""short_name"" : ""Santa Clara"",
        //                   ""types"" : [ ""administrative_area_level_2"", ""political"" ]
        //                },
        //                {
        //                   ""long_name"" : ""California"",
        //                   ""short_name"" : ""CA"",
        //                   ""types"" : [ ""administrative_area_level_1"", ""political"" ]
        //                },
        //                {
        //                   ""long_name"" : ""United States"",
        //                   ""short_name"" : ""US"",
        //                   ""types"" : [ ""country"", ""political"" ]
        //                },
        //                {
        //                   ""long_name"" : ""94043"",
        //                   ""short_name"" : ""94043"",
        //                   ""types"" : [ ""postal_code"" ]
        //                }
        //             ],
        //             ""formatted_address"" : ""1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA"",
        //             ""geometry"" : {
        //                ""location"" : {
        //                   ""lat"" : 37.42291810,
        //                   ""lng"" : -122.08542120
        //                },
        //                ""location_type"" : ""ROOFTOP"",
        //                ""viewport"" : {
        //
        //
        //           ""northeast"" : {
        //                  ""lat"" : 37.42426708029149,
        //                  ""lng"" : -122.0840722197085
        //               },
        //               ""southwest"" : {
        //                  ""lat"" : 37.42156911970850,
        //                  ""lng"" : -122.0867701802915
        //               }
        //            }
        //         },
        //         ""types"" : [ ""street_address"" ]
        //      }
        //   ],
        //   ""status"" : ""OK""
        //}   
        //";


        //            JObject jObj = JObject.Parse(stringFullOfJson);

        //            JArray results = jObj["results"] as JArray;

        //            JToken firstResult = results.First;
        //            JToken location = firstResult["geometry"]["location"];


        //            var Latitude = Convert.ToDouble(location["lat"].ToString());
        //            var Longitude = Convert.ToDouble(location["lng"].ToString());


        //        }


        public static dynamic builtInSerializer(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.Deserialize<dynamic>(json);
            return obj;
        }

        // ret words if certain letters are together
        void dispDTs(DataSet ds1, string letT)
        {

            Console.WriteLine(letT.ToUpper());
            
            string combo = "";
            string currW;
            for (int ctr = 0; ctr < ds1.Tables.Count; ctr++)
            {
                combo = "";
                foreach (DataRow row in ds1.Tables[ctr].Rows)
                {
                    currW = row[0].ToString();
                    if (currW.Contains(letT) && currW.Length> letT.Length)
                    {
                        combo += String.Format("  {0}", currW);

                    }

                    //Console.WriteLine("{0}: {1}", row[0].ToString().ToUpper(), row[2].ToString());
                }
                if (combo.Length>1)
                {
                 Console.WriteLine(ds1.Tables[ctr].TableName);
                 Console.WriteLine(combo);
                  
                }
            }
        }

        //void dispDTsMW(DataSet ds1)
        //{
        //    string combo = "";
        //    for (int ctr = 0; ctr < ds1.Tables.Count; ctr++)
        //    {
        //        Console.WriteLine(ds1.Tables[ctr].TableName);
        //        combo = "";
        //        foreach (DataRow row in ds1.Tables[ctr].Rows)
        //        {
        //            combo += String.Format("  {0}: {1}", row[0].ToString().ToUpper(), row[1].ToString());                    
        //        }
        //        Console.WriteLine(combo);
        //    }
        //}

        bool addlLetterInList(char l, char[] oC)
        {
            for (int i = 0; i < oC.Length; i++)
            {
                if (l == oC[i])
                {
                    return true;
                }
            }

            return false;
        }


        void dispDTsMW(DataSet ds1, string oL)
        {
            string currW;
            string aL;
            string combo = "";
            for (int ctr = 0; ctr < ds1.Tables.Count; ctr++)
            {
                Console.WriteLine(ds1.Tables[ctr].TableName);
                combo = "";
                foreach (DataRow row in ds1.Tables[ctr].Rows)
                {
                    currW = row[1].ToString();
                    aL = row[0].ToString();
                    if (oL.Contains(aL))
                    {
                        combo += String.Format("  {0}: {1}", row[0].ToString().ToUpper(), currW);

                    }
                }
                Console.WriteLine(combo);
            }

        }

        //void gMap()
        //{
        //    string ptB = "vancouver, bc";
        //    Distance.Units dUnit = Distance.Units.Kilometres;
        //    //string ptA = "277 Orton Park Road, Toronto, ON";
        //    string ptA = "190 Vauxhall Dr, Toronto, ON";

        //    double? d = 0;
        //    Console.WriteLine("Driving from " + ptA);
        //    Console.WriteLine(" ");
        //    for (int i = 0; i < Tester.nBrnch.Length; i++)
        //    {
        //        ptB = Tester.nBrnch[i] + ", Toronto, ON";
        //        d = Tester.getDrivingDistanceMetres(ptA, ptB) / 1000.0;
        //        Console.WriteLine(String.Format("to {0}: {1} {2}", ptB, d, dUnit));
        //    }
        //    Application.Exit();


        //    var ll1 = Distance.addressToLongLat(ptA);
        //    if (!ll1.HasValue) return;

        //    DialogResult result = MessageBox.Show("warning: mind the geocode limit. proceed?", "Confirmation", MessageBoxButtons.YesNoCancel);
        //    if (result == DialogResult.Yes)
        //    {
        //        for (int i = 0; i < GeoCode.Const1.tplBranches.Length; i++)
        //        {
        //            d = Distance.BetweenTwoLocations(ll1.Value, ptB, dUnit);
        //            Console.WriteLine(String.Format("From {0} to {1}: {2} {3}", ptA, ptB, d, dUnit));

        //            System.Threading.Thread.Sleep(5000);
        //        }
        //    }
        //    else
        //    {
        //        Application.Exit();
        //    }


        //}

        void dispList(List<string> ls1)
        {
            for (int ctr = 0; ctr < ls1.Count; ctr++)
            {
                Console.WriteLine("{0}", ls1[ctr].ToUpper());
            }
        }


        void getWFWords(string ltrs, string[] letT)
        {
            for (int i = 0; i < letT.Length; i++)
            {
                string html = HtmlTableParser.getHtmlWF(ltrs + letT[i]);
            string htmlRes = HtmlTableParser.getHtmlWFResults(html);
            DataSet ds0 = HtmlTableParser.ParseDataSet(htmlRes);

            dispDTs(ds0, letT[i]);
               
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ltrs = "qdulajr";//toufoer";  
            /*
             * 
             */
            string[] toFind = new string[] { new String('-', 8), new String('-', 7) };
            //string[] toFind = new string[] { "-t---", "o---" };

            try
            {
                string[] letTog = new string[] { "wear"
                    , "mun" 
                    , "nail"
 
                };
                //letTog = null;
                if (letTog != null)
                    getWFWords(ltrs, letTog);

                string html1 = HtmlTableParser.getHtmlMWP1(ltrs);
                string htmlP1 = HtmlTableParser.getHtmlMWP1Res(html1);
                DataSet ds1 = HtmlTableParser.ParseDataSetMW(htmlP1);
                dispDTsMW(ds1, "rnva");
                //    List<string> res1 = HtmlTableParser.getWsForCriteria(ds1, toFind);
                //    dispList(res1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
            Application.Exit();

            //string s1 = Tester.testTableGet();
            // findWords("ntealgh");

            //dynamic dyn1 = builtInSerializer(Tester.routesJson1);
            //int dMtr = (int)dyn1["routes"][0]["legs"][0]["distance"]["value"];
            //string dMtr1 = (string)dyn1["routes"][0]["legs"][0]["distance"]["text"];

            //testJson();
        }
    }
}
