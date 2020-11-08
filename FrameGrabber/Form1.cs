using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameGrabber
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                bool isLoaded = monoGamePanel1.LoadImage(fileName);
                if (!isLoaded)
                {
                    MessageBox.Show($"Invalid File: {fileName}");
                }
            }
        }        
        private void monoGamePanel1_MouseMove(object sender, MouseEventArgs e)
        {
            monoGamePanel1.IsInGamePanel = true;
            monoGamePanel1.UpdateCamera();
                        
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            
            Text = $"X:{Location.X} Y:{Location.Y} monoX:{monoGamePanel1.Location.X} monoY:{monoGamePanel1.Location.Y}";
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            monoGamePanel1.IsInGamePanel = false;
        }
    }
}
