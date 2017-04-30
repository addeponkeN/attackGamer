using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public class PopupManager
    {

        List<Popup> list = new List<Popup>();
        public void AddPopup(Popup p)
        {
            Console.WriteLine(p.Position);
            list.Add(p);
            if (list.Count >= 1)
                for (int i = 0; i < list.Count - 1; i++)
                    list[i].Position = new Vector2(list[i].Position.X, list[i].Position.Y - (list[i].Size.Y));
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i < 1)
                    list[i].Newest = true;
                else list[i].Newest = false;
                list[i].Update(gt);
                //list[i].Position = new Vector2(list[i].Position.X, list[i].Position.Y - (list[i].Size.Y * i));
            }
            if (list.Count > 0)
            {
                list.RemoveAll(p => !p.Exist);
            }
        }
        public void Draw(SpriteBatch sb)
        {
            foreach (var item in list)
            {
                item.Draw(sb);
            }
        }
    }
}
