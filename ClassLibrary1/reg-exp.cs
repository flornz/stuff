using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ClassLibrary1
{
    public class reg_exp
    {
        /// <summary>
        /// simple price retrieval of price from jayJay html
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        static public double retJayPrice1(string HTML)
        {
            const string priceRE = @"(([$])((([0-9]{1,3},)+[0-9]{3})|[0-9]+)(\.[0-9]{2})?)";
            string priceFound = "";
            string value = "";
            double price = 0.0;
            Match value1;
            const string priceRowRE = @"<tr>\s*<th>\s*Price\s*</th>\s*<td>(?<_price>((.|\n)*?))</td>\s*</tr>";

                    // Get a match for all the columns in the row 
                    value1 = match_key1(HTML, priceRowRE);
                    value = value1.Groups["_price"].ToString().Trim();


                    //if ( == "Price")
                    //{
                    //    value = match_key(Row.Value, ColumnExpression);
                    priceFound = match_key(value, priceRE);

                    if (priceFound != null)
                    {
                        priceFound = priceFound.Replace("$", "");
                        price = Convert.ToDouble(priceFound);
                    }

                    //}

            return price;
        }

        /// <summary>
        /// returns the item location using reg ex based on item details
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        static public string retJayLokay(string HTML)
        {
            const string priceRE = @"(([$])((([0-9]{1,3},)+[0-9]{3})|[0-9]+)(\.[0-9]{2})?)";
            string priceFound = "";
            string value = "";
            double price = 0.0;
            Match value1;
            const string priceRowRE = @"<tr>\s*<th>\s*Address\s*</th>\s*<td>(?<_lokay>(.*?))<br/>(.|\n)*</td>\s*</tr>";
            //const string priceRowRE1 = @"<tr>\s*<th>\s*Price\s*</th>\s*<td>(?<_lokay>((.|\n)*?))</td>\s*</tr>";


            // Get a match for all the columns in the row 
            value1 = match_key1(HTML, priceRowRE);
            value = value1.Groups["_lokay"].ToString().Trim();
            return value;
        }

        
        static public double retJayPrice(string HTML)
        {
            const string TableExpression = "<table class=\"ad-attributes\">(?<dtlTbl>((.|\n)*?))</table>";
            string HeaderExpression = "<th>(?<_th>((.|\n)*?))</th>";
            string RowExpression = "<tr>(?<_tr>((.|\n)*?))</tr>";
            string ColumnExpression = "<td>(?<_td>((.|\n)*?))</td>";
            string priceRE = @"(([$])((([0-9]{1,3},)+[0-9]{3})|[0-9]+)(\.[0-9]{2})?)";
            string priceFound = "";
            string value = "";
            double price = 0.0;


            string priceRowRE = @"<tr>\s*<th>\s*Price\s*</th>\s*<td>(?<_price>((.|\n)*?))</td>\s*</tr>";

            // Get a match for all the tables in the HTML 
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each table element 
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag 
                //iCurrentRow = 0;
                //HeadersExist = false;

                //// Add a new table to the DataSet 
                //dt = new DataTable();

                //Get a match for all the rows in the table 

                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                Match value1 = null;

                // Loop through each row element 
                foreach (Match Row in Rows)
                {
                    // Get a match for all the columns in the row 
                    value1 = match_key1(Row.Value, HeaderExpression);

                    if (value1.Groups["_th"].ToString().Trim() == "Price")
                    {
                        value = match_key(Row.Value, ColumnExpression);
                        priceFound = match_key(value, priceRE);

                        if (priceFound != null)
                        {
                            priceFound = priceFound.Replace("$", "");
                            price = Convert.ToDouble(priceFound);
                        }

                        break;
                    }

                    //MatchCollection Headers = Regex.Matches(Row.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    //headers.Add(header);

                    
                    // Get a match for all the columns in the row 
                        //MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                      //  values.Add( Columns[0].Groups[1].ToString());
                }


                // Add the DataRow to the DataTable 
 
            }

            return price;
        }



        static public Match match_key1(string input, string pattern)
        {
            Regex _regex = new Regex(pattern);
            return( _regex.Match(input));
        }


        ///    static Regex _regex = new Regex(@"/content/([a-z0-9\-]+)\.aspx$");
        /// <summary>
        /// This returns the key that is matched within the input.
        /// </summary>
        static public string match_key(string input, string pattern)
        {
            Regex _regex = new Regex(pattern);
            Match match = _regex.Match(input);

            return (match.Success ? match.Groups[0].Value : null);
        }

        static public string match_key(string input, string pattern, RegexOptions re)
        {
            Regex _regex = new Regex(pattern, re);
            Match match = _regex.Match(input.ToLower());

            return (match.Success ? match.Groups[0].Value : null);
        }

        static public void match_select_from(string input, string pattern, ref string sel, ref string from)
        {
            Regex _regex = new Regex(pattern, RegexOptions.Singleline);
            Match match = _regex.Match(input.ToLower());
            if (match.Success)
            {
                sel = match.Groups["m1"].Value;
                from = match.Groups["m2"].Value;
            }
        }

        public static MatchCollection get_matchesMC(string input, string pattern)
        {
            return Regex.Matches(input, pattern);
        }
        
        public static ArrayList get_matches(string input, string pattern)
        {
            ArrayList list = new ArrayList();

            foreach (Match match1 in Regex.Matches(input, pattern))
            {
                //codes += ", \"" + match1.Groups["ps_code"].Value + "\"";
                list.Add(match1.Groups[0].Value);
            }

            return (list);
        }


        public static ArrayList get_matches(string input, string pattern, RegexOptions re)
        {
            ArrayList list = new ArrayList();

            foreach (Match match1 in Regex.Matches(input, pattern, re))
            {
                list.Add(match1.Groups[0].Value);
            }

            return (list);
        }

        public static ArrayList get_matches(string input, string pattern, RegexOptions re, string match_id)
        {
            ArrayList list = new ArrayList();

            foreach (Match match1 in Regex.Matches(input, pattern, re))
            {
                list.Add(match1.Groups[match_id].Value);
            }

            return (list);
        }

        public static ArrayList get_matches(string input, string pattern, string match_id)
        {
            ArrayList list = new ArrayList();

            foreach (Match match1 in Regex.Matches(input, pattern))
            {
                list.Add(match1.Groups[match_id].Value);
            }

            return (list);
        }


        /// <summary>
        /// when getting matches, include the passed Additional Info to the returned arraylist
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="match_id"></param>
        /// <param name="szAddInfo"></param>
        /// <returns></returns>
        public static ArrayList get_matches(string input, string pattern, string match_id, string szAddInfo)
        {
            ArrayList list = new ArrayList();

            foreach (Match match1 in Regex.Matches(input, pattern))
            {
                list.Add(match1.Groups[match_id].Value + "--" + szAddInfo);
            }

            return (list);
        }


        /// <summary>
        ///     when submitting a form from seach page to view page, save search parameters by generating hidden input for search parameters
        ///     generated hidden inputs should be placed in view page
        ///     works only if action method is a post
        ///     replace 
        ///         <input id='asset_tag_number_search_asset' name='svrAssetTagNumber_Search' value='<%=strAssetTagNumber_Search%>' size='20' type='text' maxlength='50' />
        ///     with
        ///         <input type='hidden' id='asset_tag_number_search_asset' name='svrAssetTagNumber_Search'  value='<%=Request.Form("svrAssetTagNumber_Search")%>' />
        /// </summary>
        public class get_hidden_equiv_from_search_param
        {
            public void do_proc()
            {
                string result = "";
                //string _id = "";
                string _nm = "";

                file_io fio = null;
                string file_stream = "";
                string pattern = @"(\<input( id='(?<_id>\w+)')? name='(?<_nm>\w+)' .+ /\>)|(\<select name=(\""|')(?<_nm>\w+)(\""|').*\>)";
                //string File = @"C:\e-profile\AssetWeb.net\SearchHardwareAssets.asp";
                string File = @"C:\0.ved\WebSite\input\TextFile.txt";

                //read file into str var
                fio = new file_io();
                file_stream = fio.ReadFileToStr(File);
                fio = null;

                Regex rx = new Regex(pattern);
                //string result = rx.Replace(file_stream, replacement_str);

                foreach (Match m in Regex.Matches(file_stream, pattern))
                {
                    //_id = m.Groups["_id"].Value;
                    _nm = m.Groups["_nm"].Value;

                    //result += Environment.NewLine + @"<input type='hidden' id='" + _id + "' name='" + _nm + "'  value='<%=Request.Form(\"" + _nm + "\")%>' />";
                    result += Environment.NewLine + @"<input type='hidden' name='" + _nm + "'  value='<%=Request.Form(\"" + _nm + "\")%>' />";
                }
                Console.WriteLine(result);
            }
        }


        public class html_input_name_add_value
        {
            private string replacement_str(Match m)
            {
                string cap_val = m.Groups["val"].Value;
                string replacement = " name='svr" + cap_val + "' value='<%=str" + cap_val + "%>' ";
                return (replacement);
            }

            public void do_replace()
            {
                string File = @"C:\e-profile\AssetWeb.net\HardwareAsset_Add.asp";
                string pattern = @" name='svr(?<val>(\w+))' ";

                file_io fio = null;
                string file_stream = "";
                string result = "";

                //read file into str var
                fio = new file_io();
                file_stream = fio.ReadFileToStr(File);
                fio = null;

                Regex rx = new Regex(pattern);
                result = rx.Replace(file_stream, replacement_str);
                Console.WriteLine(result);
            }
        }


        public class sql_var_to_fld_name
        {
            private string replacement_str(Match m)
            {
                string cap_val = m.Groups["val"].Value;
                string replacement = ", " + cap_val.Substring(4);

                return (replacement);
            }

            public void do_replace()
            {
                string File = @"C:\0.ved\leuqes\crud\_32.sql";
                string pattern = @"(?<val>(, @\w+)) (?<val1>( \w+)\n)";
                pattern = @"(?<val>(, @\w+)) ";

                file_io fio = null;
                string file_stream = "";
                string result = "";

                //read file into str var
                fio = new file_io();
                file_stream = fio.ReadFileToStr(File);
                fio = null;

                Regex rx = new Regex(pattern);
                //result = rx.Replace(file_stream, replacement_str);

                foreach (Match match1 in Regex.Matches(file_stream, pattern))
                {
                    //codes += ", \"" + match1.Groups["ps_code"].Value + "\"";
                    result += ", " + match1.Groups["val"].Value.Substring(3);
                }
                Console.WriteLine(result);
            }
        }

        /// <summary>
        ///         
        ///         Dim strASSET_TAG_NUMBER As String = "855264-verified1"
        ///         Dim strPRODUCT_CODE As String = "HEWQ2477A#ABA"
        ///         Dim strPRODUCT_NAME As String = "HP LaserJet 2300l (lite) 1200dpi 20ppm 32MB PCL6 USB/PAR"
        ///         to
        /// </summary>
        public static void _net_dim_assign_to_parameter()
        {
            string File = AppDomain.CurrentDomain.BaseDirectory + @"\input\textfile.txt";
            string pattern = "";
            string extract = "";

            file_io fio = null;
            string file_stream = "";

            //read file into str var
            fio = new file_io();
            file_stream = fio.ReadFileToStr(File);
            fio = null;

            pattern = @"\<td valign='top'\>(?<val>(.+))\</td\>";
            //pattern = "        ///          strASSET_TAG_NUMBER As String = "855264-verified1"
            pattern = @"Dim (?<val>(\w+)) As ";

            Regex rx = new Regex(pattern);
            //result = rx.Replace(file_stream, replacement_str);

            foreach (Match match1 in Regex.Matches(file_stream, pattern))
            {
                extract += "\n, " + match1.Groups["val"].Value + " _";
            }

            System.Diagnostics.Debug.Write(extract);
        }


        /// <summary>
        /// write properties for this: orig_string, pattern, replacement, etc
        /// </summary>
        public class replace_matches_generic
        {
            private string replacement_str(Match m)
            {
                string cap_val = m.Groups["val"].Value;
                string replacement = "<td><input type='text' value='" + cap_val + "' size='20' /></td>";

                return (replacement);
            }

            public void do_proc1(string File, string pattern)
            {
                file_io fio = null;
                string file_stream = "";
                //string pattern = null;

                //string File = @"C:\Users\fsoliva\Downloads\e-Profile enhancments project\003 Solution Design\mockup-v2\view-warranty.htm";
                //C:\Users\fsoliva\Downloads\e-Profile enhancments project\003 Solution Design\mockup-v2\view-asset-dtl.htm";
                //C:\Users\fsoliva\Downloads\e-Profile enhancments project\003 Solution Design\Mockups\View\purchase-data.htm";

                //read file into str var
                fio = new file_io();
                file_stream = fio.ReadFileToStr(File);
                fio = null;

                pattern = @"\<td valign='top'\>(?<val>(.+))\</td\>";
                //pattern = "(?<tab_name>(.+) \\(Tab Delimited\\))";

                Regex rx = new Regex(pattern);
                string result = rx.Replace(file_stream, replacement_str);
                Console.WriteLine(result);
            }
        }

        public static string get_matches(string File)
        {
            file_io fio = null;
            string file_stream = "";
            string codes = "";

            //read file into str var
            fio = new file_io();
            file_stream = fio.ReadFileToStr(File);
            fio = null;

            string pattern = @"\<option value='(?<ps_code>([A-Z]){2})'\>(?<ps_name>([A-Za-z ])+)\</option\>";

            foreach (Match match1 in Regex.Matches(file_stream, pattern))
            {
                //codes += ", \"" + match1.Groups["ps_code"].Value + "\"";
                codes += ", \"" + match1.Groups["ps_name"].Value + "\"";
            }

            return (codes);
        }

        public static string test_match(string File, string ptrn)
        {
            file_io fio = null;
            string file_stream = "";
            string codes = "";

            //read file into str var
            fio = new file_io();
            file_stream = fio.ReadFileToStr(File);
            fio = null;

            int _mc = Regex.Matches(file_stream, ptrn).Count;

            foreach (Match match1 in Regex.Matches(file_stream, ptrn))
            {
                codes += ", \"" + match1.Groups["id"].Value + "\"";
            }

            return (codes);
        }


        #region "used-for-includes"
        public static List<string> get_matches_ar(string File, string ptrn, string szID)
        {
            List<string> szMatches = new List<string>();
            file_io fio = null;
            string file_stream = "";

            //read file into str var
            fio = new file_io();
            file_stream = fio.ReadFileToStr(File);
            fio = null;

            int _mc = Regex.Matches(file_stream, ptrn).Count;

            foreach (Match match1 in Regex.Matches(file_stream, ptrn))
            {
                szMatches.Add(match1.Groups[szID].Value);
            }

            return (szMatches);
        }

        public static int intLevelsBelow(string szDir)
        {
            string ptrn = @"\.\./";
            return (Regex.Matches(szDir, ptrn).Count);
        }
        #endregion



        #region "NIC_PATSRCH"
        /// <summary>
        /// read entire cs file -- find the webmethods
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static string read_cs_get_wm(string File)
        {
            string pattern = "";
            file_io fio = null;
            string file_stream = "";
            string codes = "";

            //read file into str var
            fio = new file_io();
            file_stream = fio.ReadFileToStr(File);
            fio = null;

            pattern = @"\[WebMethod\(EnableSession=true\)\]";
            pattern += @"\s+public (?<type>\w+) (?<wm_name>\w+)\([ \w,\n]*\)\s+";
            pattern += @"\{";
            //pattern += @"\s+|(<<|>>|\+\+|--|==|\!=|>=|<=|\{|\}|\[|\]|\(|\)|\.|,|:|;|\+|-|\*|/|%|&|\||\^|!|~|=|\<|\>|\?)";
            ////pattern += @"([.\n]+;)+";
            //pattern += @"\}";
            //pattern += @"\s+";
            //pattern += @"((\[WebMethod\(EnableSession=true\)\])|private|\})";

            foreach (Match match1 in Regex.Matches(file_stream, pattern))
            {
                codes += "<br />" + match1.Groups["wm_name"].Value;
            }

            return (codes);
        }
        #endregion


        /// <summary>
        /// recordset fields
        /// </summary>
        public static class get_fields_used_in_code
        {

            public static string get_matches()
            {
                string pattern = "";
                string file_stream = @"
		dDOB = RS('DDOB')
		szFirst = RS('szFirst')
		szLast = RS('szLast')
		szMiddle=RS('szMiddle')
		szStreet = Trim(RS('szStreet') & ' ' & RS('szStreet2'))
		szCity = RS('szCity')
		szProvince = RS('szState')
		If Not IsNull(RS('nZip')) Then
			szPostalCode = Replace(RS('nZip'),' ','')
		Else
			szPostalCode = ''
		End If

		szEmployeePhoneArea = RS('szPhoneArea')
		szEmployeePhone1 = Trim(RS('szPhone1'))
		szEmployeePhone2 = Trim(RS('szPhone2'))
		szPhoneExt= Trim('' & RS('szExtension'))
		szPatientPhone= '(' & szEmployeePhoneArea & ') ' & szEmployeePhone1 & ' ' & szEmployeePhone2

		szChartNum = RS('szChartNum')

		If RS('szMiddle') <> '' Then
			szInitial = Mid(RS('szMiddle'),1,1)
		End If

		If RS('nSex') = 1 Then
			szSex = 'M'
		ElseIf RS('nSex') = 2 Then
			szSex = 'F'
		End If
";

                pattern = @"RS\('(?<wm_name>\w+)'\)";
                //pattern = @"RS\('DDOB'\)";
                //pattern += @"\s+public (?<type>\w+) (?<wm_name>\w+)\([ \w,\n]*\)\s+";

                foreach (Match match1 in Regex.Matches(file_stream, pattern))
                {
                    Console.WriteLine(", " + match1.Groups["wm_name"].Value);
                }

                return ("");
            }

        }

    }
}
