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
    public class LivingObject
    {
        //public SheetAnimation sprite { get; set; }
        public GridSheet GSheet { get; set; }
        public Texture2D Texture => GSheet.Texture;
        public Vector2 Position { get; set; }
        public Point Point => new Point((int)Position.X / 32, (int)Position.Y / 32);
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public Vector2 CenterBox => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));

        public float Speed { get; set; } = 50f;
        public Vector2 Direction { get; set; } = Vector2.Zero;
        public float VelocityForce { get; set; } = 1f;
        public float Delta { get; set; }

        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }
        public Rectangle[] CurrentAnimation { get; set; }
        public Rectangle CurrentAnimationFrame { get; set; }

        public double AnimationDuration { get; set; } = 1;

        public bool IsAnimating { get; set; } = true;
        public bool IsTimed { get; set; }

        public double Timer;

        public Dictionary<string, Rectangle[]> Animations = new Dictionary<string, Rectangle[]>();

        DynamicBar HealthBar;

        Extras extras;

        List<DynamicText> textList = new List<DynamicText>();

        #region what to draw
        public bool IsDrawHealthBar = true;

        public LivingObject Nemesis;
        public int NemesisTimer;

        #endregion
        public LivingObject(GraphicsDevice gd)
        {
            HealthBar = new DynamicBar(gd, Position, (int)Size.X);
            extras = new Extras();
            //AddAnimation(new int[] { 0, 1, 2, 3 }, 0, "walkdown");
            //AddAnimation(new int[] { 0, 1, 2, 3 }, 1, "walkup");
            //AddAnimation(new int[] { 0, 1, 2, 3 }, 2, "walkright");
            //AddAnimation(new int[] { 0, 1, 2, 3 }, 3, "walkleft");
        }
        public bool IsPlayer = false;
        public bool IsAlive => IsDeadTimer > 0;
        public bool IsDead => Health <= 0;
        public bool IsTriggeredDeath { get; set; }
        public double IsDeadTimer = 10;

        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public void SetHealth(double health) { Health = health; MaxHealth = health; }
        public double PercentHealth => (Health / MaxHealth) * 100;

        public double Mana { get; set; }
        public double MaxMana { get; set; }
        public void SetMana(double mana) { Mana = mana; MaxMana = mana; }
        public double PercentMana => (Mana / MaxMana) * 100;

        public double Level { get; set; }
        public double Exp { get; set; }
        public double MaxExp { get; set; }
        public void SetExp(double exp) { Exp = 0; MaxExp = exp; }
        public double PercentExp => (Exp / MaxExp) * 100;

        public double MinDamage { get; set; }
        public double MaxDamage { get; set; }
        public double Damage => Rng.Noxt((int)MinDamage, (int)MaxDamage);
        public void SetDamage(double min, double max) { MinDamage = min; MaxDamage = max; }

        public float KnockbackPower { get; set; }
        public bool BeingPushed { get; set; }
        public Vector2 PushDirection { get; set; }

        public bool IsHit { get; set; }
        public int IsHitTimer = 10;

        public bool IsAttacking { get; set; }
        public bool CanAttack { get; set; } = true;
        public bool Attacked { get; set; }
        public double IsAttackingTimer { get; set; }
        public double AttackCooldown { get; set; }
        public double AttackCooldownCounter { get; set; }

        //public void HitOtherObject(LivingObject target)
        //{
        //    target.Health -= Damage;
        //    target.Push(Position - target.Position, KnockbackPower);
        //    target.IsHit = true;

        //    DidAttack();
        //}

        public void GetHitBy(LivingObject damageSource)
        {
            Nemesis = damageSource;
            //NemesisTimer = 20;
            ModifyResourceValue("hp", -damageSource.Damage);
            Push(damageSource.Position - Position, damageSource.KnockbackPower);
            IsHit = true;
            //textList.Add(new DynamicText(ScreenManager.DebugFont, Position, Size, new Vector2(0, -1), 10f, Color.DarkRed, "-" + damageSource.Damage.ToString()) { TextShader = true });
            damageSource.DidAttack();
        }
        /// <summary>
        /// push unit to direction with force
        /// </summary>
        public void Push(Vector2 dir, float force)
        {
            BeingPushed = true;
            dir.Normalize();
            PushDirection = dir;
            VelocityForce = force;
        }
        /// <summary>
        /// modify a unit resource
        /// </summary>
        /// <param name="resource">hp, mana, exp</param>
        public void ModifyResourceValue(string resource, double value)
        {
            Color color;
            string msg = "";
            double modify = value;
            switch (resource)
            {
                case "hp":
                    Health = Health + modify;
                    if (modify > 0) { color = Color.ForestGreen; msg = "+"; } else color = Color.DarkRed;
                    textList.Add(new DynamicText(ScreenManager.DebugFont, Position, Size, new Vector2(0, -1), 10f, color, msg + modify.ToString()) { TextShader = true });
                    break;
                case "mana":
                    Mana += modify;
                    if (modify > 0) { color = Color.BlueViolet; msg = "+"; } else color = Color.LightPink;
                    if (modify > 0)
                        textList.Add(new DynamicText(ScreenManager.DebugFont, Position, Size, new Vector2(0, -1), 10f, color, msg + modify.ToString()) { TextShader = true });
                    break;
                case "xp":
                    Exp += modify;
                    if (modify > 0) { color = Color.LightGoldenrodYellow; msg = "+"; } else color = Color.GreenYellow;
                    if (modify > 0)
                        textList.Add(new DynamicText(ScreenManager.DebugFont, Position, Size, new Vector2(0, -1), 10f, color, msg + modify.ToString() + "xp") { TextShader = true });
                    break;
                default:
                    textList.Add(new DynamicText(ScreenManager.DebugFont, Position, Size, new Vector2(0, -1), 10f, Color.White, "INVALID RESOURCE") { TextShader = true });
                    break;
            }
            Health = Helper.Clamp(Health, 0, MaxHealth);
            Mana = Helper.Clamp(Mana, 0, MaxMana);
            if (Exp >= MaxExp)
                LevelUp();
        }
        /// <summary>
        /// unit leveled up (exp>max exp)
        /// </summary>
        public void LevelUp()
        {
            Level++;
            var floodExp = Exp - MaxExp;
            Exp = floodExp;
            MaxExp = (int)((MaxExp * 1.1) + (5 * Level));
            MaxHealth = (int)(MaxHealth * 1.1);
            Health = MaxHealth;
            MaxMana = (int)(MaxMana * 1.1);
            Mana = MaxMana;
            textList.Add(new DynamicText(ScreenManager.DebugFont, CenterBox, Size, new Vector2(0, -1), 10f, Color.LightYellow, "level up!!") { TextShader = true });
        }
        public void OnDeath(PlayingScreen p)
        {
            if (IsPlayer)
            {
                //addScreen.DeathScreen();
                IsTriggeredDeath = true;
                return;
            }
            Nemesis.ModifyResourceValue("xp", MaxExp);
            var rnd = Rng.Noxt(10);
            if (rnd == 5)
                p.ScreenManager.GetScreen<PlayingScreen>().items.Add(new Usable(UsableType.HealthPot, PlayingScreen.itemSheet) { Position = new Vector2(Position.X + Rng.Noxt(-10, 10), Position.Y + Rng.Noxt(-10, 10)) });
            IsTriggeredDeath = true;
        }
        /// <summary>
        /// unit attacked
        /// </summary>
        public void DidAttack()
        {
            Attacked = true;
            AttackCooldown = IsAttackingTimer + 0.25;
        }
        public Color _Color()
        {
            if (IsHit)
                return Color.Red;
            else if (IsDead)
            {
                int alpha = (int)(IsDeadTimer * 255);
                return new Color(Color.Red, alpha);
            }
            else
                return BaseColor;
        }
        public virtual void Update(GameTime gameTime, PlayingScreen p)
        {
            Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (BeingPushed)
                Position += Delta * Speed * PushDirection * -VelocityForce;
            else
                Position += Delta * Speed * Direction;
            AttackCooldown -= Delta;

            HealthBar.Update(Health, MaxHealth, (int)Size.X, Position);

            //if (NemesisTimer > 0)
            //    NemesisTimer = (int)extras.AddEverySecond(gameTime, NemesisTimer, -1, 1f);
            //else Nemesis = null;

            if (IsHit)
            {
                IsHitTimer--;
                if (IsHitTimer < 0)
                {
                    IsHit = false;
                    IsHitTimer = 10;
                }
            }
            if (Attacked)
            {
                AttackCooldownCounter--;
                if (AttackCooldown < 0)
                {
                    Attacked = false;
                }
            }
            if (BeingPushed)
            {
                if (VelocityForce > 0f)
                    VelocityForce -= 0.2f;
                else BeingPushed = false;
            }
            if (IsDead)
            {
                if (!IsTriggeredDeath)
                    OnDeath(p);
                Speed = 0;
                IsDeadTimer -= Delta;
                if (IsDeadTimer < 0)
                    Exist = false;
            }
            foreach (var t in textList)
            {
                t.Update(gameTime);
            }
        }

        #region animation soon fix
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
            CurrentAnimationFrame = Rectangle.Empty;
        }
        public void SetAnimation(Rectangle[] animation, int row)
        {
            CurrentAnimation = animation;
            CurrentRow = row;
            CurrentAnimationFrame = Rectangle.Empty;
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
        #endregion

        public virtual void Draw(SpriteBatch sb, GameTime gt)
        {
            if (CurrentAnimation == null)
                CurrentAnimation = new[] { GSheet[0, 0], };
            if (CurrentAnimationFrame == null)
                CurrentAnimationFrame = CurrentAnimation[0];

            if (IsDrawHealthBar)
                HealthBar.Draw(sb);

            var i = (int)(gt.TotalGameTime.TotalSeconds * CurrentAnimation.Length / AnimationDuration % CurrentAnimation.Length);
            CurrentAnimationFrame = CurrentAnimation[i];
            sb.Draw(Texture, Rectangle, CurrentAnimationFrame, _Color(), 0, Vector2.Zero, SpriteEffects.None, 0);

            foreach (var t in textList)
            {
                t.Draw(sb);
            }
        }
    }
}