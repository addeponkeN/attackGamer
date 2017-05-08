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
            Color[] colorData = { new Color(15, 15, 15, 255) };
            Texture.SetData(colorData);
            box = new Sprite(ScreenManager.boxbox, Color.White);
            box.Size = new Vector2(64);
            Item = item;
            if (item != null)
                item.Color = new Color(Rng.Noxt(255), Rng.Noxt(255), Rng.Noxt(255));
        }
        public InventorySlot(Item item, GraphicsDevice gd, bool test)
        {
            Texture2D Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(15, 15, 15, 255) };
            Texture.SetData(colorData);
            box = new Sprite(ScreenManager.boxbox, Color.White);
            box.Size = new Vector2(64);
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
        public void AddItem(Item item)
        {
            if (item != null)
            {
                Item = item;
                State = InventorySlotState.Closed;
                Item.Size = box.Size;
                Item.Position = box.Position;
            }
            else Console.WriteLine("ADDED ITEM WAS NULL");
            //actionbarSlots[i] = new InventorySlot(item, grap, false);
            //actionbarSlots[i].State = InventorySlotState.Closed;
            //actionbarSlots[i].Item.Size = actionbarSlots[i].box.Size;
            //actionbarSlots[i].box.Position = new Vector2((int)actionbar.Position.X + (i * 64), (int)actionbar.Position.Y);
            //actionbarSlots[i].Item.Position = actionbarSlots[i].box.Position;
        }
        public void ClearSlot()
        {
            Item = null;
            State = InventorySlotState.Open;
        }
    }
    public class Inventory
    {
        public int rows = 2;
        public int columns = 10;
        public InventorySlot[,] bagSlots;
        public InventorySlot[] actionbarSlots;

        Texture2D Texture, boxbox;
        public Sprite bagSprite;
        public Sprite actionbar;
        GraphicsDevice grap;

        public bool IsDrawing { get; set; }
        public bool IsBagFull => bagSlots.Cast<InventorySlot>().All(s => s.Item != null);
        public bool IsActionBarFull => actionbarSlots.All(s => s.Item != null);
        public bool IsFull => IsBagFull && IsActionBarFull;

        public Inventory(GraphicsDevice gd)
        {
            grap = gd;
            bagSlots = new InventorySlot[columns, rows];
            actionbarSlots = new InventorySlot[10];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < columns; x++)
                {
                    if (y == 0 && x <= actionbarSlots.Length)
                    {
                        actionbarSlots[x] = new InventorySlot(null, gd);
                    }
                    bagSlots[x, y] = new InventorySlot(null, gd);
                    CheckIfOpen(bagSlots[x, y]);
                }
            Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(15, 15, 15, 255) };
            Texture.SetData(colorData);

            actionbar = new Sprite(Texture) { Size = new Vector2(64 * 10, 64 * 1) };
            actionbar.Position = new Vector2((Globals.ScreenWidth / 2) - (actionbar.Size.X / 2), Globals.ScreenHeight - actionbar.Size.Y);
            bagSprite = new Sprite(Texture)
            {
                Size = new Vector2(64 * columns, 64 * rows),
                Position = new Vector2((Globals.ScreenWidth / 2) - ((columns * 64) / 2), Globals.ScreenHeight - (actionbar.Size.Y * 3) - 8)
            };
        }
        public void AddItem(Item item)
        {
            for (int i = 0; i < actionbarSlots.Length; i++)
            {
                Console.WriteLine(actionbarSlots[i].box.Position);

                if (actionbarSlots[i].State == InventorySlotState.Closed)
                    continue;
                actionbarSlots[i].AddItem(item);
                //actionbarSlots[i] = new InventorySlot(item, grap, false);
                //actionbarSlots[i].State = InventorySlotState.Closed;
                //actionbarSlots[i].Item.Size = actionbarSlots[i].box.Size;
                //actionbarSlots[i].box.Position = new Vector2((int)actionbar.Position.X + (i * 64), (int)actionbar.Position.Y);
                //actionbarSlots[i].Item.Position = actionbarSlots[i].box.Position;
                PlayingScreen.popManager.AddPopup(new LootPopup(item, grap));
                return;
            }
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (bagSlots[x, y].State == InventorySlotState.Closed)
                        continue;
                    bagSlots[x, y].AddItem(item);
                    //bagSlots[x, y] = new InventorySlot(item, grap, false);
                    //bagSlots[x, y].State = InventorySlotState.Closed;
                    //bagSlots[x, y].Item.Size = bagSlots[x, y].box.Size;
                    //bagSlots[x, y].box.Position = new Vector2((int)actionbar.Position.X + (x * 64), (int)actionbar.Position.Y + (y * 64));
                    //bagSlots[x, y].Item.Position = bagSlots[x, y].box.Position;
                    PlayingScreen.popManager.AddPopup(new LootPopup(item, grap));
                    return;
                }
            }
        }
        public void CheckIfOpen(InventorySlot slot)
        {
            if (slot.Item == null)
            {
                slot.State = InventorySlotState.Open;
                slot.box.BaseColor = new Color(Color.Green, 150);
            }
            else
            {
                slot.State = InventorySlotState.Closed;
                slot.box.BaseColor = new Color(Color.Red, 150);
            }
        }
        public void DragItem(InventorySlot slot)
        {
            if (slot.ItemOldPositon == Vector2.Zero)
                slot.ItemOldPositon = slot.Item.Position;

            slot.Item.Position = new Vector2(Input.mPos.X - (slot.Item.Size.X / 2), Input.mPos.Y - (slot.Item.Size.Y / 2));

            if (Input.LeftRelease())
            {
                //Console.WriteLine(Helper.FixPos(new Vector2(slot.Item.Position.X, slot.Item.Position.Y - 8), 64) + "  - " + slot.ItemOldPositon);
                if ((!bagSprite.Rectangle.Contains(Input.mPos) && !actionbar.Rectangle.Contains(Input.mPos)))
                {
                    slot.Item.Position = slot.ItemOldPositon;
                    slot.ItemOldPositon = Vector2.Zero;
                    slot.IsDragging = false;
                }
                else
                {
                    SwapSlots(slot);
                }
            }
        }
        public void SwapSlots(InventorySlot holding)
        {
            InventorySlot temp = new InventorySlot(holding.Item, grap, false);

            for (int i = 0; i < actionbarSlots.Length; i++)
            {
                if (actionbarSlots[i].box.Rectangle.Contains(Input.mPos))
                {
                    if (actionbarSlots[i].State == InventorySlotState.Open)
                    {
                        //holding.State = InventorySlotState.Closed;
                        //holding.Item = null;
                        holding.ClearSlot();
                        actionbarSlots[i] = temp;
                        Console.WriteLine($"moved to action slot {i}");
                        holding.IsDragging = false;
                        return;
                    }
                    else
                    {
                        if (actionbarSlots[i].Item != null)
                        {
                            holding.ClearSlot();
                            holding.AddItem(actionbarSlots[i].Item);
                            actionbarSlots[i] = temp;
                            //holding.Item.Position = holding.ItemOldPositon;
                            holding.IsDragging = false;
                            return;
                        }
                        holding.Item.Position = holding.ItemOldPositon;
                        holding.IsDragging = false;
                        return;

                    }
                }
            }
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (bagSlots[x, y].box.Rectangle.Contains(Input.mPos))
                    {
                        if (bagSlots[x, y].State == InventorySlotState.Open)
                        {
                            holding.State = InventorySlotState.Closed;
                            holding.Item = null;
                            bagSlots[x, y] = temp;

                            Console.WriteLine($"moved to inventory slot {x} {y}");
                            holding.IsDragging = false;
                            //CheckIfOpen(bagSlots[x, y]);
                            return;
                        }
                        else
                        {
                            if (bagSlots[x, y].Item != null)
                            {
                                holding.ClearSlot();
                                holding.AddItem(bagSlots[x, y].Item);
                                bagSlots[x, y] = temp;
                                //holding.Item.Position = holding.ItemOldPositon;
                                holding.IsDragging = false;
                                return;
                            }
                            holding.Item.Position = holding.ItemOldPositon;
                            holding.IsDragging = false;
                            return;
                        }
                    }
                }
            }
        }
        public void ActionX(int i, LivingObject o)
        {
            if (actionbarSlots[i].Item != null)
                actionbarSlots[i].Item.Action(actionbarSlots[i].Item, o);
        }
        public void Update(GameTime gameTime, Player player)
        {

            if (Input.KeyClick(Keys.D1))
            {
                ActionX(0, player);
            }
            if (Input.KeyClick(Keys.D2))
            {
                ActionX(1, player);
            }
            if (Input.KeyClick(Keys.D3))
            {
                ActionX(2, player);
            }
            if (Input.KeyClick(Keys.D4))
            {
                ActionX(3, player);
            }
            if (Input.KeyClick(Keys.D5))
            {
                ActionX(4, player);
            }
            if (Input.KeyClick(Keys.D6))
            {
                ActionX(5, player);
            }
            if (Input.KeyClick(Keys.D7))
            {
                ActionX(6, player);
            }
            if (Input.KeyClick(Keys.D8))
            {
                ActionX(7, player);
            }
            if (Input.KeyClick(Keys.D9))
            {
                ActionX(8, player);
            }
            if (Input.KeyClick(Keys.D0))
            {
                ActionX(9, player);
            }

        }

        public void Draw(SpriteBatch sb, Player player)
        {
            actionbar.Draw(sb);
            if (IsDrawing)
                bagSprite.Draw(sb);
            //-----
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    InventorySlot aslot;
                    if (y == 1)
                    {
                        aslot = actionbarSlots[x];
                        if (aslot.Item != null)
                        {
                            if (!aslot.IsDragging)
                                aslot.Item.Position = aslot.box.Position;
                            aslot.Item.Draw(sb);
                        }

                        aslot.box.Position = new Vector2((int)actionbar.Position.X + (x * 64), (int)actionbar.Position.Y);
                        sb.Draw(aslot.box.Texture, new Rectangle(Convertor.ToPoint(aslot.box.Position), Convertor.ToPoint(aslot.box.Size)), aslot.box.Color);

                        if (aslot.IsRightClicked)
                            aslot.Item.GetItem<Usable>().Use(aslot.Item, player);
                        if (aslot.Item != null && !aslot.Item.Exist)
                            aslot.ClearSlot();

                        if (aslot.IsLeftClicked)
                            aslot.IsDragging = true;
                        if (aslot.IsDragging)
                            DragItem(aslot);

                        if (aslot.IsHovered && Input.KeyHold(Keys.Delete))
                            aslot.ClearSlot();

                        CheckIfOpen(aslot);
                        if (aslot.box.Rectangle.Contains(Input.mPos))
                            aslot.box.Color = new Color(150, 150, 150, 255);
                        else aslot.box.Color = new Color(50, 50, 50, 255);
                    }
                    if (bagSlots[x, y] != null)
                    {
                        var slot = bagSlots[x, y];
                        var item = bagSlots[x, y].Item;

                        #region update when drawing
                        if (IsDrawing)
                        {
                            if (slot.Item != null)
                            {
                                if (!slot.IsDragging)
                                    item.Position = slot.box.Position;
                                //sb.Draw(item.Texture, item.Rectangle, item.SetSource(item.Column, item.Row), item.Color);
                                item.Draw(sb);
                            }
                            slot.box.Position = new Vector2((int)bagSprite.Position.X + (x * 64), (int)bagSprite.Position.Y + (y * 64));
                            sb.Draw(slot.box.Texture, new Rectangle(Convertor.ToPoint(slot.box.Position), Convertor.ToPoint(slot.box.Size)), slot.box.Color);

                            if (slot.IsRightClicked)
                                item.GetItem<Usable>().Use(item, player);
                            if (slot.Item != null && !item.Exist)
                                slot.ClearSlot();

                            if (slot.IsLeftClicked)
                                slot.IsDragging = true;
                            if (slot.IsDragging)
                                DragItem(slot);

                            if (slot.IsHovered && Input.KeyHold(Keys.Delete))
                                slot.ClearSlot();
                        }
                        #endregion
                        #region update always
                        CheckIfOpen(slot);
                        if (bagSlots[x, y].box.Rectangle.Contains(Input.mPos))
                            bagSlots[x, y].box.Color = new Color(150, 150, 150, 255);
                        else slot.box.Color = new Color(50, 50, 50, 255);
                        #endregion
                    }
                }
            }//------
        }
    }
}