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
        Player playerRef;
        public Enemy(GridSheet sheet, GraphicsDevice gd, Player player) : base(gd)
        {
            GSheet = sheet;
            playerRef = player;

            AddAnimation(new int[] { 0, 1, 0, 2 }, 0, "walkdown");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 1, "walkup");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 2, "walkleft");
            AddAnimation(new int[] { 0, 1, 0, 2 }, 3, "walkright");

            Speed = 45f;
            Size = new Vector2(32, 32);

            SetHealth(10);
            SetDamage(1, 3);
            SetExp((int)Rng.NoxtDouble(Health * 0.8, Health * 1.2));
            Console.WriteLine(MaxExp);
            KnockbackPower = 2f;
        }
        public override void Update(GameTime gameTime, PlayingScreen p)
        {
            base.Update(gameTime, p);
        }
        public void UpdateMovement(GameTime gameTime)
        {
            var dir = playerRef.Position - Position;
            dir.Normalize();
            Direction = dir;

            if (!IsDead)
            {
                if (Rectangle.Intersects(playerRef.attackBox) && playerRef.IsAttacking && !IsHit)
                {
                    GetHitBy(playerRef);
                }
                if (playerRef.Rectangle.Intersects(Rectangle) && !Attacked)
                {
                    playerRef.GetHitBy(this);
                }
            }
        }
    }
}
