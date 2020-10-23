using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Components;
using MonoGame.Forms.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FrameGrabber
{
    class MonoGamePanel : MonoGameControl
    {
        private Vector2 PreviousMousePosition { get; set; }  
        private SpriteBatch spriteBatch { get; set; }
        public Texture2D SpriteSheet { get; set; }
        public Camera2D Camera { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera = new Camera2D();

            PreviousMousePosition = new Vector2(0);
        }
        public bool LoadImage(string fileName)
        {
            using(var streamSource = File.OpenRead(fileName))
            {
                try
                {
                    SpriteSheet = Texture2D.FromStream(GraphicsDevice, streamSource);
                }
                catch (InvalidOperationException)
                {

                    return false;
                }                
            }
            return true;
        }
        public void UpdateCameraPosition(System.Drawing.Point formLocation)
        {
            if (SpriteSheet == null) return;
            // Not offseting correctly, especially if form is moving around the screen
            var sheetLocation = new Rectangle(new Point(formLocation.X, formLocation.Y), new Point(SpriteSheet.Width, SpriteSheet.Height));
            if (!sheetLocation.Contains(new Point(MousePosition.X, MousePosition.Y))) return;

            float xDiff = Math.Abs(MousePosition.X) - Math.Abs(PreviousMousePosition.X);
            if (xDiff != 0)
            {
                PreviousMousePosition = new Vector2(MousePosition.X, PreviousMousePosition.Y);
                Camera.Position = new Vector2(Camera.Position.X + xDiff, Camera.Position.Y);
            }
            float yDiff = Math.Abs(MousePosition.Y) - Math.Abs(PreviousMousePosition.Y);
            if (yDiff != 0)
            {
                PreviousMousePosition = new Vector2(PreviousMousePosition.X, MousePosition.Y);
                Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y + yDiff);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Camera.GetTransformation(GraphicsDevice);

        }

        protected override void Draw()
        {
            base.Draw();

            var maxtrixTransform = Camera.Transform;
            spriteBatch.Begin(transformMatrix: maxtrixTransform);
            if (SpriteSheet != null)
            {
                spriteBatch.Draw(SpriteSheet, Vector2.Zero, Color.White);
            }
            spriteBatch.End();

        }
    }
}
