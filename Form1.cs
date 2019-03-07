using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSOnlineVersionChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public BackgroundWorker WithEvents = new BackgroundWorker();

        public void BackgroundApplicationUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            OnlineVersion.CheckUpdate();
        }

        void CheckUpdate()
        {
            Thread checkUpdateThread = new Thread(OnlineVersion.CheckUpdate);
            checkUpdateThread.SetApartmentState(ApartmentState.STA);
            checkUpdateThread.Start();
        }

        //For Debugging Purposes
        private void Form1_Load(object sender, EventArgs e)
        {
            localVersion.Text = OnlineVersion.GetLocalVersion();
            onlineVersion.Text = OnlineVersion.GetOnlineVersion();
            CheckUpdate();
        }

    }
}
