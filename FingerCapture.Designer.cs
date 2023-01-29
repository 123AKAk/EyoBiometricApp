
namespace BiometricApp
{
    partial class FingerCapture
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FingerCapture));
            this.stats = new System.Windows.Forms.Label();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.capture_title = new System.Windows.Forms.Label();
            this.save_btn = new System.Windows.Forms.Button();
            this.preview_img = new System.Windows.Forms.PictureBox();
            this.init_image = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preview_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.init_image)).BeginInit();
            this.SuspendLayout();
            // 
            // stats
            // 
            this.stats.Font = new System.Drawing.Font("Segoe UI Symbol", 7.75F, System.Drawing.FontStyle.Bold);
            this.stats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.stats.Location = new System.Drawing.Point(13, 495);
            this.stats.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stats.Name = "stats";
            this.stats.Size = new System.Drawing.Size(564, 21);
            this.stats.TabIndex = 28;
            this.stats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Picture
            // 
            this.Picture.BackColor = System.Drawing.Color.White;
            this.Picture.Image = ((System.Drawing.Image)(resources.GetObject("Picture.Image")));
            this.Picture.Location = new System.Drawing.Point(157, 83);
            this.Picture.Margin = new System.Windows.Forms.Padding(4);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(276, 309);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Picture.TabIndex = 17;
            this.Picture.TabStop = false;
            // 
            // capture_title
            // 
            this.capture_title.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capture_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.capture_title.Location = new System.Drawing.Point(13, 28);
            this.capture_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.capture_title.Name = "capture_title";
            this.capture_title.Size = new System.Drawing.Size(564, 28);
            this.capture_title.TabIndex = 22;
            this.capture_title.Text = "Capture Finger ";
            this.capture_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // save_btn
            // 
            this.save_btn.BackColor = System.Drawing.Color.RoyalBlue;
            this.save_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.save_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_btn.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_btn.ForeColor = System.Drawing.Color.White;
            this.save_btn.Location = new System.Drawing.Point(239, 416);
            this.save_btn.Margin = new System.Windows.Forms.Padding(4);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(115, 44);
            this.save_btn.TabIndex = 21;
            this.save_btn.Text = "✓ Next";
            this.save_btn.UseVisualStyleBackColor = false;
            this.save_btn.Visible = false;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // preview_img
            // 
            this.preview_img.BackColor = System.Drawing.Color.White;
            this.preview_img.Location = new System.Drawing.Point(157, 83);
            this.preview_img.Margin = new System.Windows.Forms.Padding(4);
            this.preview_img.Name = "preview_img";
            this.preview_img.Size = new System.Drawing.Size(276, 309);
            this.preview_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.preview_img.TabIndex = 14;
            this.preview_img.TabStop = false;
            this.preview_img.Visible = false;
            // 
            // init_image
            // 
            this.init_image.BackColor = System.Drawing.Color.White;
            this.init_image.Image = ((System.Drawing.Image)(resources.GetObject("init_image.Image")));
            this.init_image.Location = new System.Drawing.Point(11, 549);
            this.init_image.Margin = new System.Windows.Forms.Padding(4);
            this.init_image.Name = "init_image";
            this.init_image.Size = new System.Drawing.Size(39, 38);
            this.init_image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.init_image.TabIndex = 38;
            this.init_image.TabStop = false;
            // 
            // FingerCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 542);
            this.Controls.Add(this.init_image);
            this.Controls.Add(this.stats);
            this.Controls.Add(this.preview_img);
            this.Controls.Add(this.capture_title);
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.Picture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FingerCapture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finger Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Capture_FormClosing);
            this.Load += new System.EventHandler(this.VerificationCapture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preview_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.init_image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label stats;
        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Label capture_title;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.PictureBox preview_img;
        private System.Windows.Forms.PictureBox init_image;
    }
}