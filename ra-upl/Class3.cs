using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace WindowsFormsApplication1
{
    static class Class3
    {
        private static List<int> ReadEntityConfigurationFile(string cfgFile)
        {
            var ds = new DataSet();
            List<int> idList = new List<int>();
            List<int> idListDS = new List<int>();

            try
            {
                ds.ReadXml(cfgFile);
                if (ds.Tables.Count > 0)
                {
                    idList = (from DataRow dr in ds.Tables[0].Rows
                              select Convert.ToInt32(dr["id"])
                                ).ToList();

                    idListDS = (from DataRow dr in ds.Tables[1].Rows
                                select Convert.ToInt32(dr["id"])
                                ).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return idList;
        }


        static void createArgs()
        {

            string[] args1 = {"/operation="
                                , "/T="
                                , "/DS="
                                , "/DE="
                                , "/LD="
                                , "/S="
                                , "/DB="
                                , "/DEL="
                                , "/PORT="
                                , "/BENCH="
                                , "/CLASS="
                                , "/MAP="
                                , "/PAR="
                                , "/IS="
                                , "/SLA="
                                , "/FOFX="
                                , "/FOFI="
                                , "/FOFT="
                                , "/BS="
                                , "/CC="
                                , "/OF="
                                , "/RR="
                                , "/CP="
                                , "/SPLIT="
                                , "/RP="
                                , "/PL="
                                , "/BL="  
        };

            for (int i = 0; i < args1.Length; i++)
            {
                Console.WriteLine("args[" + (i + 1).ToString() + "] = @\"" + args1[i] + "\";");
            }

        }

    }
}
