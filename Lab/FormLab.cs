using SPAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPAccess.ViewModel;

namespace Lab
{
    public partial class FormLab : Form
    {
        SPAccess.SPAccess SPAccess;
        private Account acc;

        public FormLab()
        {
            InitializeComponent();
        }

        private void FormLab_Load(object sender, EventArgs e)
        {
            SPAccess = new SPAccess.SPAccess();

            acc = SPAccess.AccountManager.GetAccount(2);
        }

        private void ButtonTest_Click(object sender, EventArgs e)
        {
            //acc = SPAccess.AccountManager.GetAccount(2);
            Log.ResetText();
            Log.AppendText("StartTime = " + SPAccess.ConfigurationManager.StartTime + "\n");
            Log.AppendText("" + SPAccess.SessionManager.test() + "\n");
            Log.AppendText("Account.Username = " + acc.GetHashCode() + "\n");
            Log.AppendText("Account.Username = " + acc.Username + "\n");
        }

        private void ButtonUpdateData_Click(object sender, EventArgs e)
        {
            SPAccess.AccountManager.GetAccount(2);
        }
    }
}
