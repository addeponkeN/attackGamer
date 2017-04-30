using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class StaticText
    {
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size => Font.MeasureString(Text);
        public Color Color { get; set; } = Color.White;
        public string Text { get; set; } = "Text";
        public bool TextShader { get; set; } = true;

        public StaticText() { }
        public StaticText(SpriteFont font, Vector2 pos, Color color, string text)
        {
            Font = font;
            Position = pos;
            Color = color;
            Text = text;
        }
        public void Draw(SpriteBatch sb)
        {
            if (TextShader)
                sb.DrawString(Font, Text, new Vector2(Position.X + 1, Position.Y + 1), Color.Black);
            sb.DrawString(Font, Text, Position, Color);
        }
    }
}
