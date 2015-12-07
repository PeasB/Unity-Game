using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace Delirium_Laucher
{
    public partial class Delirium : Form
    {
        //Globals.
        float ClientVersionNum;
        string GameRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Delirium";
        int ForceUpdateCount;


        public Delirium()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ForceUpdateCount = 0; //Allow User to attempt to update normally. (After 3 Forced Updates)


            if (File.Exists(GameRoot + "/Version.txt")) //Check if the Version File exists to find out the Clients current version.
            {
                StreamReader VersionReader = new StreamReader(GameRoot + "/Version.txt");
                ClientVersionNum = float.Parse(VersionReader.ReadLine());
            }
            else if (!Directory.Exists(GameRoot)) //The Appdata Folder does not exist.
            {
                Directory.CreateDirectory(GameRoot);
                ClientVersionNum = 0;
            }
            else
            {
                ClientVersionNum = 0;
            }

            WebClient VersionWC = new WebClient();

            VersionWC.DownloadStringCompleted += VersionWC_DownloadStringCompleted;
            VersionWC.DownloadStringAsync(new Uri("http://www.peastudios.net/Delirium/CurrentVersion.txt"));
            lblActivity.Text = "Checking Version";
            lblActivity.Visible = true;
        }

        private void VersionWC_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            float ServerVersionNum = ClientVersionNum;

            if (e.Error == null)
             ServerVersionNum = float.Parse(e.Result); //Parsing Server Version.
                

            if(ServerVersionNum > ClientVersionNum) //Check if the Server Version is newer then the Client Version.
            {
                lblActivity.Text = "Downloading Updates";
                ForceUpdate();
            }
            else //No updates attempt to start game.
            {
                lblActivity.Text = "No Updates. Starting game.";

                if(File.Exists(GameRoot + "/Delirium.exe"))
                {
                    Process.Start(GameRoot + "/Delirium.exe");
                    Application.Exit();
                }
                else if(e.Error != null)
                {
                    lblActivity.Text = "No network connection. Please connect to the Internet to download game.";
                }
                //If Launcher has failed to boot game then force an update.
                else if (ForceUpdateCount != 3)
                {
                    ForceUpdateCount++;
                    lblActivity.Text = "Could not start game. No game file found. Forcing Update... Attempt " + ForceUpdateCount + " of 3";
                    ForceUpdate();
                }
                else //Could not get a boot even after 3 Forced Updates.
                {
                    lblActivity.Text = "All 3 Attempts to force Update has failed. Please reinstall the launcher, or Contact the Developer";
                }

            }

        }

        private void UpdateWC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lblActivity.Text = "Download Complete. Extracting...";

            // Checks if old file or directory exists before deleting.
            if(File.Exists(GameRoot + "/Delirium.exe"))
                File.Delete(GameRoot + "/Delirium.exe");

            if(Directory.Exists(GameRoot + "/Delirium_Data"))
                Directory.Delete(GameRoot + "/Delirium_Data",true);

            if(File.Exists(GameRoot + "/Version.txt"))
                File.Delete(GameRoot + "/Version.txt");


            //Checks if the Zip is folder and extracts game files.
            if(File.Exists(GameRoot + "Delirium.zip"))
                ZipFile.ExtractToDirectory(GameRoot + "Delirium.zip", GameRoot);

            lblActivity.Text = "Extraction Complete, Starting game.";

            //Checks if the Game exe exists if so run it.
            if (File.Exists(GameRoot + "/Delirium.exe"))
            {
                Process.Start(GameRoot + "/Delirium.exe");
                Application.Exit();
            }
            else if (e.Error != null) //If errors downloading files.
            {
                lblActivity.Text = "No network connection. Please connect to the Internet to download game.";
            }
            //If Launcher has failed to boot game then force an update.
            else if(ForceUpdateCount != 3)
            {
                ForceUpdateCount++; 
                lblActivity.Text = "Could not start game. No game file found. Forcing Update... Attempt " + ForceUpdateCount + " of 3";
                ForceUpdate();
            }
            else //Could not get a boot even after 3 Forced Updates.
            {
                lblActivity.Text = "All 3 Attempts to force Update has failed. Please reinstall the launcher, or contact the developer.";
            }
            
                
        }

        private void UpdateWC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Updates the Progress bar with a percetage out of 100.
            prgDownloadBar.Maximum = 100;
            prgDownloadBar.Value = e.ProgressPercentage;
        }


        private void ForceUpdate() //Creates a update call.
        {
            WebClient UpdateWC = new WebClient();

            UpdateWC.DownloadProgressChanged += UpdateWC_DownloadProgressChanged;
            UpdateWC.DownloadFileCompleted += UpdateWC_DownloadFileCompleted;
            UpdateWC.DownloadFileAsync(new Uri("http://www.peastudios.net/Delirium/Delirium.zip"), GameRoot + "Delirium.zip");

        }


    }


}
