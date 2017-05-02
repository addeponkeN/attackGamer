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
            Direction = new Vector2(-1, 0);
            Texture = Extras.NewTexture(gd);
            AliveTime = 2f;
            itemRec = new Rectangle((int)Position.X, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 32, 32);

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
            Size = new Vector2(32 + Text.Size.X + 8, 32);
            itemRec = new Rectangle((int)Position.X, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 32, 32);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            itemRec = new Rectangle((int)Position.X, (int)Helper.Center(Rectangle, new Vector2(itemRec.Width, itemRec.Height)).Y, 32, 32);
            Text.Position = new Vector2(itemRec.X + itemRec.Width, Helper.Center(itemRec, Text.Size).Y);            
        }
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(item.Texture, itemRec, item.SetSource(item.Column, item.Row), new Color(item.Color, Alpha));            
        }
    }
}
