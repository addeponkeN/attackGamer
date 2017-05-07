﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Spritesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    public enum LoadType
    {
        FromFile,
        Fill,
    }
    public class Map
    {

        public Tile[,] tileMap;

        public int Width;
        public int Height;

        public Map()
        {

        }
        public void SetTile()
        {

        }
        public void LoadMap(LoadType type, int width, int height)
        {
            Width = width;
            Height = height;
            tileMap = new Tile[Width, Height];
            switch (type)
            {
                case LoadType.FromFile:
                    break;
                case LoadType.Fill:
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            tileMap[x, y] = new Tile(TileType.Grass, PlayingScreen.tileSheet);
                            tileMap[x, y].Position = new Vector2(32 * x, 32 * y);
                        }
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tileMap[x, y].Draw(sb, gt);
                }
            }
        }
    }
}
