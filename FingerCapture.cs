using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;

namespace BiometricApp
{

    public partial class FingerCapture : Form, DPFP.Capture.EventHandler
    {
        public int index = 0;

        public Bitmap export;

        //text to be displayed on the image
        //public string[] names = { "Right Thumb" };
        public string[] names = { "" };
        //public string[] names = { "Right Thumb", "Left Thumb", "Right Index", "Left Index", "Right Middle", "Left Middle", "Right Ring", "Left Ring", "Right Pinky", "Left Pinky", "Right Toe", "Left Toe" };


        public List<Bitmap> images = new List<Bitmap>();

        public bool current_image_saved = false;

        public Bitmap bmp;

        public bool fingerprocesed = false;

        string exportDir = @Directory.GetCurrentDirectory() + "\\finger_print_exports\\";
        string exportDir2 = @Directory.GetCurrentDirectory() + "\\finger_print_verify_exports\\";

        public FingerCapture()
        {
            InitializeComponent();

            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }
            else if (!Directory.Exists(exportDir2))
            {
                Directory.CreateDirectory(exportDir2);

            }
        }
        private void VerificationCapture_Load(object sender, EventArgs e)
        {
            Init();
            Start();
            save_btn.Visible = true;
        }

        protected virtual void Init()
        {
            bmp = new Bitmap(init_image.Image);
            try
            {
                Capturer = new DPFP.Capture.Capture();              // Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;                   // Subscribe for capturing events.
                else
                    SetPrompt("Can't initiate capture operation!");
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'DPFP' could not be found (are you missing a using directive or an assembly reference?)
        protected virtual void Process(DPFP.Sample Sample)
#pragma warning restore CS0246 // The type or namespace name 'DPFP' could not be found (are you missing a using directive or an assembly reference?)
        {
            // Draw fingerprint sample image.
            DrawPicture(ConvertSampleToBitmap(Sample));
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                }
                catch
                {
                    SetPrompt("Can't initiate capture!");
                }
                save_btn.Top = 360;
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    SetPrompt("Can't terminate capture!");
                }
            }
        }

        #region Form Event Handlers:
        #endregion

        #region EventHandler Members:

#pragma warning disable CS0246 // The type or namespace name 'DPFP' could not be found (are you missing a using directive or an assembly reference?)
        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
#pragma warning restore CS0246 // The type or namespace name 'DPFP' could not be found (are you missing a using directive or an assembly reference?)
        {
            Process(Sample);
            fingerprocesed = true;
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            //finger removed
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            //finger touched
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Fingerprint device connected!");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Fingerprint device  disconnected!");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.LowContrast)
            {
                //quality poor
                MessageBox.Show("Low Constrast", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.NoFinger)
            {
                //quality poor
                MessageBox.Show("No Finger", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.TooFast)
            {
                //quality poor
                MessageBox.Show("Too Fast", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.TooLow)
            {
                //quality poor
                MessageBox.Show("Too Low", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.TooNoisy)
            {
                //quality poor
                MessageBox.Show("Too Noisy", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.NoCentralRegion)
            {
                //quality poor
                MessageBox.Show("No Central Region", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.TooDark)
            {
                //quality poor
                MessageBox.Show("Too Dark", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CaptureFeedback == DPFP.Capture.CaptureFeedback.TooLight)
            {
                //quality poor
                MessageBox.Show("Too Light", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //quality good
                //if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                //{

                //}
            }
        }
        #endregion

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
            Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
            Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
            return bitmap;
        }
       protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                MessageBox.Show("FingerPrint Quality is Badd", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
        }

        protected void SetPrompt(string prompt)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //MessageBox.Show(prompt, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                stats.Text = prompt;
            });
        }

        protected void SetError(string prompt)
        {
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(prompt, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        private void DrawPicture(Bitmap bitmap)
        {
            this.Invoke((MethodInvoker)delegate
            {
                bmp = new Bitmap(bitmap, new Size(Picture.Width + 20, Picture.Height + 20));
                Picture.Image = bmp;
            });
        }

        private DPFP.Capture.Capture Capturer;

        private void save_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Home.pageState == "register")
                {
                
                    //images[index] = bmp; //^I don't know why this does not work

                    images.Add(bmp);
                    if (fingerprocesed == true)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        finish_Click(@exportDir, true);
                    }
               
                    current_image_saved = true;
                }
                else if(Home.pageState == "login")
                {
                    images.Add(bmp);
                    if (fingerprocesed == true)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        finish_Click(@exportDir2, false);
                    }

                    current_image_saved = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void finish_Click(string fpath, bool location)
        {
            Bitmap bmp = new Bitmap(preview_img.Width, preview_img.Height);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, preview_img.Width, preview_img.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
            }
            preview_img.Image = bmp;
            preview_img.Visible = true;
            drawImg();
            Stop();

            string filename = fpath + Home.userEmail + ".jpg";

            //checks if file exsits first, if true delete file and replace
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            //export the fingerprint file
            export.Save(filename, ImageFormat.Jpeg);

            if(location)
            { 
                string connectionString = "Server=Localhost;Port=3306;Database=biometric;Uid=root;Pwd=;CharSet=utf8;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                try
                {
                    string updateQuery = "UPDATE user SET fingercapture=@fingercapture WHERE email=@email";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = updateQuery;
                    cmd.Parameters.AddWithValue("@fingercapture", 1);
                    cmd.Parameters.AddWithValue("@email", Home.userEmail);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                      
                    capture_title.Text = Home.userEmail + " Finger Print Captured";


                    string selectQuery = "SELECT * FROM user WHERE email=@email";
                    MySqlCommand bcmd = new MySqlCommand(selectQuery, connection);
                    bcmd.Parameters.AddWithValue("@email", Home.userEmail);
                    MySqlDataReader rdr = bcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr["facecapture"].ToString() != "1")
                        {
                            DialogResult d;
                            d = MessageBox.Show("You have not done your Biometric Face Capture \n\nDo you want to continue to Face Capture?", " Information: ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (d == DialogResult.Yes)
                            {
                                this.Hide();
                                FaceCapture frm = new FaceCapture();
                                frm.ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                this.Hide();
                                Home frm = new Home();
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
            }
            else
            {
                this.Hide();
                FingerLogin frm = new FingerLogin();
                frm.ShowDialog();
                this.Close();
            }
        }

        public void drawImg()
        {
            int start_y = 10;
            int start_x = 10;
            int cnt = 0;
            int stepper_x = (int)(preview_img.Width - 0) / 1;
            int stepper_y = (int)(preview_img.Height - 0) / 1;

            Bitmap screen = new Bitmap(preview_img.Image, preview_img.Size);

            Graphics g = Graphics.FromImage(screen);

            RectangleF rect_text = new RectangleF(200, 15, preview_img.Width, 50);
            //g.DrawString(String.Format("", mainregno), new Font("Segoe UI Symbol", 12, FontStyle.Bold), Brushes.Black, rect_text);

            foreach (Bitmap bitmap in images)
            {
                int w = stepper_x - 10;
                int h = stepper_y - 10;

                Bitmap small = new Bitmap(bitmap, w - 12, h - 12);

                g.DrawImage(small, new Point(start_x, start_y));

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF rectf = new RectangleF(start_x + 10, (start_y + h) - 8, stepper_x, 20);

                g.DrawString(names[cnt], new Font("Segoe UI Symbol", 8, FontStyle.Bold), Brushes.Black, rectf);

                preview_img.Image = screen;

                export = screen;

                cnt++;
                start_x += stepper_x;
                if (cnt == 6)
                {
                    start_x = 45;
                    start_y += stepper_y;
                }
            }
            g.Flush();
        }

        private void Capture_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Home.pageState == "login")
            {
                this.Hide();
                Home frm = new Home();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                this.Hide();
                Register frm = new Register();
                frm.ShowDialog();
                this.Close();
            }
        }
    }
}