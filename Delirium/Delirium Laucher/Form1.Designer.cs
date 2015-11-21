namespace Delirium_Laucher
{
    partial class Form1
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
            this.prgDownloadBar = new System.Windows.Forms.ProgressBar();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lblActivity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prgDownloadBar
            // 
            this.prgDownloadBar.Location = new System.Drawing.Point(30, 294);
            this.prgDownloadBar.Name = "prgDownloadBar";
            this.prgDownloadBar.Size = new System.Drawing.Size(386, 34);
            this.prgDownloadBar.TabIndex = 0;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(437, 274);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(161, 70);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblActivity
            // 
            this.lblActivity.Location = new System.Drawing.Point(30, 331);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(386, 13);
            this.lblActivity.TabIndex = 2;
            this.lblActivity.Text = "Activity";
            this.lblActivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblActivity.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 356);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.prgDownloadBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Delirium Launcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar prgDownloadBar;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label lblActivity;
    }
}

