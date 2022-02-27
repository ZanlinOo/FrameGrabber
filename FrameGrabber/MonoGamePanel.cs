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
        private Texture2D Pixel { get; set; }
        private Vector2 PreviousMousePosition { get; set; } 
        private int PreviousScrollWheelValue { get; set; }
        private SpriteBatch spriteBatch { get; set; }
        public Texture2D SpriteSheet { get; set; }
        public Camera2D Camera { get; set; }
        public bool IsInGamePanel { get; set; }
        // they selection box with grow once clicked based on the mouse position.
        public SelectionBox SelectionBox { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //resets Camera position to the top left corner. 
            Camera = new Camera2D();
            Camera.Position = new Vector2(Width / 2f, Height / 2f);

            PreviousMousePosition = new Vector2(0);
            IsInGamePanel = false;

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new[] { Color.Lerp(Color.Red, Color.Transparent, 0.65f) });

            SelectionBox = new SelectionBox(Pixel, Color.Red);
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
        
        public Rectangle getSelection()
        {
            return SelectionBox.Box;
        }
  
        private void UpdateCameraPosition()
        {
            if (SpriteSheet == null) return;

            var DeltaMousePosition = new Vector2(Math.Abs(MousePosition.X) - Math.Abs(PreviousMousePosition.X), 
                                                 Math.Abs(MousePosition.Y) - Math.Abs(PreviousMousePosition.Y));
            PreviousMousePosition = new Vector2(MousePosition.X, MousePosition.Y);
            
            if (Mouse.GetState().MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Camera.Position += new Vector2(-DeltaMousePosition.X / Camera.Zoom, -DeltaMousePosition.Y / Camera.Zoom);
            }
        }
        private void ZoomCamera(float zoomScale)
        {
            var DeltaScrollWheelValue = Mouse.GetState().ScrollWheelValue - PreviousScrollWheelValue;
            PreviousScrollWheelValue = Mouse.GetState().ScrollWheelValue;

            if (IsInGamePanel && DeltaScrollWheelValue != 0)
            {                
                // zooms based on middle mouse wheel. 
                Camera.Zoom += (DeltaScrollWheelValue * zoomScale);
                
            }

        }
        public void UpdateCamera()
        {
            UpdateCameraPosition();

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Camera.GetTransformation(GraphicsDevice);
            float zoomScale = 0.001f;
            
            SelectionBox.Update(ClientSize: ClientSize, CameraPosition: Camera.Position, zoomScale: Camera.Zoom);
            
            ZoomCamera(zoomScale);
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

            SelectionBox.Draw(spriteBatch);

            spriteBatch.End();

        }
    }
}
