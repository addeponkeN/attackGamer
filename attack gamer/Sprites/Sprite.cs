using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 SpeedV { get; set; }
        public float SpeedF { get; set; }
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(32 * 2f);
        public Vector2 SetSize(Vector2 size) { if (Size.X <= 0 && Size.Y <= 0) return new Vector2(Texture.Width, Texture.Height); return Size; }
        public Color Color { get; set; } = Color.White;
        public Color BaseColor { get; set; } = Color.White;
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }
        public Vector2 CenterBox { get { return new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2)); } }

        public bool exist;

        public int row;
        public int column;
        public int SourceSize = 32;
        public Rectangle SourceRectangle { get { return new Rectangle(column * SourceSize, row * SourceSize, SourceSize, SourceSize); } }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }
        public Sprite(Texture2D texture, Color color)
        {
            Texture = texture;
            BaseColor = color;
            Color = BaseColor;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
        public virtual void DrawSheet(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, SourceRectangle, Color);
        }
    }
}
