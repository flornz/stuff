using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace ClassLibrary1
    {

public class stressTest
{
    public static void do_test()
    {
        int numTs = 100;
        ParameterizedThreadStart job = new ParameterizedThreadStart(ThreadJob);
        var threads = new Thread[numTs];


        //System.Threading.Tasks.Parallel.For(0, 50, x =>
        //{
        //    //randomize webaddress
        //    object[] parameters = new object[] { webaddress };

        //    threads[x] = new Thread(job);
        //    threads[x].Start(parameters);
        //}
        //           );
        
        
        for (int x = 0; x < numTs; x++)
        {
            //randomize webaddress
            string webaddress = @"http://www.cnn.com/";
            object[] parameters = new object[] { webaddress };

            threads[x] = new Thread(job);
            threads[x].Start(parameters);
        }
    }

    public static void ThreadJob(object parameters)
    {
        object[] parameterArray = (object[])parameters;
        string webPageAddy = (string)parameterArray[0];

        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(webPageAddy);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        }
        catch
        {
            //MessageBox.Show("fail");
            throw;
        }

    }
}
    }



