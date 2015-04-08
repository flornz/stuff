using System;
using System.Net;
using System.Web;
using System.Data;
using System.Text;
//using System.Web.Extensions;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Web.Script.Serialization;


namespace GeoCode
{
    class Tester
    {

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        public static string tblHTML1 = @"
            <a name=""6""></a><span class=""table_title"">6 Letter Words</span><table class='searchResults' id='results_6'><thead><tr><th>Word</th><th>ScrabbleÂ® Points</th><th>Words With FriendsÂ® Points</th></tr></thead><tbody><tr><td>gelant</td><td>7</td><td>10</td></tr><tr><td><a href='http://www.yourdictionary.com/tangle'>tangle</a></td><td>7</td><td>10</td></tr><tr><td>hantle</td><td>9</td><td>10</td></tr><tr><td>thenal</td><td>9</td><td>10</td></tr><tr><td><a href='http://www.yourdictionary.com/length'>length</a></td><td>10</td><td>12</td></tr></tbody></table>                <div class='ad-after-table' id=""ad300small-1""><div id=""ad300-Wordfinder-small-1""><script type=""text/javascript"">googletag.cmd.push(function(){googletag.display('ad300-Wordfinder-small-1');});</script></div></div>
</section>
";


        public static string findWords(string w1)
        {
            string wf1 = @"http://wordfinder.yourdictionary.com/unscramble/";
            using (var client = new WebClient())
            {
                var url = string.Format("{0}{1}", wf1, w1);
                var sHTML = GetBytes(client.DownloadString(url));

                //byte[] sHTML1 = GetBytes(sHTML);
                String source = Encoding.GetEncoding("utf-8").GetString(sHTML, 0, sHTML.Length - 1);
                source = WebUtility.HtmlDecode(source);
                
                HtmlDocument res = new HtmlDocument();
                res.OptionUseIdAttribute = true;
                res.LoadHtml(source);

                string r = res.ToString();
            }

            return ("");
        }



        public static Int32? getDrivingDistanceMetres(string o, string d)
        {
            using (var client = new WebClient())
            {
                var url = string.Format("https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}", HttpUtility.UrlEncode(o), HttpUtility.UrlEncode(d));
                var rJson = client.DownloadString(url);

                return getDrivingDistanceMetres(rJson);
            }
        }

        public static Int32? getDrivingDistanceMetres(string routesJson)
        {
            JObject jObj = JObject.Parse(routesJson);
            JArray results = jObj["routes"] as JArray;
            JToken firstResult = results.First;
            JArray legs = firstResult["legs"] as JArray;
            JToken distance = legs.First["distance"];

            try
            {
                return (Int32.Parse(distance["value"].ToString()));
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        public static string[] nBrnch = new string[] { 
            "567 Sheppard Avenue East"
            , "941 Progress Ave"
            //"545 Markham Road" 
            //, "30 Sewells Road"
            //, "123 Guildwood Parkway"
            //, "2862 Ellesmere Rd"
        };


        public static string routesJson1 = @"
{
   ""routes"" : [
      {
         ""bounds"" : {
            ""northeast"" : {
               ""lat"" : 43.6850582,
               ""lng"" : -79.34006980000001
            },
            ""southwest"" : {
               ""lat"" : 43.6582091,
               ""lng"" : -79.3527342
            }
         },
         ""copyrights"" : ""Map data ©2015 Google"",
         ""legs"" : [
            {
               ""distance"" : {
                  ""text"" : ""4.0 km"",
                  ""value"" : 3981
               },
               ""duration"" : {
                  ""text"" : ""8 mins"",
                  ""value"" : 482
               },
               ""end_address"" : ""911 Pape Avenue, East York, ON M4K 3V1, Canada"",
               ""end_location"" : {
                  ""lat"" : 43.6850582,
                  ""lng"" : -79.34714930000001
               },
               ""start_address"" : ""635 Queen Street East, Toronto, ON M4M 1G4, Canada"",
               ""start_location"" : {
                  ""lat"" : 43.6582091,
                  ""lng"" : -79.3527342
               },
               ""steps"" : [
                  {
                     ""distance"" : {
                        ""text"" : ""1.1 km"",
                        ""value"" : 1067
                     },
                     ""duration"" : {
                        ""text"" : ""2 mins"",
                        ""value"" : 132
                     },
                     ""end_location"" : {
                        ""lat"" : 43.6610592,
                        ""lng"" : -79.34006980000001
                     },
                     ""html_instructions"" : ""Head \u003cb\u003eeast\u003c/b\u003e on \u003cb\u003eQueen St E\u003c/b\u003e toward \u003cb\u003eCarroll St\u003c/b\u003e"",
                     ""polyline"" : {
                        ""points"" : ""y~miGpqicNQiBSmAM}@[wBs@yEu@eFIe@m@mE]aCAMKw@QkA[}BG_@a@cDe@oDq@yEu@oFSyAIm@WiB""
                     },
                     ""start_location"" : {
                        ""lat"" : 43.6582091,
                        ""lng"" : -79.3527342
                     },
                     ""travel_mode"" : ""DRIVING""
                  },
                  {
                     ""distance"" : {
                        ""text"" : ""1.1 km"",
                        ""value"" : 1121
                     },
                     ""duration"" : {
                        ""text"" : ""2 mins"",
                        ""value"" : 140
                     },
                     ""end_location"" : {
                        ""lat"" : 43.670724,
                        ""lng"" : -79.34403499999999
                     },
                     ""html_instructions"" : ""Turn \u003cb\u003eleft\u003c/b\u003e onto \u003cb\u003eCarlaw Ave\u003c/b\u003e"",
                     ""maneuver"" : ""turn-left"",
                     ""polyline"" : {
                        ""points"" : ""spniGlbgcNqFbBaIbC}DhAe@LcF~AyDjAiBl@_D`AgAZ_@Le@N{Bl@c@N}CbA""
                     },
                     ""start_location"" : {
                        ""lat"" : 43.6610592,
                        ""lng"" : -79.34006980000001
                     },
                     ""travel_mode"" : ""DRIVING""
                  },
                  {
                     ""distance"" : {
                        ""text"" : ""0.2 km"",
                        ""value"" : 195
                     },
                     ""duration"" : {
                        ""text"" : ""1 min"",
                        ""value"" : 21
                     },
                     ""end_location"" : {
                        ""lat"" : 43.6712327,
                        ""lng"" : -79.34171649999999
                     },
                     ""html_instructions"" : ""Turn \u003cb\u003eright\u003c/b\u003e onto \u003cb\u003eRiverdale Ave\u003c/b\u003e"",
                     ""maneuver"" : ""turn-right"",
                     ""polyline"" : {
                        ""points"" : ""_mpiGd{gcNuAgLOe@""
                     },
                     ""start_location"" : {
                        ""lat"" : 43.670724,
                        ""lng"" : -79.34403499999999
                     },
                     ""travel_mode"" : ""DRIVING""
                  },
                  {
                     ""distance"" : {
                        ""text"" : ""1.6 km"",
                        ""value"" : 1598
                     },
                     ""duration"" : {
                        ""text"" : ""3 mins"",
                        ""value"" : 189
                     },
                     ""end_location"" : {
                        ""lat"" : 43.6850582,
                        ""lng"" : -79.34714930000001
                     },
                     ""html_instructions"" : ""Turn \u003cb\u003eleft\u003c/b\u003e at the 1st cross street onto \u003cb\u003ePape Ave\u003c/b\u003e\u003cdiv style=\""font-size:0.9em\""\u003eDestination will be on the right\u003c/div\u003e"",
                     ""maneuver"" : ""turn-left"",
                     ""polyline"" : {
                        ""points"" : ""eppiGvlgcNkD`AeD`AoCz@mCz@qCz@eCx@ODuCz@uCz@SHcCt@aDbA_B`@u@TyCv@{I~Be@LaDz@mA\\}Ab@iAZmA\\w@R""
                     },
                     ""start_location"" : {
                        ""lat"" : 43.6712327,
                        ""lng"" : -79.34171649999999
                     },
                     ""travel_mode"" : ""DRIVING""
                  }
               ],
               ""via_waypoint"" : []
            }
         ],
         ""overview_polyline"" : {
            ""points"" : ""y~miGpqicNe@wDi@uDaDsT}@sGkBqNeCqQWiBqFbB_OlEmPfFgF|AeA\\_D|@}CbAuAgLOe@kD`AuH|BuLvDcMvDaGdBoElAqRfFmHpB""
         },
         ""summary"" : ""Queen St E, Carlaw Ave and Pape Ave"",
         ""warnings"" : [],
         ""waypoint_order"" : []
      }
   ],
   ""status"" : ""OK""
}
            ";

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

    }
}
