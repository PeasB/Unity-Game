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
            this.lblActivity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prgDownloadBar
            // 
            this.prgDownloadBar.Location = new System.Drawing.Point(45, 260);
            this.prgDownloadBar.Name = "prgDownloadBar";
            this.prgDownloadBar.Size = new System.Drawing.Size(386, 34);
            this.prgDownloadBar.TabIndex = 0;
            // 
            // lblActivity
            // 
            this.lblActivity.Location = new System.Drawing.Point(45, 307);
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
            this.ClientSize = new System.Drawing.Size(487, 339);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.prgDownloadBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delirium Launcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar prgDownloadBar;
        private System.Windows.Forms.Label lblActivity;
    }
}

