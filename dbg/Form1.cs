using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace stuff
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable dt1 = null;
        DataTable dt2 = null;
        string colPK = "IsinCode";

        void createDTs()
        {
            dt1 = new DataTable("dtLeft");
            dt1.Columns.Add(colPK, typeof(String));
            dt1.Columns.Add("AliasCode", typeof(String));

            dt1.Rows.Add("1", "Chai");
            dt1.Rows.Add("2", "Queso Cabrales");
            dt1.Rows.Add("3", "Tofu");
            dt1.Rows.Add("4", "Mirapua");
            dt1.Rows.Add("5", "Ceviche");

            dt2 = new DataTable("dtRight");
            dt2.Columns.Add(colPK, typeof(String));
            dt2.Columns.Add("AliasCode", typeof(String));

            dt2.Rows.Add("1", "Chai");
            dt2.Rows.Add("2", "Queso Cabrales4");
            dt2.Rows.Add("3", "Tofu");
        }


        IEnumerable<object[]> getLJRowsSinglePK(DataTable dtL, DataTable dtR, string id)
        {
            var rowsFromLJ = from rowLeft in dtL.AsEnumerable()
                             join rowRight in dtR.AsEnumerable() on rowLeft[id] equals rowRight[id] into gj
                             from subRight in gj.DefaultIfEmpty()
                             select rowLeft.ItemArray.Concat((subRight == null) ? (dtL.NewRow().ItemArray) : subRight.ItemArray).ToArray();

            return rowsFromLJ;
        }


        IEnumerable<object[]> getIJRowsSinglePK(DataTable dtL, DataTable dtR, string id)
        {
            var rowsFromRJ = from rowLeft in dtL.AsEnumerable()
                             join rowRight in dtR.AsEnumerable() on rowLeft[id] equals rowRight[id]
                             select rowLeft.ItemArray.Concat(rowRight.ItemArray).ToArray();
            return rowsFromRJ;
        }

        IEnumerable<object[]> getIJRowsPKs(DataTable dtL, DataTable dtR)
        {
            var items = from rowL in dtL.AsEnumerable()
                        join rowR in dtR.AsEnumerable()
                        on new
                        {
                            Name = rowL.Field<String>("IsinCode"),
                            LastName = rowL.Field<String>("AliasCode"),
                        }  equals new
                        {
                            Name = rowR.Field<String>("IsinCode"),
                            LastName = rowR.Field<String>("AliasCode"),
                        }
                        select rowL.ItemArray.Concat(rowR.ItemArray).ToArray();

            return items;
        }

        //var qry = from t1 in table1
        //  join t2 in table2
        //  on new {t1.ID,t1.Country} equals new {t2.ID,t2.Country}
        //  ...

        //DataTable getLJRowsSinglePK_1(DataTable dtL, DataTable dtR, string id)
        //{
        //    //table1        table2
        //    //ID, name       ID, stock
        //    //1, item1       1, 100
        //    //2, item2       3, 50
        //    //3, item3

        //    DataTable dtResult = new DataTable();
        //    dtResult.Columns.Add("ID", typeof(string));
        //    dtResult.Columns.Add("name", typeof(string));
        //    dtResult.Columns.Add("stock", typeof(int));

        //    var rowsFromLJ = from dataRows1 in dtL.AsEnumerable()
        //                     join dataRows2 in dtR.AsEnumerable()
        //                     on dataRows1.Field<string>("ID") equals dataRows2.Field<string>("ID") into lj
        //                     from r in lj.DefaultIfEmpty()
        //                     select dtResult.LoadDataRow(new object[]
        //                         {
        //                            dataRows1.Field<string>("ID"),
        //                            dataRows1.Field<string>("name"),
        //                            r == null ? 0 : dataRows2.Field<int>("stock")
        //                          }, false);

        //    rowsFromLJ.CopyToDataTable();

        //    return rowsFromLJ;
        //}


        internal static DataTable getIJRowsPK(DataTable dtL, DataTable dtR)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("IsinCode", typeof(String));
            dt1.Columns.Add("AliasCode", typeof(String));

            var items = from rowL in dtL.AsEnumerable()
                        join rowR in dtR.AsEnumerable()
                        on rowL.Field<String>("IsinCode") equals rowR.Field<String>("IsinCode")
                        select rowL;

            foreach (DataRow item in items)
            {
                dt1.ImportRow(item);
            }

            return dt1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<object[]> rows = new List<object[]>();
            createDTs();

            getIJRowsPK(dt2, dt1);

            foreach (var item in rows)
            {
                Console.WriteLine(item[1].ToString() + ": " + item[3].ToString());
            }
        }
    }
}
