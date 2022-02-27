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
using Newtonsoft.Json;

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
        //export a JSON file or type rectangle.
        //first serialize rectangle.
        //Next is to write file.

        //************************************************************************* Need to Export an array of Rectangles
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // need to change the rectanlge to one from the selection.
                string valuesToExport = JsonConvert.SerializeObject(monoGamePanel1.getSelection());

                File.WriteAllText(saveFileDialog.FileName, valuesToExport);
            }
        }
        private void monoGamePanel1_MouseMove(object sender, MouseEventArgs e)
        {
            monoGamePanel1.SelectionBox.MouseHelper.HasMoved = true;

            monoGamePanel1.IsInGamePanel = true;
            monoGamePanel1.UpdateCamera();

            Text = $"X:{e.Location.X} Y:{e.Location.Y} " +
                   $"CameraX:{monoGamePanel1.Camera.Position.X} " +
                   $"CameraY:{monoGamePanel1.Camera.Position.Y} " + 
                   $"Zoom: {monoGamePanel1.Camera.Zoom} " +
                   $"Transfrom: {monoGamePanel1.Camera.Transform}";
            monoGamePanel1.SelectionBox.MouseHelper.Position = new Microsoft.Xna.Framework.Vector2(e.X, e.Y);
        }

        private void Form1_Move(object sender, EventArgs e)
        {            
           // Text = $"X:{Location.X} Y:{Location.Y} monoX:{monoGamePanel1.Location.X} monoY:{monoGamePanel1.Location.Y}";
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            monoGamePanel1.IsInGamePanel = false;
        }
        // when clicked. draw a Red square on the monogame panel.
        int counter = 0;
        private void monoGamePanel1_MouseClick(object sender, MouseEventArgs e)
        {
            Text += $" {counter}";
            if(e.Button == MouseButtons.Left) 
                monoGamePanel1.SelectionBox.MouseHelper.IsClicked = true;
        }

        private void monoGamePanel1_MouseHover(object sender, EventArgs e)
        {

        }
    }
}
