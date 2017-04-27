using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class DynamicBar
    {
        public Texture2D Texture { get; set; }

        public int OutlineSize { get; set; } = 1;
        public Color OutlineColor { get; set; } = Color.Black;
        Rectangle OutlineRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X - OutlineSize,
                    (int)NewPosition.Y - OutlineSize,
                    (int)Size.X + (OutlineSize * 2),
                    (int)Size.Y + (OutlineSize * 2));
            }
        }

        public Color BackgroundColor { get; set; } = Color.DarkRed;
        Rectangle BackgroundRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X,
                    (int)NewPosition.Y,
                    (int)Size.X,
                    (int)Size.Y);
            }
        }

        public Color ForegroundColor { get; set; } = Color.ForestGreen;
        Rectangle ForegroundRectangle
        {
            get
            {
                return new Rectangle(
                    (int)NewPosition.X,
                    (int)NewPosition.Y,
                    (int)Percent,
                    (int)Size.Y);
            }
        }

        Vector2 Size { get { return new Vector2(BarWidth, BarHeight); } }

        public double Min;
        public double MinMax;
        public int BarWidth;
        public int BarHeight = 4;

        public int distanceBetweenObjectY = 2;

        public Vector2 NewPosition { get { return new Vector2(Position.X, Position.Y - ((Size.Y * distanceBetweenObjectY) + OutlineSize)); } }
        public Vector2 Position;

        public bool OutlineActive = true;
        public bool BackgroundActive = true;

        public double Percent;
        public double PercentText;

        //{
        //    get { return (Min / MinMax) * BarWidth; }
        //}

        public DynamicBar(GraphicsDevice gd, Vector2 position, int barWidth)
        {
            Position = new Vector2(position.X, position.Y - (Size.Y * distanceBetweenObjectY));
            BarWidth = barWidth;
            Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { Color.White };
            Texture.SetData(colorData);
        }

        public Vector2 UpdatePosition(Vector2 position)
        {
            return Position = position;
        }
        public void Update(double min, double minMax, int barSizeOrPercent, Vector2 position)
        {
            Console.WriteLine(min);
            BarWidth = barSizeOrPercent;
            Percent = (min / minMax) * barSizeOrPercent;
            PercentText = (min / minMax) * 100;
            UpdatePosition(position);

            MathHelper.Clamp((float)Percent, 0, (float)minMax);
            //Console.WriteLine(Percent);
        }

        //public double Percent => (Min / MinMax) * BarWidth;
        //public double PercentText => (Min / MinMax) * 100;


        public void Draw(SpriteBatch spriteBatch)
        {
            if (OutlineActive)
                spriteBatch.Draw(Texture, OutlineRectangle, OutlineColor);
            if (BackgroundActive)
                spriteBatch.Draw(Texture, BackgroundRectangle, BackgroundColor);
            spriteBatch.Draw(Texture, ForegroundRectangle, ForegroundColor);
        }
    }
}