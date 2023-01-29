using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BiometricApp
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();

            label1.Text = "Welcome to your Dasboard " + Home.userEmail;
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
