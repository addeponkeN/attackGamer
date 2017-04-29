using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public InventorySlotState State = InventorySlotState.Open;
        public Item Item;
        public Vector2 ItemOldPositon = Vector2.Zero;
        public Sprite box;

        public bool IsHovered => Item != null && Item.Rectangle.Contains(Input.mPos);
        public bool IsRightClicked => IsHovered && Input.RightClick();
        public bool IsLeftClicked => IsHovered && Input.LeftClick();
        public bool IsDragging;

        public InventorySlot() { }
        public InventorySlot(Item item, GraphicsDevice gd)
        {
            Texture2D Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.SandyBrown, (int)50) };
            Texture.SetData(colorData);
            box = new Sprite(Texture);
            box.Size = new Vector2(32);
            Item = item;
            if (item != null)
                item.Color = new Color(Rng.Noxt(255), Rng.Noxt(255), Rng.Noxt(255));
        }
        public InventorySlot(Item item, GraphicsDevice gd, bool test)
        {
            Texture2D Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.SandyBrown, (int)50) };
            Texture.SetData(colorData);
            box = new Sprite(Texture);
            box.Size = new Vector2(32);
            Item = item;
            Item.Color = item.Color;
        }
        public InventorySlot Copy()
        {
            InventorySlot temp = new InventorySlot();
            temp.box = box;
            temp.ItemOldPositon = ItemOldPositon;
            temp.Item = Item;
            return temp;
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
        Sprite P;
        GraphicsDevice grap;
        public Inventory(GraphicsDevice gd)
        {
            grap = gd;
            slots = new InventorySlot[columns, rows];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    slots[x, y] = new InventorySlot(null, gd);
                }
            Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.Black, (int)50) };
            Texture.SetData(colorData);

            P = new Sprite(Texture);
            P.Size = new Vector2(32 * columns, 32 * rows);
            P.Position = new Vector2(32 * 25, 32 * 10);
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
                    Console.WriteLine($"added: {item}");
                    slots[x, y] = new InventorySlot(item, grap, false);
                    //slots[x, y].Item.Position = new Vector2(P.Position.X + (x * 32), P.Position.Y + (y * 32));
                    Console.WriteLine($"slot: {slots[x, y].Item.Exist}   i: {item.Exist}");
                    breakLoops = true;
                    if (breakLoops)
                        break;
                }
                if (breakLoops)
                    break;
            }
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
        public void DragItem(InventorySlot slot)
        {
            if (slot.ItemOldPositon == Vector2.Zero)
                slot.ItemOldPositon = slot.Item.Position;

            slot.Item.Position = new Vector2(Input.mPos.X - (slot.Item.Size.X / 2), Input.mPos.Y - (slot.Item.Size.Y / 2));

            if (Input.LeftRelease())
            {
                if (!P.Rectangle.Contains(Input.mPos))
                {
                    slot.Item.Position = slot.ItemOldPositon;
                    slot.ItemOldPositon = Vector2.Zero;
                    slot.IsDragging = false;
                }
                else
                    SwapSlots(slot);
            }
        }
        public void SwapSlots(InventorySlot holding)
        {
            InventorySlot temp = new InventorySlot(holding.Item, grap, false);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (slots[x, y].box.Rectangle.Contains(Input.mPos))
                    {
                        //if (slots[x, y].State == InventorySlotState.Closed)
                        //{
                        //    holding = new InventorySlot(slots[x, y].Item, grap, false);
                        //    slots[x, y] = new InventorySlot(temp.Item, grap, false);

                        //    Console.WriteLine("swapped");
                        //    holding.IsDragging = false;
                        //    slots[x, y].IsDragging = false;
                        //    return;
                        //}
                        //else
                        //{
                        if (slots[x, y].State == InventorySlotState.Open)
                        {
                            holding.Item = null;
                            slots[x, y] = temp;

                            Console.WriteLine($"moved  x: {x}   y:{y}");
                            holding.IsDragging = false;
                            return;
                        }
                        else
                        {
                            holding.Item.Position = holding.ItemOldPositon;
                            holding.IsDragging = false;
                            return;
                        }
                        //}
                    }
                }
            }

        }

        public void Draw(SpriteBatch sb, Player player)
        {
            P.Draw(sb);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (slots[x, y] != null)
                    {

                        var slot = slots[x, y];
                        var item = slots[x, y].Item;
                        if (slot.Item != null)
                        {
                            if (!slot.IsDragging)
                                item.Position = slot.box.Position;
                            sb.Draw(item.Texture, item.Rectangle, item.SetSource(item.Column, item.Row), item.Color);
                        }
                        slot.box.Position = new Vector2((int)P.Position.X + (x * 32), (int)P.Position.Y + (y * 32));
                        sb.Draw(slot.box.Texture, new Rectangle(Convertor.ToPoint(slot.box.Position), Convertor.ToPoint(slot.box.Size)), slot.box.Color);

                        if (slots[x, y].Item == null)
                        {
                            slots[x, y].State = InventorySlotState.Open;
                            //slot.box.Color = new Color(Color.Green, 150);
                        }
                        else
                        {
                            slots[x, y].State = InventorySlotState.Closed;
                            //slot.box.Color = new Color(Color.Red, 150);
                        }

                        if (slot.IsRightClicked)
                            item.GetItem<Usable>().Use(item, player);
                        if (slot.Item != null && !item.Exist)
                            slot.RemoveItem();

                        if (slot.IsLeftClicked)
                            slot.IsDragging = true;
                        if (slot.IsDragging)
                            DragItem(slot);
                        if (slots[x, y].box.Rectangle.Contains(Input.mPos))
                        {
                            slots[x, y].box.Color = new Color(Color.White, 255);
                        }
                        else slots[x, y].box.Color = new Color(Color.White, 200);
                        if (slot.IsHovered && Input.KeyClick(Keys.Delete))
                            slot.RemoveItem();
                    }
                }
            }
        }
    }
}
