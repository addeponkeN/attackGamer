using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Spritesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class EquippableItem
    {
        public GridSheet GSheet { get; set; }
        public Texture2D Texture => GSheet.Texture;
        public Vector2 Position { get; set; }
        public Point Point => new Point((int)Position.X / 32, (int)Position.Y / 32);
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public Vector2 CenterBox => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));

        public float Speed { get; set; } = 50f;
        public Vector2 Direction { get; set; } = Vector2.Zero;
        public float VelocityForce { get; set; } = 1f;
        public float Delta { get; set; }

        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;

        public Rectangle SetSource(int column, int row)
        {
            return GSheet[column, row];
        }


        public virtual void Draw(SpriteBatch sb, GameTime gt)
        {

            sb.Draw(Texture, Rectangle, SetSource(Column, Row), Color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
