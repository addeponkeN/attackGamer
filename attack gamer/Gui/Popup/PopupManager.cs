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
            p.Newest = true;
            list.Add(p);
            if (list.Count > 10)
                list.RemoveAt(0);
            if (list.Count >= 1)
            {
                //if (list.Any(g => g.Text.Msg == p.Text.Msg))
                //{
                //    p.Quantity++;
                //    p.AliveTime = 5f;
                //}
                //else
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        list[i].Newest = false;
                        list[i].Position = new Vector2(list[i].Position.X, list[i].Position.Y - (list[i].Size.Y));
                    }
            }
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Update(gt);
            }
            if (list.Count > 0)
            {
                list.RemoveAll(p => !p.Exist);
            }

        }
        public void Draw(SpriteBatch sb)
        {
            foreach (var p in list)
            {
                p.Draw(sb);
            }
        }
    }
}
