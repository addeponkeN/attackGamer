using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public static class Globals
    {
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        public static Vector2 ScreenPosition { get; set; }
        public static Vector2 ScreenSize { get { return new Vector2(ScreenWidth, ScreenHeight); } }  // update this with camera pos
        public static Vector2 ScreenCenter = new Vector2(ScreenWidth / 2, ScreenHeight / 2);

        public static Rectangle ScreenRectangle { get { return new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, ScreenWidth, ScreenHeight); } }
        public static Vector2 ScreenTopLeft => new Vector2(ScreenRectangle.X, ScreenRectangle.Y);
        public static Vector2 ScreenTopRight => new Vector2(ScreenRectangle.X + ScreenRectangle.Width, ScreenRectangle.Y);
        public static Vector2 ScreenBotLeft => new Vector2(ScreenRectangle.X, ScreenRectangle.Y + ScreenRectangle.Height);
        public static Vector2 ScreenBotRight => new Vector2(ScreenRectangle.X + ScreenRectangle.Width, ScreenRectangle.Y + ScreenRectangle.Height);

        public static Rectangle ScreenBoundBox { get { return new Rectangle(ScreenRectangle.X - 32, ScreenRectangle.Y - 32, ScreenRectangle.Width + 64, ScreenRectangle.Height + 64); } }

        public static Rectangle ScreenTop { get { return new Rectangle(ScreenRectangle.X, ScreenRectangle.Y, ScreenRectangle.Width, 4); } }
        public static Rectangle ScreenBot { get { return new Rectangle(ScreenRectangle.X, (ScreenRectangle.Y + ScreenHeight) - 4, ScreenRectangle.Width, 4); } }
        public static Rectangle ScreenLeft { get { return new Rectangle(ScreenRectangle.X, ScreenRectangle.Y, 4, ScreenHeight); } }
        public static Rectangle ScreenRight { get { return new Rectangle(ScreenRectangle.X + ScreenWidth - 4, ScreenRectangle.Y, 4, ScreenHeight); } }

        public static bool debug;         // F1 toggle
    }


    public class Extras
    {
        private static float counter;
        public double AddEverySecond(GameTime gt, double value, double add, float everyXsecond)
        {
            counter += (float)gt.ElapsedGameTime.TotalSeconds;
            if (counter >= everyXsecond)
            {
                counter -= everyXsecond;
                return value + add;
            }
            return value;
        }

        public static void DrawString(SpriteBatch sb, SpriteFont font, string text, Vector2 position, Color color)
        {
            sb.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            sb.DrawString(font, text, position, color);
        }
        public static void DrawDebug(SpriteBatch sb, string text, Vector2 position)
        {
            sb.DrawString(ScreenManager.DebugFont, text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            sb.DrawString(ScreenManager.DebugFont, text, position, Color.White);
        }
        public static void DrawDebug(SpriteBatch sb, string text, int topLeftLine)
        {
            sb.DrawString(ScreenManager.DebugFont, text, new Vector2(1, (14 * topLeftLine) + 1), Color.Black);
            sb.DrawString(ScreenManager.DebugFont, text, new Vector2(0, (14 * topLeftLine)), Color.White);
        }
        public static Texture2D NewTexture(GraphicsDevice gd)
        {
            Texture2D Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.Black, (int)50) };
            Texture.SetData(colorData);
            return Texture;
        }
    }
    public static class Convertor
    {
        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
    }
    public static class Helper
    {
        public static bool IsBetween(int value, int min, int max)
        {
            if (value <= min)
                return false;
            if (value >= max)
                return false;
            return true;                
        }
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public static bool IsPointInTri(Vector2 vsource, Vector2 vv1, Vector2 vv2, Vector2 vv3)
        {
            var source = new Point((int)vsource.X, (int)vsource.Y);
            var v1 = new Point((int)vv1.X, (int)vv1.Y);
            var v2 = new Point((int)vv2.X, (int)vv2.Y);
            var v3 = new Point((int)vv3.X, (int)vv3.Y);
            bool b1, b2, b3;
            b1 = Sign(source, v1, v2) > 0.0f;
            b2 = Sign(source, v2, v3) > 0.0f;
            b3 = Sign(source, v3, v1) > 0.0f;
            return ((b1 == b2) && (b2 == b3));
        }
        public static bool IsPointInTri(Point source, Point v1, Point v2, Point v3)
        {
            bool b1, b2, b3;
            b1 = Sign(source, v1, v2) > 0.0f;
            b2 = Sign(source, v2, v3) > 0.0f;
            b3 = Sign(source, v3, v1) > 0.0f;
            return ((b1 == b2) && (b2 == b3));
        }
        private static float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }
        public static Vector2 Center(Vector2 center, Vector2 thisSize)
        {
            return new Vector2(center.X - (thisSize.X * 0.5f), center.Y - (thisSize.Y * 0.5f));
        }
        public static Vector2 Center(Rectangle rec, Vector2 thisSize)
        {
            return new Vector2(rec.X + (rec.Width * 0.5f) - (thisSize.X * 0.5f), rec.Y + (rec.Height * 0.5f) - (thisSize.Y * 0.5f));
        }
        public static int RoundUp(double input, int roundTo)
        {
            var i = Math.Round(input);
            var output = (((int)i / roundTo) + 1) * roundTo;
            return output;
        }
        public static int RoundDown(double input, int roundTo)
        {
            var i = Math.Round(input);
            var output = (((int)i / roundTo) - 1) * roundTo;
            return output;
        }
        public static Vector2 FixPos(Vector2 mpos, int roundTo)
        {
            var x = Math.Round(mpos.X);
            var y = Math.Round(mpos.Y);
            var xx = ((((int)x / roundTo)) * roundTo);
            var yy = ((((int)y / roundTo)) * roundTo);
            return new Vector2((xx), (yy));
        }
        public static Point ToPoint(Vector2 mpos)
        {
            var x = Math.Round(mpos.X);
            var y = Math.Round(mpos.Y);
            return new Point((int)x, (int)y);
        }
        public static Point ToPoint(Vector2 mpos, int roundTo)
        {
            var x = Math.Round(mpos.X) / roundTo;
            var y = Math.Round(mpos.Y) / roundTo;
            var xx = ((((int)x / roundTo) + 1) * roundTo);
            var yy = ((((int)y / roundTo) + 1) * roundTo);
            return new Point((xx) - 1, (yy) - 1);
        }
    }
    public static class Rng
    {
        public static Random rnd = new Random();
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        public static int Next(int min, int max)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int range = max - min + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(min + randomValueInRange);
        }
        public static int Next(int max)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            int range = max - 0 + 1;
            double randomValueInRange = Math.Floor(multiplier * range);
            return (int)(0 + randomValueInRange);
        }
        public static int Noxt(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }
        public static int Noxt(int max)
        {
            return rnd.Next(0, max + 1);
        }
        public static double NoxtDouble(double min, double max)
        {
            return rnd.NextDouble() + rnd.Next((int)min, (int)max + 1);
        }
        public static double NoxtDouble(double max)
        {
            return rnd.NextDouble() + rnd.Next((int)max + 1);
        }
        public static float NoxtFloat(float min, float max)
        {
            return (float)rnd.NextDouble() + rnd.Next((int)min, (int)max + 1);
        }
        public static float NoxtFloat(float max)
        {
            return (float)rnd.NextDouble() + rnd.Next((int)max + 1);
        }

    }
}
