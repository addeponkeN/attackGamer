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
    public enum AnimationType
    {
        Walk,

        //...
    }
    public class SheetAnimation
    {
        public GridSheet GSheet { get; set; }
        public Texture2D Texture => GSheet.Texture;
        public Vector2 Position { get; set; }
        public Point Point => new Point((int)Position.X / 32, (int)Position.Y / 32);
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public Vector2 CenterBox => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));

        public float Speed { get; set; } = 50f;
        public Vector2 Direction { get; set; }
        public float Delta { get; set; }
        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        public int CurrentRow { get; set; }
        public Rectangle[] CurrentAnimation { get; set; }
        public double AnimationDuration { get; set; } = 1;

        public bool IsAnimating { get; set; } = true;
        public bool IsTimed { get; set; }

        public double Timer;

        public Dictionary<string, Rectangle[]> Animations = new Dictionary<string, Rectangle[]>();

        public void AddAnimation(int[] column, int row, string name)
        {
            var frames = column.Length;
            Rectangle[] test = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
                test[i] = GSheet[column[i], row];
            Animations.Add(name.ToLower(), test);
        }
        public void Remove(string a)
        {
            Animations.Remove(a);
        }
        public Rectangle[] GetAnimation(string name)
        {
            return Animations[name];
        }
        public void SetAnimation(Rectangle[] animation)
        {
            CurrentAnimation = animation;
        }
        public void SetAnimation(Rectangle[] animation, int row)
        {
            CurrentAnimation = animation;
            CurrentRow = row;
        }
        public Rectangle GetSource(Rectangle[] animation, GameTime gt)
        {
            var i = (int)(gt.TotalGameTime.TotalSeconds * animation.Length / AnimationDuration % animation.Length);
            return animation[i];
        }
        public Rectangle SetSource(int column, int row)
        {
            return GSheet[column, row];
        }
        public virtual void Draw(SpriteBatch sb, GameTime gameTime)
        {
            if (CurrentAnimation == null)
                CurrentAnimation = new[] { GSheet[0, 0], };

            if (IsAnimating)
            {
                sb.Draw(GSheet.Texture, Rectangle, GetSource(CurrentAnimation, gameTime), Color);
            }
            else
                sb.Draw(GSheet.Texture, new Rectangle(0, 0, 0, 0), Color);

            if (Size.X < 1 || Size.Y < 1)
                Size = new Vector2(GSheet.SpriteWidth, GSheet.SpriteHeight);
        }
    }
}