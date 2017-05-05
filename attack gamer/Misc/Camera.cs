using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class Camera
    {
        //float zoom;
        float zoomBase;
        public Matrix transform;
        public Vector2 Position { get; set; }
        //float rotation;
        public float Zoom { get; set; }
        public float Rotation { get; set; }

        public Rectangle rectangle { get { return new Rectangle((int)Position.X - (Globals.ScreenWidth / 2), (int)Position.Y - (Globals.ScreenHeight / 2), Globals.ScreenWidth, Globals.ScreenHeight); } }

        public Vector2 Origin;

        public Camera()
        {
            Origin = new Vector2(Globals.ScreenWidth / 2.0f, Globals.ScreenHeight / 2.0f);
            zoomBase = 1.0f;
            Zoom = zoomBase;
            Rotation = 0.0f;
            Position = Vector2.Zero;
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }
        public Matrix get_transformation(GraphicsDevice graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0.0f));
        }
        public Matrix get_transformationTwo(GraphicsDevice graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 1f, graphics.Viewport.Height * 1f, 0.0f));
        }

        public void Reset()
        {
            Zoom = zoomBase;
            Rotation = 0.0f;
        }
    }
}
