using System;
using System.Threading;
using System.Windows.Forms;

namespace ClassLibrary1
    {
    public class cboard
        {

        private Thread thread = null;// = new Thread(new ThreadStart(do_thread_ftn));

        private string _bored_txt;

        public string BoredText
            {
            get
                {
                return (_bored_txt);
                }
            set
                {
                _bored_txt = value;
                }
            }

        //[STAThread]
        public void clip_text()
            {
            thread = new Thread(new ThreadStart(do_thread_ftn));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            }

        private void do_thread_ftn()
            {
            try
                {
                //Clipboard.SetDataObject("richTextBox1.SelectedText");
                Clipboard.SetText(BoredText);

                // Retrieves data
                //IDataObject iData = Clipboard.GetDataObject();
                // Is Data Text?
                //if (iData.GetDataPresent(DataFormats.Text))
                //    label1.Text = (String)iData.GetData(DataFormats.Text);
                //else
                //    label1.Text = "Data not found.";
                }
            finally
                {
                thread.Abort();
                thread = null;
                }
            }
        }
    }
