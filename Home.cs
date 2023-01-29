using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace BiometricApp
{
    public partial class Home : Form
    {
        public static string userEmail = null;
        public static string pageState = null;

        public Home()
        {
            InitializeComponent();
        }

     
        private void home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(email.Text == "" && password.Text == "")
            {
                MessageBox.Show("Fill all feilds, enter your Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string connectionString = "Server=Localhost;Port=3306;Database=biometric;Uid=root;Pwd=;CharSet=utf8;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                try
                {
                    string selectQuery = "SELECT * FROM user WHERE email=@email";
                    MySqlCommand bcmd = new MySqlCommand(selectQuery, connection);
                    bcmd.Parameters.AddWithValue("@email", email.Text);
                    MySqlDataReader rdr = bcmd.ExecuteReader();

                    bool checkee = false;

                    while (rdr.Read())
                    {
                        checkee = true;

                        if (rdr["password"].ToString() == password.Text && rdr["email"].ToString() == email.Text)
                        {
                            if(rdr["biometrics"].ToString() != "1")
                            {
                                DialogResult d;
                                d = MessageBox.Show("You have not completed your Biometrics \n\nDo you want to continue to Register?", " Information: ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (d == DialogResult.Yes)
                                {
                                    this.Hide();
                                    Register frm = new Register();
                                    frm.ShowDialog();
                                    this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Login details correct!");
                            
                                userEmail = email.Text;
                                pageState = "login";

                                this.Hide();
                                FingerCapture frm = new FingerCapture();
                                frm.ShowDialog();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    rdr.Close();

                    if(!checkee)
                    {
                        MessageBox.Show("Invalid Email: \n\nEmail not found in Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Server connection error. \n\nMore information:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //register
            this.Hide();
            Register frm = new Register();
            frm.ShowDialog();
            this.Close();
        }

    }
}
