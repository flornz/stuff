using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace my_utils
{
    public class read_xls
    {
        protected static System.Data.DataTable get_tbl_from_cmd(System.Data.OleDb.OleDbCommand ole_cmd)
        {
            System.Data.DataTable ole_tbl =null;
            System.Data.OleDb.OleDbDataAdapter ole_da = null;
            //= new DataTable();
            using (ole_da = new System.Data.OleDb.OleDbDataAdapter())
            {
                ole_da.SelectCommand = ole_cmd;
                using (ole_tbl = new System.Data.DataTable())
                {
                    ole_da.Fill(ole_tbl);
                    return (ole_tbl);
                }
            }
        }

        /// <summary>
        /// uses Microsoft.Jet.OLEDB.4 to see if a connection to an xls file is possible
        /// should be used mainly for debugging
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public bool can_connect (string fn) {
            System.Data.OleDb.OleDbConnection ole_con = null;
            string connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=";
            connection += fn + ";";

            try
            {
                ole_con = new System.Data.OleDb.OleDbConnection(connection);
                ole_con.Open();
                return (true);
            }
            catch 
            {
                return (false);
            }

            finally
            {
                if (ole_con != null) { ole_con.Dispose(); ole_con = null; }
            }
        
        }

        public void ole_xls_tbl_v1(string fn, string sn)
        {
            string connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=";//C:\Users\fsoliva\Downloads\Log.xls";
            System.Data.OleDb.OleDbConnection ole_con = null;//ew System.Data.OleDb.OleDbConnection(connection);
            System.Data.OleDb.OleDbCommand ole_cmd = null;//ew System.Data.OleDb.OleDbCommand();

            try
            {
                connection += fn + ";";//@"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;";//Data Source=C:\Users\fsoliva\Downloads\Log.xls";

                ole_con = new System.Data.OleDb.OleDbConnection(connection);
                ole_cmd = new System.Data.OleDb.OleDbCommand();
                ole_cmd.CommandText = @"select * from [" + sn + @"$]";
                ole_cmd.Connection = ole_con;

                ole_con.Open();

                System.Data.DataTable zxc = get_tbl_from_cmd(ole_cmd);
            }

            finally
            {
                //if (obj_dst != null) { obj_dst.Dispose(); obj_dst = null; }
                //if (ole_adr != null) { ole_adr.Dispose(); ole_adr = null; }
                if (ole_con != null) { ole_con.Dispose(); ole_con = null; }
                if (ole_cmd != null) { ole_cmd.Dispose(); ole_cmd = null; }

            }
        }

        /// <summary>
        /// using "using"
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="sn"></param>
        ///public void ole_xls_tbl(string fn, string sn)
        public System.Data.DataTable ole_xls_tbl(string fn, string sn)
        {
            System.Data.OleDb.OleDbConnection ole_con = null;
            System.Data.OleDb.OleDbCommand ole_cmd = null;
            //System.Data.DataTable obj_tbl = null;

            string cmd_text = @"select * from [" + sn + @"$]";
            string connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=";
            connection += fn + ";";

            using (ole_con = new System.Data.OleDb.OleDbConnection(connection))
            {
                using (ole_cmd = new System.Data.OleDb.OleDbCommand(cmd_text, ole_con))
                {
                    ole_con.Open();
                    return (get_tbl_from_cmd(ole_cmd));
                    // filter table, objective is to use it as a datasource
                }
            }
        }
    
    }    //public class read_xls

}
