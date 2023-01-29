using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Reflection.Emit;

namespace BiometricApp
{
    public partial class FaceCapture : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;


        public FaceCapture()
        {
            InitializeComponent();

            nextbtn.Visible = false;

            textBox1.Text = Home.userEmail;

            if(Home.pageState == "login")
            {
                groupBox1.Visible = false;
            }

            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

                //Initialize the capture device
                grabber = new Capture();
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                Application.Idle += new EventHandler(FrameGrabber);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                //MessageBox.Show("Nothing in binary database, Please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                label6.Text = "Face Capture :- Nothing in Binary Database, simply add a face";

                //Initialize the capture device
                grabber = new Capture();
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                Application.Idle += new EventHandler(FrameGrabber);
            }

        }


        private void nextbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard frm = new Dashboard();
            frm.ShowDialog();
            this.Close();
        }

        int checkbtn = 0;
        private void button2_Click(object sender, System.EventArgs e)
        {
            checkbtn++;

            string connectionString = "Server=Localhost;Port=3306;Database=biometric;Uid=root;Pwd=;CharSet=utf8;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                string updateQuery = "UPDATE user SET facecapture=@facecapture,biometrics=@biometrics WHERE email=@email";
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = updateQuery;
                cmd.Parameters.AddWithValue("@facecapture", 1);
                cmd.Parameters.AddWithValue("@biometrics", 1);
                cmd.Parameters.AddWithValue("@email", Home.userEmail);
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                

                DialogResult d;
                d = MessageBox.Show(Home.userEmail + " face detected and added \n\nDo you want to continue to Login?", " Capture OK: ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == DialogResult.Yes)
                {
                    this.Hide();
                    Home frm = new Home();
                    frm.ShowDialog();
                    this.Close();
                }

                if(checkbtn >= 2)
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enable the face detection first", "Capture Failed" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);


                    name = recognizer.Recognize(result);

                    //MessageBox.Show(name);

                    if (Home.pageState == "login")
                    {
                        if (name == Home.userEmail)
                        {
                            currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                            nextbtn.Enabled = true;
                            nextbtn.Visible = true;
                        }
                    }

                    //Draw the label for each face detected and recognized

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        private void FaceCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }
    }
}