using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public enum InventorySlotState
    {
        Closed, Open
    }
    public class InventorySlot
    {
        public InventorySlotState State { get; set; } = InventorySlotState.Open;
        public Item Item { get; set; }
        public InventorySlot(Item item)
        {
            Item = item;
        }
        public void RemoveItem()
        {
            Item = null;
            State = InventorySlotState.Open;
        }
    }
    public class Inventory
    {
        public int rows = 2;
        public int columns = 10;
        public InventorySlot[,] slots;
        List<Usable> list = new List<Usable>();

        Texture2D Texture;
        Texture2D itemText;
        Sprite P;

        public Inventory(GraphicsDevice gd)
        {
            slots = new InventorySlot[columns, rows];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    slots[x, y] = new InventorySlot(null);
                }
            itemText = new Texture2D(gd, 1, 1);
            Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.SandyBrown, (int)50) };
            Texture.SetData(colorData);

            P = new Sprite(Texture);
            P.Size = new Vector2(33 * columns, 33 * rows);
            P.Position = new Vector2(800, 300);
        }
        public void AddItem(Item item)
        {
            bool breakLoops = false;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (slots[x, y].State == InventorySlotState.Closed)
                        continue;

                    Console.WriteLine("yes!");
                    slots[x, y] = new InventorySlot(item);
                    slots[x, y].Item.Position = new Vector2(P.Position.X + (x * 32) + x, P.Position.Y + (y * 32) + y);
                    breakLoops = true;


                    if (breakLoops)
                        break;
                }
                if (breakLoops)
                    break;
            }

            //var i = slots.Length;
            //int x = 0, y = 0;
            //for (int a = 0; a < i + 1; a++)
            //{
            //    x++;
            //    if (a >= 3)
            //    {
            //        y++;
            //        x = 0;
            //    }
            //}
            //Console.WriteLine($"x{x}  y{y}");
            //item.Position = new Vector2(P.Position.X + (x * 32) + x, P.Position.Y + (y * 32) + y);
        }
        public void AddItemList(Usable item)
        {
            var i = list.Count;
            int x = 0; int y = 0;
            int time = 0;
            for (int a = 1; a < i + 1; a++)
            {
                var count = a - time;
                x++;
                if (count >= 5)
                {
                    y++;
                    x = 0;
                    time += 5;
                }
            }
            item.Position = new Vector2(P.Position.X + (x * 32) + x, P.Position.Y + (y * 32) + y);
            list.Add(item);
        }

        public void Update(Player player)
        {
            foreach (var item in list)
            {
                if (item.IsRightClicked)
                    item.Use(item, player);
            }

            list.RemoveAll(it => !it.Exist);
        }

        public void Draw(SpriteBatch sb, Player player)
        {
            P.Draw(sb);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (slots[x, y] != null && slots[x, y].Item != null)
                    {
                        var slot = slots[x, y];
                        var item = slots[x, y].Item;
                        sb.Draw(item.Texture, item.Rectangle, item.SetSource(item.Column, item.Row), item.Color);
                        //Console.WriteLine("drawing slot");
                        if (slots[x, y].Item == null)
                            slots[x, y].State = InventorySlotState.Open;
                        else
                            slots[x, y].State = InventorySlotState.Closed;

                        if (item.IsRightClicked)
                            item.GetItem<Usable>().Use(item, player);
                        if (slot.Item != null && !item.Exist)
                            slot.RemoveItem();
                    }
                }
            }

            foreach (var item in list)
            {
                //Color[] colorData = { new Color(Color.OrangeRed, (int)255) };
                //Texture.SetData(colorData);
                //sb.Draw(Texture, item.Rectangle, Color.MonoGameOrange);

                item.Draw(sb);
            }
        }

    }
}
