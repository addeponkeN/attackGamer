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
    public enum ItemType
    {
        Usable, Weapon, Armor
    }
    public class Item
    {
        public ItemType Type { get; set; }
        public GridSheet GSheet { get; set; }
        public Texture2D Texture => GSheet.Texture;
        public Vector2 Position { get; set; }
        public Point Point => new Point((int)Position.X / 32, (int)Position.Y / 32);
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public Vector2 CenterBox => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));

        public float Speed { get; set; }
        public Vector2 Direction { get; set; } = Vector2.Zero;
        public float VelocityForce { get; set; } = 1f;
        public float Delta { get; set; }

        public Vector2 Size { get; set; } = new Vector2(32);
        public Color BaseColor { get; set; } = Color.White;
        public Color Color { get; set; } = Color.White;

        public bool Visible { get; set; } = true;
        public bool Exist { get; set; } = true;

        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;

        public bool Vacuumable { get; set; } = true;
        public bool IsBeingLooted { get; set; }

        public Rectangle SetSource(int column, int row)
        {
            return GSheet[column, row];
        }

        public int Distance = 4;
        public float DistanceTo(Vector2 pos)
        {
            return Vector2.Distance(pos, Position);
        }
        public bool CloseTo(Vector2 pos)
        {
            return DistanceTo(pos) < Distance;
        }
        public void VacuumLoot(GameTime gt, Item item, Vector2 des, Inventory i)
        {
            Delta = (float)gt.ElapsedGameTime.TotalSeconds;
            Speed += 400f * Delta;
            Position += Delta * Speed * Direction;
            var dir = des - Position;
            dir.Normalize();
            Direction = dir;

            if (CloseTo(des))
            {
                switch (item.Type)
                {
                    case ItemType.Usable:
                        i.AddItem(new Usable(GetItem<Usable>().type, GSheet));
                        break;
                    case ItemType.Weapon:
                        i.AddItem(new Weapon(GetItem<Weapon>().type));

                        break;
                    case ItemType.Armor:
                        i.AddItem(new Armor());

                        break;
                }
                item.Exist = false;
            }
        }

        public void Action(Item item, LivingObject player)
        {
            switch (item.Type)
            {
                case ItemType.Usable:
                    GetItem<Usable>().Use(item, player);

                    break;
                case ItemType.Weapon:
                    //GetItem<Weapon>().Use();

                    break;
                case ItemType.Armor:
                    //GetItem<Armor>().Use();

                    break;
                default:
                    break;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, SetSource(Column, Row), Color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public T GetItem<T>() where T : Item => this as T;

    }
}
