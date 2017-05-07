using Microsoft.Xna.Framework;
using MonoGame.Spritesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public enum UsableType
    {
        HealthPot,
        ManaPot
    }
    public class Usable : Item
    {
        public UsableType type;

        public double Value { get; set; }

        public Usable(UsableType type, GridSheet sheet)
        {
            this.type = type;
            GSheet = sheet;

            switch (this.type)
            {
                case UsableType.HealthPot:
                    Value = 10;
                    break;
                case UsableType.ManaPot:
                    Value = 5;
                    Column = 1;
                    break;
            }

            Type = ItemType.Usable;
        }

        public void Use(Item item, LivingObject o)
        {
            var i = item.GetItem<Usable>();
            switch (i.type)
            {
                case UsableType.HealthPot:
                    if (o.Health >= o.MaxHealth)
                    {
                        Console.WriteLine("full hp");
                        return;
                    }
                    Console.WriteLine("hp restored");
                    o.ModifyResourceValue("hp",i.Value);
                    Console.WriteLine(i.Value);
                    break;
                case UsableType.ManaPot:
                    o.Mana += i.Value;
                    break;
            }
            i.Exist = false;
        }
    }
}
