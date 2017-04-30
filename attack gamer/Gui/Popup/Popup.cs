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
    public class Popup
    {
        public StaticText Font { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        public string Text { get; set; }
        public bool CentreText { get; set; }

        public float AliveTime { get; set; } = 5;

        public virtual void Update(GameTime gameTime)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            AliveTime -= Delta;
            if (AliveTime < 1)
            {
                Position += Speed * Delta * Direction;
                if (AliveTime <= 0) Exist = false;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, Color);
            if (Text != null)
            {
                var pos = Helper.Center(Rectangle, Font.Size);
                if (CentreText)
                    Extras.DrawString(sb, Font.Font, Font.Text, pos, Font.Color);
                else Extras.DrawString(sb, Font.Font, Font.Text, Font.Position, Font.Color);
            }
        }
    }
}
