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


    public class Enemy : LivingObject
    {

        public float isHitVelocity = 1f;

        public Enemy(GridSheet sheet, GraphicsDevice gd) : base(gd)
        {
            GSheet = sheet;

            AddAnimation(new int[] { 0, 1, 0, 2 }, 0, "walkdown");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 1, "walkup");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 2, "walkleft");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 3, "walkright");

            Speed = 45f;
            Size = new Vector2(32, 32);

            SetHealth(10);
            SetDamage(1, 3);
            SetExp(10);
            KnockbackPower = 2f;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void UpdateMovement(GameTime gameTime, Player player)
        {
            var dir = player.Position - Position;
            dir.Normalize();
            Direction = dir;

            if (!IsDying)
            {
                if (Rectangle.Intersects(player.attackBox) && player.IsAttacking && !IsHit)
                {
                    GetHitBy(player);
                }
                if (player.Rectangle.Intersects(Rectangle) && !Attacked)
                {
                    player.GetHitBy(this);
                }
            }
        }
    }
}
