using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace attack_gamer
{
    public class LootPopup : Popup
    {
        Item item;
        Rectangle itemRec;

        public LootPopup(Item it, GraphicsDevice gd)
        {
            item = it;
            Position = new Vector2(0, (int)(Globals.ScreenY * 0.666));
            Direction = new Vector2(0, -1);
            Texture = Extras.NewTexture(gd);
            AliveTime = 2f;
            itemRec = new Rectangle(0, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 64, 64);

            switch (item.Type)
            {
                case ItemType.Usable:
                    Text.Msg = item.GetItem<Usable>().type.ToString();
                    break;
                case ItemType.Weapon:
                    Text.Msg = item.GetItem<Weapon>().type.ToString();
                    break;
                case ItemType.Armor:
                    //Text = $"{item.GetItem<Armor>().type}";
                    break;
                default:
                    break;
            }
            Console.WriteLine(Text.Size.X); 
            Size = new Vector2(itemRec.Width + (Text.Size.X * 4), 64);
            itemRec = new Rectangle(0, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 64, 64);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            itemRec = new Rectangle(0, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 64, 64);
            Text.Position = new Vector2(itemRec.X + itemRec.Width, Helper.Center(itemRec, Text.Size).Y);            
        }
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(item.Texture, itemRec, item.SetSource(item.Column, item.Row), new Color(item.Color, Alpha));            
        }
    }
}
