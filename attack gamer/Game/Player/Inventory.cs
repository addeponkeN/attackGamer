using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class Inventory
    {

        Item[,] items = new Item[5, 5];
        List<Usable> list = new List<Usable>();

        Texture2D Texture;
        Texture2D itemText;
        Sprite P;

        public Inventory(GraphicsDevice gd)
        {
            itemText = new Texture2D(gd, 1,1);
            Texture = new Texture2D(gd, 1, 1);
            Color[] colorData = { new Color(Color.SandyBrown, (int)50) };
            Texture.SetData(colorData);

            P = new Sprite(Texture);
            P.Size = new Vector2(34 * 5);
            P.Position = new Vector2(800,300);
        }
        public void AddItem(Item item)
        {
            var i = items.Length;
            int x = 0, y = 0;
            for (int a = 0; a < i + 1; a++)
            {
                x++;
                if (a > 5)
                {
                    y++;
                    x = 0;
                }
            }
            items[x, y] = item;
            items[x, y].Position = new Vector2(P.Position.X + (x * 32), P.Position.Y + (y * 32));
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
                Console.WriteLine($"{a}/{i}  x{x}  y{y}");
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

        public void Draw(SpriteBatch sb)
        {
            P.Draw(sb);
            //if (items != null)
            //{
            //    for (int y = 0; y < 5; y++)
            //    {
            //        for (int x = 0; x < 5; x++)
            //        {
            //            if (items[x, y] != null)
            //            {
            //                var item = items[x, y];
            //                item.Draw(sb);
            //            }
            //        }
            //    }
            //}
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
