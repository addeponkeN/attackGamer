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
    public class LivingObjectManager
    {

        public List<Enemy> enemies = new List<Enemy>();

        //public T GetFirstObject<T>() where T : LivingObject => list.First(s => s is T) as T;

        public LivingObjectManager()
        {

        }
        public void AddEnemy(Enemy e)
        {
            enemies.Add(e);
        }
        public void Update(GameTime gameTime, Player player)
        {
            foreach (var item in enemies)
            {
                item.Update(gameTime);
                item.UpdateMovement(gameTime, player);
            }

            enemies.RemoveAll(e => !e.IsAlive);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (var item in enemies)
            {
                item.Draw(sb, gameTime);
            }
        }
    }
}
