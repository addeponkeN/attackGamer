using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Spritesheet;
using Microsoft.Xna.Framework.Input;

namespace attack_gamer
{
    public class Player : LivingObject
    {
        public Inventory inventory;

        #region swinger
        //public Vector2 attackPos;
        //public Rectangle attackBox => new Rectangle((int)(Position.X + (Size.X / 2)), (int)(Position.Y + (Size.Y / 2)), attackWidth, attackLength);
        //public Rectangle attackBox => new Rectangle((int)attackPos.X, (int)attackPos.Y, attackWidth, attackLength);
        public Rectangle attackBox;
        //public Vector2 attackDirection;
        //public Vector2 attackOrigin;
        //public float attackRotation = 0;
        SheetAnimation Swing;
        //public Vector2 swingPos;
        public Rectangle swingBox => new Rectangle((int)(Position.X + (Size.X / 2)), (int)(Position.Y + (Size.Y / 2)), (int)Swing.Size.X + 16, (int)Swing.Size.Y);
        //public Rectangle swingBox;
        public SpriteEffects swingEffect;
        public Vector2 swingOrigin;
        public float swingRotation = 0;
        #endregion

        public double attackCd;
        public int attackWidth, attackLength;

        public int LootRadiusSize = 128;
        public Rectangle LootRadius => new Rectangle((int)(Position.X + (Size.X / 2) - (LootRadiusSize / 2)), (int)(Position.Y + (Size.Y / 2) - (LootRadiusSize / 2)), LootRadiusSize, LootRadiusSize);


        public Player(GridSheet shet, GridSheet swing, GraphicsDevice gd) : base(gd)
        {
            GSheet = shet;
            KnockbackPower = 4f;

            Swing = new SheetAnimation();
            Swing.GSheet = swing;
            Swing.AddAnimation(new int[] { 0, 1, 2, 3 ,3,3,3}, 0, "swing");
            Swing.AddAnimation(new int[] { 0 }, 0, "test");

            Swing.Size = new Vector2(32, 96);

            Swing.CurrentAnimation = Swing.Animations["swing"];
            Swing.AnimationDuration = 0.25;

            AddAnimation(new int[] { 0, 1, 2, 3 }, 0, "walkdown");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 1, "walkup");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 2, "walkright");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 3, "walkleft");
            Speed = 300f;
            AnimationDuration = 1;

            attackWidth = 48;
            attackLength = 64;
            //attackOrigin = new Vector2(Size.X / 2, Size.Y);
            swingOrigin = new Vector2(40, 48);

            SetHealth(20);
            SetDamage(2, 5);
            inventory = new Inventory(gd);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //attackDirection = Input.mPos - new Vector2(attackBox.X, attackBox.Y);
            //attackRotation = (float)Math.Atan2(attackDirection.Y, attackDirection.X);

            //Swing.Direction = Input.mPos - new Vector2(swingBox.X, swingBox.Y);
            //swingRotation = (float)Math.Atan2(Swing.Direction.Y, Swing.Direction.X);

            //swingPos = RotateAround(swingPos, new Vector2(Position.X + (Size.X / 2), (int)Position.Y + (Size.Y / 2)), swingRotation);
            inventory.Update(gameTime, this);

            if (Input.LeftClick())
            {
                if (CanAttack && !Attacked)
                {
                    IsAttacking = true;
                    Action(gameTime);
                }
            }
            else
            {
                IsAttacking = false;
                IsAttackingTimer -= Delta;
            }

            #region Keybindongs

            if (Input.KeyClick(Keys.I) || Input.KeyClick(Keys.B))
                if (inventory.IsDrawing)
                    inventory.IsDrawing = false;
                else inventory.IsDrawing = true;

            Direction = new Vector2(0);
            if (Direction == new Vector2(0))
                SetAnimation(new[] { GSheet[0, CurrentRow] });
            if (!Attacked)
            {
                if (Input.KeyHold(Keys.W))
                {
                    Direction = new Vector2(Direction.X, -1);
                    SetAnimation(Animations["walkup"]);
                }
                if (Input.KeyHold(Keys.S))
                {
                    Direction = new Vector2(Direction.X, 1);
                    SetAnimation(Animations["walkdown"]);
                }
                if (Input.KeyHold(Keys.A))
                {
                    Direction = new Vector2(-1, Direction.Y);
                    SetAnimation(Animations["walkleft"]);
                }
                if (Input.KeyHold(Keys.D))
                {
                    Direction = new Vector2(1, Direction.Y);
                    SetAnimation(Animations["walkright"]);
                }

                

                #endregion

                #region triangle
                // triangle above player
                if (Helper.IsPointInTri(Input.mPos, new Vector2(0), new Vector2(Globals.ScreenX, 0), Position))
                {
                    CurrentRow = 1;
                    attackBox = new Rectangle((int)Position.X - 32 - 8, (int)Position.Y - (int)Size.Y - 8, (int)Swing.Size.Y + 16, (int)Swing.Size.X + 16);
                    swingRotation = MathHelper.ToRadians(270);
                }

                // triangle under player
                if (Helper.IsPointInTri(Input.mPos, new Vector2(0, Globals.ScreenY), Globals.ScreenXY, Position))
                {
                    CurrentRow = 0;
                    attackBox = new Rectangle((int)Position.X - 32 - 8, (int)Position.Y + 32 - 8, (int)Swing.Size.Y + 16, (int)Swing.Size.X + 16);
                    swingRotation = MathHelper.ToRadians(90);
                }

                // triangle left player
                if (Helper.IsPointInTri(Input.mPos, new Vector2(0), new Vector2(0, Globals.ScreenY), Position))
                {
                    CurrentRow = 3;
                    attackBox = new Rectangle((int)Position.X - 32 - 17 + 8, (int)Position.Y - 32 - 8, (int)Swing.Size.X + 16, (int)Swing.Size.Y + 16);
                    swingRotation = MathHelper.ToRadians(180);
                }

                // triangle right player
                if (Helper.IsPointInTri(Input.mPos, new Vector2(Globals.ScreenX, 0), new Vector2(Globals.ScreenX, Globals.ScreenY), Position))
                {
                    CurrentRow = 2;
                    attackBox = new Rectangle((int)Position.X + 32 - 8, (int)Position.Y - 32 - 8, (int)Swing.Size.X + 16, (int)Swing.Size.Y + 16);
                    swingRotation = MathHelper.ToRadians(0);
                }
            }

            #endregion

            #region test
            if (Input.KeyHold(Keys.Left))
            {
            }
            if (Input.KeyHold(Keys.Right))
            {
            }
            if (Input.KeyHold(Keys.Up))
            {
            }
            if (Input.KeyHold(Keys.Down))
            {
            }
            #endregion

        }
        public void Action(GameTime gameTime)
        {
            IsAttackingTimer = Swing.AnimationDuration;

            DidAttack();
        }

        public void Loot(Item item)
        {
            inventory.AddItem(item);            
        }

        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            base.Draw(sb, gameTime);

            if (IsAttackingTimer > 0)
            {
                sb.Draw(Swing.GSheet.Texture, swingBox, Swing.GetSource(Swing.CurrentAnimation, gameTime), Swing.Color, swingRotation + ((float)Math.PI * 1f), swingOrigin, swingEffect, 0f);

            }

            //if(IsAttacking)
            //sb.Draw(ScreenManager.box, attackBox, Swing.GetSource(Swing.CurrentAnimation, gameTime), new Color(Color.Green, 0.2f));
            sb.Draw(ScreenManager.box, LootRadius, new Color(Color.Green, 0.2f));


            inventory.Draw(sb, this);

            sb.DrawString(ScreenManager.DebugFont, "" + swingBox, new Vector2(0, 60), Color.White);
            sb.DrawString(ScreenManager.DebugFont, "" + swingOrigin, new Vector2(0, 80), Color.White);
            sb.DrawString(ScreenManager.DebugFont, "" + swingRotation, new Vector2(0, 100), Color.White);
        }
    }
}