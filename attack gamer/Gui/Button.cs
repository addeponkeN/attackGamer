using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    class Button : Properties
    {
        public StaticText staticText;
        public Sprite outline;
        public bool isOutline = true;
        public bool isText = true;
        Color baseColor;

        public bool IsHovered => Rectangle.Contains(Input.mPos);
        public bool IsPressed => IsHovered && Input.LeftClick();
        public bool IsReleased => IsHovered && Input.LeftRelease();
        public bool IsHold => IsHovered && Input.LeftHold();

        Color ColorNew => IsHold ? new Color(baseColor.R - 10, baseColor.G - 10, baseColor.B - 10) : IsHovered ? new Color(baseColor.R + 10, baseColor.G + 10, baseColor.B + 10) : baseColor;
        Rectangle outLineSize => IsHold ? new Rectangle((int)Position.X - 1, (int)Position.Y - 1, (int)Size.X + 3, (int)Size.Y + 3) : new Rectangle((int)Position.X - 1, (int)Position.Y - 1, (int)Size.X + 2, (int)Size.Y + 2);

        public Button(SpriteFont font, Vector2 position, Vector2 size, string text, Texture2D texture)
        {
            baseColor = new Color(50, 50, 50);
            Color = baseColor;
            Texture = texture;
            Position = position;
            Size = size;
            staticText = new StaticText();
            outline = new Sprite(texture) { Position = new Vector2(Position.X - 1, Position.Y - 1), Size = new Vector2(Size.X + 2, Size.Y + 2), Color = Color.Black };
            var textSize = font.MeasureString(text);
            staticText.Font = font;
            staticText.Text = text;
            staticText.Position = Helper.Center(Position, Size, textSize);
            staticText.Color = Color.White;
        }
        public void SetNewPosition(Vector2 position)
        {
            Position = position;
            var textSize = staticText.Font.MeasureString(staticText.Text);
            staticText.Position = Helper.Center(Position, Size, textSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isOutline)
                spriteBatch.Draw(outline.Texture, outLineSize, outline.Color);
            spriteBatch.Draw(Texture, Rectangle, ColorNew);
            if (isText)
                staticText.Draw(spriteBatch);
        }
    }
}