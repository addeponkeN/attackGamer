﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Spritesheet;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace attack_gamer
{
    class PlayingScreen : GameScreen
    {
        ContentManager content;
        Camera cam;
        Player player;

        GridSheet playerSheet, swingSheet, skeleSheet, goblinSheet;

        Enemy goblin;
        
        public PlayingScreen()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            playerSheet = content.Load<GridSheet>("playerSheet");
            swingSheet = content.Load<GridSheet>("swing");
            goblinSheet = content.Load<GridSheet>("goblinSheet");
            skeleSheet = content.Load<GridSheet>("skeleSheet");

            player = new Player(playerSheet, swingSheet, ScreenManager.GraphicsDevice) { Position = new Vector2(350) };
            goblin = new Enemy(goblinSheet, ScreenManager.GraphicsDevice) { Position = new Vector2(450) };
            

            cam = new Camera();



            cam.pos = Globals.ScreenCenter;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            player.Update(gameTime);
            goblin.Update(gameTime);
            goblin.EUpdate(gameTime, player);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (Input.KeyClick(Keys.Escape))
                ExitScreen();

        }
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, new Color(35, 35, 35), 0, 0);
            SpriteBatch sb = ScreenManager.SpriteBatch;
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, null, cam.get_transformation(ScreenManager.GraphicsDevice));
            // draw here ---------------------------------------
            player.Draw(sb, gameTime);

            goblin.Draw(sb, gameTime);
            
            sb.DrawString(ScreenManager.DebugFont, "playing", Vector2.One, Color.Black);
            sb.DrawString(ScreenManager.DebugFont, "playing", Vector2.Zero, Color.White);

            sb.End();
            base.Draw(gameTime);
        }
    }
}