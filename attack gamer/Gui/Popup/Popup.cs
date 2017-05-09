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
        public StaticText Text { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle => new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y);

        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(128, 48);
        public Color BaseColor { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; } = (int)150;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        //public string Msg { get; set; }
        public bool CentreText { get; set; }
        public bool Newest { get; set; }

        public float AliveTime { get; set; } = 5;

        public T GetPopup<T>() where T : Popup => this as T;
        public Popup()
        {
            Text = new StaticText();
            Text.Font = ScreenManager.DebugFont;
        }
        public virtual void Update(GameTime gameTime)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            AliveTime -= Delta;
            if (AliveTime < 1)
            {
                Alpha = 150 * AliveTime;
                Speed += 250f * Delta;
                Position += Speed * Delta * Direction;
                if (AliveTime <= 0) Exist = false;
            }
            if (Newest)
                Color = new Color(0, 0, 0, (int)Alpha);
            else Color = new Color(0, 0, 0, (int)Alpha);
        }
        public void SetMsg(string msg)
        {
            Text.Msg = msg;
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, Color);
            if (Text.Msg != null)
            {
                var pos = Helper.Center(Rectangle, Text.Size);
                if (CentreText)
                {
                    //if (Quantity > 0)
                    //Extras.DrawString(sb, Text.Font, Text.Msg + " x" + Quantity, pos, new Color(Text.Color, Alpha));
                    //else
                    Extras.DrawString(sb, Text.Font, Text.Msg, pos, new Color(Text.Color, Alpha));
                }
                //if (Quantity > 0)
                //Extras.DrawString(sb, Text.Font, Text.Msg + " x" + Quantity, Text.Position, new Color(Text.Color, Alpha));
                else Extras.DrawString(sb, Text.Font, Text.Msg, Text.Position, new Color(Text.Color, Alpha));
            }
        }
    }
}
