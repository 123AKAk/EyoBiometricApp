using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace BiometricApp
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        

        string connectionString = "Server=Localhost;Port=3306;Database=biometric;Uid=root;Pwd=;CharSet=utf8;";
        private void save_btn_Click(object sender, EventArgs e)
        {
            Home.pageState = "register";

            bool finishState = false;
            bool checkState = false;

            if (name.Text != "" && email.Text != "" && password.Text != "")
            {
                MySqlConnection connection = new MySqlConnection(connectionString);

                try
                {
                    connection.Open();

                    string sql = "SELECT * FROM user WHERE email=@email";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@email", email.Text);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        checkState = true;
                        MessageBox.Show("User with Email: " + rdr["email"] + " already exsits");
                        Home.userEmail = rdr["email"].ToString();

                        if (rdr["biometrics"].ToString() != "1")
                        {
                            DialogResult d;
                            d = MessageBox.Show("You have not done your Biometric Finger Print Capture \n\nDo you want to continue to Biometrics Capture?", " Information: ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (d == DialogResult.Yes)
                            {
                                this.Hide();
                                FingerCapture frm = new FingerCapture();
                                frm.ShowDialog();
                                this.Close();
                            }
                        }
                    }
                    rdr.Close();
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

                if(!checkState)
                {
                    try
                    {
                        MySqlCommand cmd;
                        connection.Open();

                        cmd = connection.CreateCommand();
                        cmd.CommandText = "INSERT INTO user (email, username, password) VALUES(@email, @username, @password)";
                        cmd.Parameters.AddWithValue("@email", email.Text);
                        cmd.Parameters.AddWithValue("@username", name.Text);
                        cmd.Parameters.AddWithValue("@password", password.Text);
                        cmd.ExecuteNonQuery();

                        Home.userEmail = email.Text;

                        MessageBox.Show("Registration Successfull, \n\nClick Ok to Capture Biometrics", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        finishState = true;
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

                            if (finishState == true)
                            {
                                this.Hide();
                                FingerCapture frm = new FingerCapture();
                                frm.ShowDialog();
                                this.Close();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill all Feilds", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }
    }
}
