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
        Camera cam;
        GraphicsDevice gd;

        #region swinger
        //public Vector2 attackPos;
        //public Rectangle attackBox => new Rectangle((int)(Position.X + (Size.X / 2)), (int)(Position.Y + (Size.Y / 2)), attackWidth, attackLength);
        //public Rectangle attackBox => new Rectangle((int)attackPos.X, (int)attackPos.Y, attackWidth, attackLength);
        public Rectangle attackBox;
        //public Vector2 attackDirection;
        //public Vector2 attackOrigin;
        //public float attackRotation = 0;
        public SheetAnimation Swing;
        //public Vector2 swingPos;
        public Rectangle swingBox => new Rectangle((int)(Position.X + (Size.X / 2)), (int)(Position.Y + (Size.Y / 2)), (int)Swing.Size.X + 16, (int)Swing.Size.Y);
        //public Rectangle swingBox;
        //public SpriteEffects swingEffect;
        //public Vector2 swingOrigin;
        //public float swingRotation = 0;
        #endregion

        public double attackCd;
        public int attackWidth, attackLength;

        public int LootRadiusSize = 128;
        public Rectangle LootRadius => new Rectangle((int)(Position.X + (Size.X / 2) - (LootRadiusSize / 2)), (int)(Position.Y + (Size.Y / 2) - (LootRadiusSize / 2)), LootRadiusSize, LootRadiusSize);


        public Player(GridSheet shet, GridSheet swing, GraphicsDevice grap, Camera camer) : base(grap)
        {
            IsPlayer = true;
            gd = grap;
            cam = camer;
            GSheet = shet;
            KnockbackPower = 4f;

            Swing = new SheetAnimation();
            Swing.GSheet = swing;
            Swing.AddAnimation(new int[] { 0,3,6,8,9,10,11,12,13,14,15,15}, 0, "swing");
            Swing.AddAnimation(new int[] { 0 }, 0, "test");

            Swing.Size = new Vector2(32, 96);
            Swing.Origin = new Vector2(40, 48);
            Swing.frameLength = 0.025;
            Swing.CurrentAnimation = Swing.Animations["swing"];

            AddAnimation(new int[] { 0, 1, 2, 3 }, 0, "walkdown");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 1, "walkup");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 2, "walkright");
            AddAnimation(new int[] { 0, 1, 2, 3 }, 3, "walkleft");

            Speed = 150;
            AnimationDuration = 0.60;

            attackWidth = 48;
            attackLength = 64;
            //attackOrigin = new Vector2(Size.X / 2, Size.Y);
            
            SetHealth(20);
            SetMana(10);
            SetDamage(2, 5);
            SetExp(10);
            inventory = new Inventory(gd);
        }

        public override void Update(GameTime gameTime, PlayingScreen p)
        {
            base.Update(gameTime, p);

            //attackDirection = Input.mPos - new Vector2(attackBox.X, attackBox.Y);
            //attackRotation = (float)Math.Atan2(attackDirection.Y, attackDirection.X);
            //Swing.Direction = Input.mPos - new Vector2(swingBox.X, swingBox.Y);
            //swingRotation = (float)Math.Atan2(Swing.Direction.Y, Swing.Direction.X);
            //swingPos = RotateAround(swingPos, new Vector2(Position.X + (Size.X / 2), (int)Position.Y + (Size.Y / 2)), swingRotation);
            inventory.Update(gameTime, this);

            if (!Attacked && !inventory.actionbar.Rectangle.Contains(Input.mPos))
            {
                if (Input.LeftHold())
                {

                    bool att = true;
                    if (inventory.bagSprite.Rectangle.Contains(Input.mPos))
                        if (inventory.IsDrawing)
                            att = false;
                    if (att)
                    {
                        IsAttacking = true;
                        Action(gameTime);
                    }
                }
            }
            else
            {
                IsAttacking = false;
                IsAttackingTimer -= Delta;
            }

            #region keybinds
            if (Input.KeyClick(Keys.I) || Input.KeyClick(Keys.B))
                if (inventory.IsDrawing)
                    inventory.IsDrawing = false;
                else inventory.IsDrawing = true;
            #region Movement
            Direction = new Vector2(0);
            if (Direction == new Vector2(0))
                SetAnimation(new[] { GSheet[0, CurrentRow] });
            if (AttackCooldown < 0.1)
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
                if (Helper.IsPointInTri(Input.mWorldPos(cam, gd), Globals.ScreenTopLeft, Globals.ScreenTopRight, CenterBox))
                {
                    var mod = (int)(8);
                    CurrentRow = 1;
                    attackBox = new Rectangle((int)Position.X - (int)Size.X - (mod/2), (int)Position.Y - (int)Size.Y - (mod/2), (int)Swing.Size.Y + mod, (int)Swing.Size.X + mod + 8);
                    Swing.Rotation = MathHelper.ToRadians(270);
                }

                // triangle under player
                if (Helper.IsPointInTri(Input.mWorldPos(cam, gd), Globals.ScreenBotLeft, Globals.ScreenBotRight, CenterBox))
                {
                    var mod = (int)(8);
                    CurrentRow = 0;
                    attackBox = new Rectangle((int)Position.X - (int)Size.X - (mod / 2), (int)Position.Y + (int)Size.Y - (mod / 2) - 8, (int)Swing.Size.Y + mod, (int)Swing.Size.X + mod + 8);
                    Swing.Rotation = MathHelper.ToRadians(90);
                }

                // triangle left player
                if (Helper.IsPointInTri(Input.mWorldPos(cam, gd), Globals.ScreenTopLeft, Globals.ScreenBotLeft, CenterBox))
                {
                    var mod = (int)(8);
                    CurrentRow = 3;
                    attackBox = new Rectangle((int)Position.X - (int)Size.X - (mod/2), (int)Position.Y - (int)Size.Y - (mod / 2), (int)Swing.Size.X + mod + 8, (int)Swing.Size.Y + mod);
                    Swing.Rotation = MathHelper.ToRadians(180);
                }

                // triangle right player
                if (Helper.IsPointInTri(Input.mWorldPos(cam, gd), Globals.ScreenTopRight, Globals.ScreenBotRight, CenterBox))
                {
                    var mod = (int)(8);
                    CurrentRow = 2;
                    attackBox = new Rectangle((int)Position.X + (int)Size.X - (mod / 2) - 8, (int)Position.Y - (int)Size.Y - (mod / 2), (int)Swing.Size.X + mod + 8, (int)Swing.Size.Y + mod);
                    Swing.Rotation = MathHelper.ToRadians(0);
                }
            }

            #endregion
            #endregion

            Swing.Update(gameTime);

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
            Swing.Reset();
            OnAttack();
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
                //Swing.Rotation += ((float)Math.PI * 1f);
                sb.Draw(Swing.GSheet.Texture, swingBox, Swing.CurrentAnimationFrame, Swing.Color, Swing.Rotation + ((float)Math.PI * 1f), Swing.Origin, Swing.SpriteEffect, 0f);
                //Swing.Draw(sb, gameTime);
            }

            //if(IsAttacking)
            //sb.Draw(ScreenManager.box, attackBox, Swing.GetSource(Swing.CurrentAnimation, gameTime), new Color(Color.MonoGameOrange, 0.3f));
            //sb.Draw(ScreenManager.box, LootRadius, new Color(Color.Green, 0.2f));

        }
        public void DrawInventory(SpriteBatch sb)
        {
            inventory.Draw(sb, this);
        }
    }
}