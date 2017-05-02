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
    public class Animation
    {
        public GridSheet GSheet { get; set; }

        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }
        public Rectangle[] CurrentAnimation { get; set; }
        public double frameTimer;
        public double frameLength;
        public int frame;

        public void AddAnimation(int[] column, int row, string name, Dictionary<string, Rectangle[]> animations)
        {
            var frames = column.Length;
            Rectangle[] test = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
                test[i] = GSheet[column[i], row];
            animations.Add(name, test);
        }
        public void Update(GameTime gt)
        {
            frameTimer += gt.ElapsedGameTime.TotalSeconds;
            if (frameTimer >= frameLength)
            {
                frameTimer = 0;
                frame = (frame + 1) % CurrentAnimation.Length;
            }
        }
    }
}
