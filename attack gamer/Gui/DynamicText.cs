using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class DynamicText
    {
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; } = Color.White;
        public string Text { get; set; } = "Text";
        public bool TextShader { get; set; } = true;

        public bool Fade { get; set; } = true;
        public int Alpha { get; set; }
        public double Time { get; set; } = 3;

        public DynamicText() { }
        public DynamicText(SpriteFont font, Vector2 pos, Vector2 size, Vector2 direction, float speed, Color color, string text)
        {
            Font = font;
            var textSize = font.MeasureString(text);
            Position = new Vector2(pos.X + (size.X / 2) - (textSize.X / 2), pos.Y);
            Color = color;
            Text = text;
            Direction = direction;
            Speed = speed;
        }
        public void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Speed * delta * Direction;
            if (Fade)
            {
                Time -= delta;
                Alpha = (int)(Time * 255);
            }
        }
        public void Draw(SpriteBatch sb)
        {
            if (TextShader)
                sb.DrawString(Font, Text, new Vector2(Position.X + 1, Position.Y + 1), new Color(Color, Alpha));
            sb.DrawString(Font, Text, Position, new Color(Color, Alpha));
        }
    }
}
