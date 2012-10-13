using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseTestApp
{
    public partial class ErrorForm : Form
    {
        public static void ShowErrorForm(Exception e)
        {
            using (ErrorForm ef = new ErrorForm())
            {
                ef.Exception = e;
                ef.ShowDialog();
            }
        }
        private Exception exc;

        public ErrorForm()
        {
            InitializeComponent();
        }

        public Exception Exception
        {
            get
            {
                return exc;
            }
            set
            {
                exc = value;
                if (lblText.Text == string.Empty)
                {
                    Exception e = exc;
                    while (e != null)
                    {
                        lblText.Text += exc.Message + "\r\n";
                        e = e.InnerException;
                    }
                }
                txtDetail.Text = exc.ToString();
            }
        }
        public string ExceptionText
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }
    }
}
