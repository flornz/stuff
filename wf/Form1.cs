using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Web.Script.Serialization;
namespace WindowsFormsApplication1
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
                    if (letT == "")
                    {
                            combo += String.Format("  {0}", currW);
                    }
                    else {
                        if (currW.Contains(letT) && currW.Length > letT.Length)
                        {
                            combo += String.Format("  {0}", currW);
                        }
                    }

                    //Console.WriteLine("{0}: {1}", row[0].ToString().ToUpper(), row[2].ToString());
                }
                if (combo.Length > 1)
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
                    if (string.IsNullOrEmpty(oL))
                    {
                        combo += String.Format("  {0}: {1}", row[0].ToString().ToUpper(), currW);

                    }
                    else
                    {
                        if (oL.Contains(aL))
                        {
                            combo += String.Format("  {0}: {1}", row[0].ToString().ToUpper(), currW);
                        }
                    }
                }
                Console.WriteLine(combo);
            }

        }


        void dispList(List<string> ls1)
        {
            for (int ctr = 0; ctr < ls1.Count; ctr++)
            {
                Console.WriteLine("{0}", ls1[ctr].ToUpper());
            }
        }


        DataSet getWFWords(string ltrs, string letT)
        {
            DataSet ds0 = null;

            string html = HtmlTableParser.getHtmlWF(ltrs + letT);
            string htmlRes = HtmlTableParser.getHtmlWFResults(html);
            ds0 = HtmlTableParser.ParseDataSet(htmlRes);

            return ds0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataSet ds2 = null;
            string ltrs = "uycefqa";
            /*             *              */
            string[] toFind = new string[] { "c*", "*g", new String('-', 8), new String('-', 7) };

            // debug
            try
            {
                //string htmlP1 = HtmlTableParser.getHtmlMWP1Res(HtmlTableParser.mwHTML);
                DataSet ds1 = HtmlTableParser.ParseDataSetMW(HtmlTableParser.mwHTML);
                List<string> res1 = HtmlTableParser.getWsForCriteria(ds1, toFind);
            }
            catch (Exception)
            {
                
                throw;
            }


            try
            {
                string[] letTog = new string[] { "be"
                    , "di" 
                    , "ya" 
                };

                if (letTog != null)
                {
                    for (int i = 0; i < letTog.Length; i++)
                    {
                        ds2 = getWFWords(ltrs, letTog[i]);
                        dispDTs(ds2, letTog[i]);
                   }
                }
                else {
                    ds2 = getWFWords(ltrs, "");
                    dispDTs(ds2, "");
                }


                string html1 = HtmlTableParser.getHtmlMWP1(ltrs);
                string htmlP1 = HtmlTableParser.getHtmlMWP1Res(html1);
                DataSet ds1 = HtmlTableParser.ParseDataSetMW(htmlP1);
                dispDTsMW(ds1, "couvapg");
                //    List<string> res1 = HtmlTableParser.getWsForCriteria(ds1, toFind);
                //    dispList(res1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
            //Application.Exit();

        }
    }
}