/*
 SQL staement to fields
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
    {
    public class sql_to_fld
        {

//        public static string file_stream = @"
//
//SELECT 
//	[RU_MATRIX_ID]
//      , [ASSET_CLASS]
//      , [ASSET_CLASS_DESC] = AC.REF_VALUE_DESC
//      , [ASSET_TYPE]
//      , [ASSET_TYPE_DESC] = AT.REF_VALUE_DESC
//      , [T_RUMatrix].[MANUFACTURER_ID]
//      , [MANUFACTURER] = MN.[MANUFACTURER_NAME]
//      , [T_RUMatrix].[SERVICE_PROVIDER_ID]
//      , [SERVICE_PROVIDER] = SP.[SERVICE_PROVIDER_NAME]
//      , [SLA_TYPE]
//      , [SLA] = SLA.REF_VALUE_DESC
//
//      , [IN_WARRANTY]
//      , [IN_WARRANTY_DESC] = case [IN_WARRANTY]
//			when 'T' then 'Yes'
//			when 'F' then 'No'
//			else 'Any'
//		end
//	      
//      , [IS_BREAK_FIX_ONLY]
//      , [IS_BREAK_FIX_ONLY_DESC] = case [IS_BREAK_FIX_ONLY]
//			when 'T' then 'Yes'
//			when 'F' then 'No'
//			else 'Any'
//		end
//
//      , [T_RUMatrix].[COUNTRY_ID]
//      , [COUNTRY] = CO.[COUNTRY_NAME]
//      , [RU_RATE_ID_1]
//      , [RU_CODE_1] = RU1.RU_CODE
//      , [RU_RATE_ID_2]
//      , [RU_CODE_2] = RU2.RU_CODE
//      , [IS_EXCEPTION]
//
//FROM 
//	[dbo].[T_RUMatrix]
//	INNER JOIN dbo.T_ReferenceValue AC ON [dbo].[T_RUMatrix].[ASSET_CLASS] = AC.REF_CODE
//	LEFT JOIN dbo.T_ReferenceValue AT ON [dbo].[T_RUMatrix].[ASSET_TYPE] = AT.REF_CODE
//	LEFT JOIN dbo.[T_MANUFACTURER] MN on [dbo].[T_RUMatrix].[MANUFACTURER_ID] = MN.[MANUFACTURER_ID]
//	LEFT JOIN dbo.[T_ServiceProvider] SP on [dbo].[T_RUMatrix].[SERVICE_PROVIDER_ID] = SP.[SERVICE_PROVIDER_ID]
//	INNER JOIN dbo.T_ReferenceValue SLA ON [SLA_TYPE] = SLA.REF_CODE
//	INNER JOIN dbo.[T_COUNTRY] CO on [dbo].[T_RUMatrix].[COUNTRY_ID] = CO.[COUNTRY_ID]
//	LEFT JOIN dbo.[T_RURATE] RU1 on [dbo].[T_RUMatrix].[RU_RATE_ID_1] = RU1.[RU_RATE_ID]
//	LEFT JOIN dbo.[T_RURATE] RU2 on [dbo].[T_RUMatrix].[RU_RATE_ID_2] = RU2.[RU_RATE_ID]
//
//                ";

        // worjing sql string here

//SELECT 
//	[RU_MATRIX_ID]
//      , [ASSET_CLASS]
//      , [ASSET_TYPE]
//      , [T_RUMatrix].[MANUFACTURER_ID]
//      , [T_RUMatrix].[SERVICE_PROVIDER_ID]
//      , [SLA_TYPE]
//      , [IN_WARRANTY]
//      , [IS_BREAK_FIX_ONLY]
//      , [T_RUMatrix].[COUNTRY_ID]
//      , [RU_RATE_ID_1]
//      , [RU_RATE_ID_2]
//FROM 
//	[dbo].[T_RUMatrix]
//	INNER JOIN dbo.T_ReferenceValue AC ON [dbo].[T_RUMatrix].[ASSET_CLASS] = AC.REF_CODE
//	LEFT JOIN dbo.T_ReferenceValue AT ON [dbo].[T_RUMatrix].[ASSET_TYPE] = AT.REF_CODE
//	LEFT JOIN dbo.[T_MANUFACTURER] MN on [dbo].[T_RUMatrix].[MANUFACTURER_ID] = MN.[MANUFACTURER_ID]
//	LEFT JOIN dbo.[T_ServiceProvider] SP on [dbo].[T_RUMatrix].[SERVICE_PROVIDER_ID] = SP.[SERVICE_PROVIDER_ID]
//	INNER JOIN dbo.T_ReferenceValue SLA ON [SLA_TYPE] = SLA.REF_CODE
//	INNER JOIN dbo.[T_COUNTRY] CO on [dbo].[T_RUMatrix].[COUNTRY_ID] = CO.[COUNTRY_ID]
//	LEFT JOIN dbo.[T_RURATE] RU1 on [dbo].[T_RUMatrix].[RU_RATE_ID_1] = RU1.[RU_RATE_ID]
//	LEFT JOIN dbo.[T_RURATE] RU2 on [dbo].[T_RUMatrix].[RU_RATE_ID_2] = RU2.[RU_RATE_ID]

        public static string file_stream = @"

	SELECT top 4
		CL.[CLASSIFICATION_ID]
		, [REF_CODE]
		, CL.[SCHEME_NAME] 
		, ASSET_CLASS = CASE 
			WHEN CL.[SCHEME_NAME] = 'WORKSTATION_TYPE' THEN 'Workstation'
			WHEN CL.[SCHEME_NAME] = 'SERVER_TYPE' THEN 'Server'
			WHEN CL.[SCHEME_NAME] = 'PRINTER_TYPE' THEN 'Printer'
			WHEN CL.[SCHEME_NAME] = 'PERIPHERAL_TYPE' THEN 'Peripheral'
		END
		
		, [REF_VALUE_DESC]
		, [REF_VALUE_DESC_FR]
		, RV.[IS_LOGICAL_DELETED]
	    
	FROM 
		dbo.T_Classification cl INNER JOIN dbo.T_ReferenceValue rv ON cl.CLASSIFICATION_ID = rv.CLASSIFICATION_ID
	WHERE 
		CL.IS_LOGICAL_DELETED = 'F'

                ";


        public static string flds_from_sql(string cs, bool dr_t_tbl)
            {
            // step: obtain sql        string startupPath = AppDomain.CurrentDomain.BaseDirectory;//System.IO.Directory.GetCurrentDirectory();
            string in_sql_file = AppDomain.CurrentDomain.BaseDirectory + @"input\TextFile2.txt";
            string ptrn = @"";
            ptrn = @"(?<m1>select .+)(?<m2>from .+)";
            //file_stream = "select * from table";// ClassLibrary1.file_io.ReadFileToStr1(in_sql_file);

            // div sql between select fields and from table
            string sel = "";
            string from = "";
            ClassLibrary1.reg_exp.match_select_from(file_stream, ptrn, ref sel, ref from);

            // create tmp table name
            string context = "matrix";
            string t_tbl = "_" + context + "_" + DateTime.Now.ToLongTimeString().Replace(":", "").Replace(" ", "");

            // drop table from db if necessary
            string drop_tbl_sql = @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + t_tbl + "]') AND type in (N'U')) DROP TABLE [dbo].[" + t_tbl + "]";
            ClassLibrary1.db_fld.exec_sql(cs, drop_tbl_sql);

            // create table
            string create_tmp_tbl = sel.ToUpper() + " into [" + t_tbl + "] \n\r " + from;
            ClassLibrary1.db_fld.exec_sql(cs, create_tmp_tbl);

            string rcc = ClassLibrary1.sql_to_fld.ret_class_code_v2(cs, t_tbl);
            //string rcc = ClassLibrary1.sql_to_fld.ret_class_code_search_byid(cs, t_tbl);
            

            // drop table from db
            if (dr_t_tbl)
                {
                ClassLibrary1.db_fld.exec_sql(cs, drop_tbl_sql);
                }

            return (rcc);
            } // void return_field_names


        /// <summary>
        /// manipulate dt as oppose to saving to arays first
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static string ret_class_code_v2(string cs, string tbl)
            {
            //string outFile = @"C:\0.ved\WebSite\output\TextFile.txt";
            string crlf_str = "<br />"; //vbcrlf
            string res_str = "";
            string[] flds1 = new string[] { "IS_LOGICAL_DELETED", "INSERT_DATE", "INSERT_EMPLOYEE_ID", "LM_DATE", "LM_EMPLOYEE_ID", "GLOBAL_UNIQUE_ID" };
            string curr_col = "";

            StringBuilder props_sb = null;
            StringBuilder cmd_prm_sb = null;
            StringBuilder srch_prm_sb = null;
            StringBuilder rd_curr_sb = null;
            StringBuilder test_ftn_prm_sb = null;
            StringBuilder html_search_hdr_sb = null;
            StringBuilder html_search_sb = null;
            StringBuilder html_asp_var_assign_sb = null;

            StringBuilder gen_sb = null;

            ArrayList my_AL = new ArrayList();
            ArrayList al_data_types = new ArrayList();
            ArrayList al_data_sizes = new ArrayList();
            string orig_fld_name = "";
            string disp_name = "";
            string fld_name = "";
            string fld_type = "";
            string fld_size = "";

            string svr_name = "";
            string str_name = "";
            string svr_id = "";

            string str_dbg_tmp = "";
            string str_dll_sub_call = "";
            string str_dbg_bef_sub_call = "";
            string str_dbg_for_dll_test_cs = "";
            string back_to_search = "";

            System.Data.DataTable dt = null;//reader.GetSchemaTable(); 

            //ClassLibrary1.db_fld.return_field_names_types(ref my_AL, ref al_data_types, conString, tbl);
            dt = ClassLibrary1.db_fld.return_field_names_types_sizes(ref my_AL, ref al_data_types, ref al_data_sizes, cs, tbl);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            srch_prm_sb = new StringBuilder();
            cmd_prm_sb = new StringBuilder();
            props_sb = new StringBuilder();
            rd_curr_sb = new StringBuilder();
            test_ftn_prm_sb = new StringBuilder(); 
            
            html_search_sb = new StringBuilder();
            html_search_hdr_sb = new StringBuilder();
            html_asp_var_assign_sb = new StringBuilder();

            /// dbg return_field_names_types_sizes dt
            foreach (System.Data.DataRow row in dt.Rows)
                {
                orig_fld_name = row["ColumnName"].ToString();
                if (flds1.Contains(curr_col) == false)
                    {
                    fld_name = ClassLibrary1.str_ftns.lower_underscore_to_capitalize(orig_fld_name);
                    disp_name = ClassLibrary1.str_ftns.lower_underscore_to_display(orig_fld_name);

                    svr_name = "svr" + fld_name;
                    str_name = "str" + fld_name;
                    svr_id = orig_fld_name.ToLower();// +"_search";
                    //fld_size = al_data_sizes[ctr].ToString();

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    gen_sb = new StringBuilder();
                    gen_sb.Append(Environment.NewLine + ", ByVal " + str_name + " As String _");
                    gen_sb = null;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //Response.Write("<br />Column Name: " + row["ColumnName"]);
                    al_data_types.Add(row["DataType"]);
                    al_data_sizes.Add(row["ColumnSize"]);

                    //Response.Write("<br />Data Type: " + row["DataType"]);
                    //Response.Write("<br />Is Unique: " + row["IsUnique"]);
                    //Response.Write("<br />Allow DBNull: " + row["AllowDBNull"]);
                    //Response.Write("<br />Is Key: " + row["IsKey"]);

                    /////////////////////////            /////////////////////////            /////////////////////////
                    string str_action = "_Update";
                    str_action = "_Search";
                    str_dll_sub_call += Environment.NewLine + "\t, " + str_name + "_Update _";
                    // in asp page, write variable values to screen
                    str_dbg_bef_sub_call += Environment.NewLine + "Response.Write(\"<br />" + str_name + "_Update: \" & " + str_name + "_Update)";
                    // in asp page, write ASP/.Net code to screen that can be used to debug DLL
                    str_dbg_for_dll_test_cs += Environment.NewLine + "Response.Write(\"<br />string " + str_name + "_Update = \"\"\" & " + str_name + "_Update & \"\"\";\")";

                    //<!-- hiddenvar for back to search form -->
                    back_to_search += Environment.NewLine + "<input type='hidden' name='" + svr_name + str_action + "' value='<%=Request.Form(\"" + svr_name + str_action + "\")%>' />";
                    str_dbg_tmp += ""
                        //+ "\"', '\" & objRU." + fld_name
                        //+ Environment.NewLine + "document.RUMatrixSearchForm." + svr_name + str_action + ".value = matrix_values[id][0];"
                        + Environment.NewLine + "<input type='hidden' name='" + svr_name + str_action + "' />"
                        ;

                    //read_new_values_for_edit += Environment.NewLine + str_name + "_Update = Trim(Request.Form(\"" + svr_name + "_Update\"))";

                    rd_curr_sb.Append(Environment.NewLine + "m" + fld_name + str_action + " = objRdr.Item(\"" + orig_fld_name + "\").ToString()");
                    html_asp_var_assign_sb.Append(Environment.NewLine + str_name + str_action + " = Trim(Request.Form(\"" + svr_name + str_action + "\"))");
                    html_search_hdr_sb.Append(Environment.NewLine + "<th width='' align='left'>" + disp_name + "</th>");
                    html_search_sb.Append(Environment.NewLine + "<td width='' align='left'><input name='" + svr_name + "' type='text' id='" + svr_id + "' value='<%=Server.HTMLEncode(" + svr_name + ") %>' size='20' maxlength='" + fld_size + "' /></td>");

                    props_sb.Append(Environment.NewLine + Environment.NewLine + "Private m" + fld_name + " As String");
                    props_sb.Append(Environment.NewLine + "Public ReadOnly Property " + fld_name + " As String");
                    props_sb.Append(Environment.NewLine + "Get");
                    props_sb.Append(Environment.NewLine + fld_name + " = m" + fld_name);
                    props_sb.Append(Environment.NewLine + "End Get");
                    props_sb.Append(Environment.NewLine + "End Property");

                    srch_prm_sb.Append("<br />, ByVal str" + fld_name + " As String _");

                    /////////////////////////            /////////////////////////            /////////////////////////

                    /////////////////////////
                    test_ftn_prm_sb.Append("<br />, str" + fld_name + "_Search _");

                    /////////////////////////
                    fld_type = row["DataType"].ToString();
                    // this is the search parameter; nullify all fileds
                    cmd_prm_sb.Append("<br />objCmd.Parameters.AddWithValue(\"@" + orig_fld_name + "\", Nullify.");

                    switch (fld_type)
                        {
                        case "datetime":
                            cmd_prm_sb.Append("DateTimeString(str" + fld_name + "))");
                            break;
                        case "bit":
                            cmd_prm_sb.Append("Bit(str" + fld_name + "))");
                            break;
                        case "int":
                            cmd_prm_sb.Append("Int32(str" + fld_name + "))");
                            break;
                        default:
                            cmd_prm_sb.Append("String(str" + fld_name + "))");
                            //cmd_prm_sb.Append("str" + fld_name + ")");
                            break;
                        }
                    /////////////////////////
                    }
                }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            my_AL = null;

            res_str = srch_prm_sb.ToString();
            res_str += crlf_str + cmd_prm_sb.ToString();
            res_str += crlf_str + "#Region \"Fields\"" + props_sb.ToString() + crlf_str + "\n#End Region";
            res_str += crlf_str + rd_curr_sb.ToString();
            res_str += crlf_str + test_ftn_prm_sb.ToString();

            string str_search1 =

                ""
                //html_asp_var_assign_sb.ToString()
                //+ Environment.NewLine + str_dbg_bef_sub_call
                + Environment.NewLine + str_dbg_tmp
                //+ Environment.NewLine + str_dbg_for_dll_test_cs
                //+ Environment.NewLine + "Call objRU.Update( _" + Environment.NewLine + str_dll_sub_call.Substring(4) + Environment.NewLine +  ")"
                //+ Environment.NewLine + props_sb.ToString()
                //+ Environment.NewLine + rd_curr_sb.ToString()
                //+ Environment.NewLine + html_search_hdr_sb.ToString()
                //+ Environment.NewLine + html_search_sb.ToString()

                ;

            //if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);
            //System.IO.File.WriteAllText(outFile, str_search1);

            gen_sb = null;

            html_asp_var_assign_sb = null;
            html_search_hdr_sb = null;
            html_search_sb = null;
            test_ftn_prm_sb = null;
            rd_curr_sb = null;
            props_sb = null;
            srch_prm_sb = null;

            return (str_search1);
            }

        public static string ret_class_code_search_byid(string cs, string tbl)
            {
            //string res_str = "";
            string[] non_search_flds = new string[] { "IS_LOGICAL_DELETED", "INSERT_DATE", "INSERT_EMPLOYEE_ID", "LM_DATE", "LM_EMPLOYEE_ID", "GLOBAL_UNIQUE_ID" };

            StringBuilder gen_sb = null;
            StringBuilder gen_sb1 = null;

            ArrayList my_AL = new ArrayList();
            ArrayList al_data_types = new ArrayList();
            ArrayList al_data_sizes = new ArrayList();
            string orig_fld_name = "";
            string disp_name = "";
            string fld_name = "";

            string svr_name = "";
            string str_name = "";
            string svr_id = "";

            bool is_pk = false;
            bool is_nullable = false;
            string pk_out = "";
            string pk_out1 = "";

            System.Data.DataTable dt = null;//reader.GetSchemaTable();

            //ClassLibrary1.db_fld.return_field_names_types(ref my_AL, ref al_data_types, cs, tbl);
            dt = ClassLibrary1.db_fld.return_schema_table_tsql(cs, tbl);

            gen_sb = new StringBuilder();
            gen_sb1 = new StringBuilder();
            string byid_sub = "";


            string const_1 = @"
        'mHAError = ""

        Try
            objConn = New Data.SqlClient.SqlConnection(mSQLConnStr)
            objConn.Open()
            objCmd = New Data.SqlClient.SqlCommand(mSearchSP, objConn)
            objCmd.CommandType = Data.CommandType.StoredProcedure

        ";

            string const_2 = @"
            objRdr = objCmd.ExecuteReader()

            If objRdr.Read() Then
                mReaderOpen = True
                mReaderEOF = False
                mReaderCount = 1
                mRowId = ""1""

        ";

            string const_3 = @"
                Else
                    mReaderOpen = False
                    mReaderEOF = True
                    mReaderCount = 0
                End If

            Catch ex As Exception
                'mHAError = ex.ToString()

                mReaderOpen = False
                mReaderEOF = True
                mReaderCount = 0

                Call CloseRdrCmdConn()

                Throw (ex)
            End Try
        End Sub
        ";


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// dbg return_field_names_types_sizes dt
            /// 
            foreach (System.Data.DataRow row in dt.Rows)
                {
                orig_fld_name = row["COLUMN_NAME"].ToString();
                is_pk = row["IS_PK"].ToString() == "NOT_A_PK" ? false : true;

                fld_name = ClassLibrary1.str_ftns.lower_underscore_to_capitalize(orig_fld_name);
                svr_name = "svr" + fld_name;
                str_name = "str" + fld_name;
                svr_id = orig_fld_name.ToLower() + "_search";

                if (non_search_flds.Contains(orig_fld_name) == false && is_pk == false)
                    {
                    //Response.Write("<br />");
                    disp_name = ClassLibrary1.str_ftns.lower_underscore_to_display(orig_fld_name);
                    is_nullable = row["IS_NULLABLE"].ToString() == "0" ? false : true;

                    //fld_size = al_data_sizes[ctr].ToString();

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    gen_sb.Append(Environment.NewLine + ", ByVal " + str_name + "_Ins As String _");
                    gen_sb1.Append(Environment.NewLine + "m" + fld_name + " = objRdr.Item(\"" + orig_fld_name + "\").ToString()");
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                if (is_pk == true)
                    {
                    pk_out = "Public Sub SearchById(ByVal " + str_name + " As String)";
                    pk_out1 = "objCmd.Parameters.AddWithValue(\"@" + orig_fld_name + "\", " + str_name + ")";
                    }

                }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            byid_sub = Environment.NewLine + pk_out;
            //byid_sub += Environment.NewLine + gen_sb.ToString().Substring(4);
            byid_sub += Environment.NewLine + const_1;
            byid_sub += Environment.NewLine + pk_out1;
            byid_sub += Environment.NewLine + const_2;
            byid_sub += Environment.NewLine + gen_sb1.ToString().Substring(2);
            byid_sub += Environment.NewLine + const_3;


            //if (System.IO.File.Exists(outFile)) System.IO.File.Delete(outFile);
            //string outFile = @"C:\0.ved\WebSite\output\TextFile.txt";
            //System.IO.File.AppendAllText(outFile, byid_sub);

            gen_sb = null;
            gen_sb1 = null;

            return (byid_sub);
            }
        
        }
    }
