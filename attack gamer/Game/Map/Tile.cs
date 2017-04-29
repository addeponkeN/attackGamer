using Microsoft.Xna.Framework;
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

    }
    public class Tile
    {
        public int Width;
        public int Height;

        public Vector2 Position { get; set; }
        public Point Point => new Point((int)Position.X / 32, (int)Position.Y / 32);
        public Rectangle Rectangle => new Rectangle(Convertor.ToPoint(Position), new Point(Width, Height));

        public bool Walkable { get; set; }        
    }
}
