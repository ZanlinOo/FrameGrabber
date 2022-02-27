using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FrameGrabber
{
    public class MyMouseHelperData
    {
        public bool IsClicked { get; set; } = false;
        public bool HasMoved { get; set; } = false;
        public Vector2 Position { get; set; } = Vector2.Zero;
    }
    public class SelectionBox
    {
        private Texture2D texture;
        private Color tint;

        // when a click is detected.
        // place the first point of the selection box.
        // when a second click is detected, place the next point
        public Vector2[] Points { get; set; } = new Vector2[2] { Vector2.Zero, Vector2.Zero };
        public Rectangle Box => UpdatedBox();
        public MyMouseHelperData MouseHelper { get; set; } = new MyMouseHelperData();

        private MouseStates MouseState = MouseStates.None;
        Rectangle UpdatedBox()
        {
            int x = (int)Points[0].X;
            int y = (int)Points[0].Y;
            
            int width = (int)(Points[1].X - x);
            int height = (int)( Points[1].Y - y);
            
            //expand to the bottom left
            if (width < 0 && height > 0)
            {
                x += width;
                width = Math.Abs(width);
            }
            //expand to the top right
            else if (height < 0 && width > 0)
            {
                y += height;
                height = Math.Abs(height);
            }

            return new Rectangle(x, y, width, height);            
        }

        public SelectionBox(Texture2D texture, Color tint)
        {
            this.texture = texture;
            this.tint = tint;  
        }

        public void Update(System.Drawing.Size ClientSize, Vector2 CameraPosition, float zoomScale)
        {
            if (MouseHelper.HasMoved)
            {
                var cameraX_Offest = (ClientSize.Width / 2f) - (CameraPosition.X);
                var cameraY_Offest = (ClientSize.Height / 2f) - (CameraPosition.Y);
                MouseHelper.Position += new Vector2(- cameraX_Offest, - cameraY_Offest);
                
                MouseHelper.HasMoved = false;
            }
            // need to add option ****
            // when ESC button is pressed. reset points back to zero.
            switch (MouseState)
            {
                //First click state
                //Box size now follows the position of the mouse.
                case MouseStates.AfterFirstClick:
                    Points[0] = MouseHelper.Position;
                    Points[1] = MouseHelper.Position;
                    MouseState = MouseStates.WaitForSecondClick;
                    break;
                //Second click state
                //Box size is now set and no longer follows the mouse.
                case MouseStates.WaitForSecondClick:
                    Points[1] = MouseHelper.Position;
                    if(MouseHelper.IsClicked)
                    {
                        MouseState = MouseStates.None;
                        MouseHelper.IsClicked = false;
                    }
                    break;
                default:
                    // when not clicked
                    if(MouseHelper.IsClicked)
                    {
                        MouseState = MouseStates.AfterFirstClick;
                        MouseHelper.IsClicked = false;
                    }
                    break;
            }
        }

        //box must be drawn.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Box, tint);
        }
    }
}
