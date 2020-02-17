using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server {
    public partial class Form1 : Form {
        private Server server;

        public Form1() {
            InitializeComponent();

            server = new Server();
            server.Status += ServerStatusChanged;
        }

        private void Button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            server.Run();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e) {

        }

        private void ServerStatusChanged(object sender, EventArgs args) {
            infoBox.Invoke((MethodInvoker)delegate {
                infoBox.AppendText((args as StatusEventArgs).Status + "\n");
            });
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            server.Stop();
        }
    }
}
