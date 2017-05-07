using Microsoft.Xna.Framework;
using MonoGame.Spritesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public enum TileType
    {
        Grass,
        Tree,
        Stone,
        Bush,


    }
    public class Tile : SheetAnimation
    {
        public int Width;
        public int Height;

        public int ID { get; set; }        

        public bool Walkable { get; set; }

        

        public Tile(TileType type, GridSheet sheet)
        {
            IsAnimating = false;
            GSheet = sheet;
            switch (type)
            {
                case TileType.Grass:
                    CurrentColumn = Rng.Noxt(3);
                    CurrentRow = 0;
                    ID = CurrentColumn;
                    break;
                case TileType.Tree:
                    break;
                case TileType.Stone:
                    break;
                case TileType.Bush:
                    break;
            }
        }
        public Tile(int id, GridSheet sheet)
        {
            GSheet = sheet;
            switch (id)
            {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 5: break;
                case 6: break;
                case 7: break;
                case 8: break;
                case 9: break;
                case 10: break;
                case 11: break;
                case 12: break;
                default: break;
            }
        }
    }
}
