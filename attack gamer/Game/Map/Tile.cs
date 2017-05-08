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
        Water,



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
                case TileType.Water:
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
                case 13: break;
                case 14: break;
                case 15: break;
                case 16: break;
                case 17: break;
                case 18: break;
                case 19: break;
                case 20: break;
                case 21: break;
                case 22: break;
                case 23: break;
                case 24: break;
                case 25: break;
                case 26: break;
                case 27: break;
                case 28: break;
                case 29: break;
                case 30: break;
                default: break;
            }
        }
    }
}
