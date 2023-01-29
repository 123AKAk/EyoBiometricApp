using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PatternRecognition.FingerprintRecognition.Core;
using PatternRecognition.FingerprintRecognition.FeatureExtractors;
using PatternRecognition.FingerprintRecognition.Matchers;

namespace BiometricApp
{
    public partial class FingerLogin : Form
    {
        //match variables
        public double score;
        public string matchtemp, matchtemp2;

        public FingerLogin()
        {
            InitializeComponent();

            nextbtn.Visible = false;

            matchtemp = @Directory.GetCurrentDirectory() + "\\finger_print_verify_exports\\" + Home.userEmail + ".jpg";
            matchtemp2 = @Directory.GetCurrentDirectory() + "\\finger_print_exports\\" + Home.userEmail + ".jpg";

            match(matchtemp, matchtemp2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FingerCapture frm = new FingerCapture();
            frm.ShowDialog();
            this.Close();
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
            FaceCapture frm = new FaceCapture();
            frm.ShowDialog();
            this.Close();
        }

        //run fingerprint match here
        private Bitmap Change_Resolution(string file)
        {
            using (Bitmap bitmap = (Bitmap)Image.FromFile(file))
            {
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    newBitmap.SetResolution(500, 500);
                    return newBitmap;
                }
            }
        }

        private void page_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            FingerCapture frm = new FingerCapture();
            frm.ShowDialog();
            this.Close();
        }

        private void match(string query, string template)
        {
            try
            {
                Change_Resolution(query);
                Change_Resolution(template);


                // Loading fingerprints
                var fingerprintImg1 = ImageLoader.LoadImage(query);
                var fingerprintImg2 = ImageLoader.LoadImage(template);
                //// Building feature extractor and extracting features
                var featExtractor = new MTripletsExtractor() { MtiaExtractor = new Ratha1995MinutiaeExtractor() };
                var features1 = featExtractor.ExtractFeatures(fingerprintImg1);
                var features2 = featExtractor.ExtractFeatures(fingerprintImg2);

                // Building matcher and matching
                var matcher = new M3gl();
                double similarity = matcher.Match(features1, features2);
                //score = similarity.ToString("0.000");
                score = similarity;
                matchtxt.Text = score.ToString();
                if (score > 1)
                {
                    matchtxt.ForeColor = Color.Green;
                    matchstatus.Text = "Passed";
                    matchstatus.ForeColor = Color.Green;

                    nextbtn.Visible = true;
                }
                else if(score > 0)
                {
                    matchtxt.ForeColor = Color.Blue;
                    matchstatus.Text = "Passed";
                    matchstatus.ForeColor = Color.Blue;

                    nextbtn.Visible = true;

                }
                else
                {
                    matchtxt.ForeColor = Color.Red;
                    matchstatus.Text = "Failed";
                    matchstatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
