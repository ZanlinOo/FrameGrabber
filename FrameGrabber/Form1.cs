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
            Text = $"X:{e.Location.X} Y:{e.Location.Y}";

            monoGamePanel1.MousePositionOnPanel = new Microsoft.Xna.Framework.Vector2(e.X, e.Y);
        }

        private void Form1_Move(object sender, EventArgs e)
        {            
            Text = $"X:{Location.X} Y:{Location.Y} monoX:{monoGamePanel1.Location.X} monoY:{monoGamePanel1.Location.Y}";
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            monoGamePanel1.IsInGamePanel = false;
        }
        // when clicked. draw a Red square on the monogame panel.
        private void monoGamePanel1_MouseClick(object sender, MouseEventArgs e)
        {
            monoGamePanel1.IsMouseDown = true;
            monoGamePanel1.ClickLocation = new Microsoft.Xna.Framework.Vector2(e.Location.X, e.Location.Y);
        }
    }
}
