using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ProgressForm : Form
    {
        #region PROPERTIES

        public string Message
        {
            set { labelMessage.Text = value; }
        }

        public int ProgressValue
        {
            set
            {
                int maxProgCtrIncrease = 8;
                int resetVal = this.progressBar1.Maximum - maxProgCtrIncrease;

                //if (value >= resetVal)
                //{
                //    // progress bar max value reached, restart
                //    value = 0;
                //}
                
                progressBar1.Value = value % 100; 
            }
        }

        #endregion
        public ProgressForm()
        {
            InitializeComponent();
        }

        #region EVENTS

        public event EventHandler<EventArgs> Canceled;


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Create a copy of the event to work with
            EventHandler<EventArgs> ea = Canceled;
            /* If there are no subscribers, eh will be null so we need to check
             * to avoid a NullReferenceException. */
            if (ea != null)
                ea(this, e);
        }

        #endregion
    }
}
