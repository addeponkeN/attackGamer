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
        float zoom;
        float zoomBase;
        public Matrix transform;
        public Vector2 pos;
        float rotation;

        public Rectangle rectangle { get { return new Rectangle((int)pos.X - (Globals.ScreenX / 2), (int)pos.Y - (Globals.ScreenY / 2), Globals.ScreenX, Globals.ScreenY); } }

        public Vector2 Origin;

        public Camera()
        {
            Origin = new Vector2(Globals.ScreenX / 2.0f, Globals.ScreenY / 2.0f);
            zoomBase = 1.0f;
            zoom = zoomBase;
            rotation = 0.0f;
            pos = Vector2.Zero;
        }
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.02f) zoom = 0.02f; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public void Move(Vector2 amount)
        {
            pos += amount;
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Matrix get_transformation(GraphicsDevice graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0.0f));
        }
        public Matrix get_transformationTwo(GraphicsDevice graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 1f, graphics.Viewport.Height * 1f, 0.0f));
        }

        public void Reset()
        {
            zoom = zoomBase;
            rotation = 0.0f;
        }
    }
}
