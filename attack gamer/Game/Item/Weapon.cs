using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public enum WeaponType
    {
        //weapons
        Sword,

        
        //tools
        Pickaxe,
        Shovel,

    }
    public class Weapon : Item
    {

        public WeaponType type;

        public Weapon(WeaponType type)
        {
            this.type = type;

            switch (this.type)
            {
                case WeaponType.Sword:

                    break;
                case WeaponType.Pickaxe:

                    break;
                case WeaponType.Shovel:

                    break;
            }
        }
    }
}
