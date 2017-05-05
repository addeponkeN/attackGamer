using System;
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
        public static PopupManager popManager;

        public static GridSheet playerSheet, swingSheet, skeleSheet, goblinSheet, itemSheet;
        LivingObjectManager loManager;
        List<Item> items = new List<Item>();

        float delay;
        bool spawnEnemy;

        public PlayingScreen()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            playerSheet = content.Load<GridSheet>(@"SpriteSheets/playerSheet");
            swingSheet = content.Load<GridSheet>("SpriteSheets/swing");
            goblinSheet = content.Load<GridSheet>("SpriteSheets/goblinSheet");
            skeleSheet = content.Load<GridSheet>("SpriteSheets/skeleSheet");
            itemSheet = content.Load<GridSheet>("SpriteSheets/itemSheet");
            cam = new Camera();

            player = new Player(playerSheet, swingSheet, ScreenManager.GraphicsDevice, cam) { Position = new Vector2(350) };
            loManager = new LivingObjectManager();
            popManager = new PopupManager();
            //loManager.AddEnemy(new Enemy(goblinSheet, ScreenManager.GraphicsDevice) { Position = new Vector2(400) });



            items.Add(new Usable(UsableType.HealthPot, itemSheet) { Vacuumable = true, Position = new Vector2(450) });
            items.Add(new Usable(UsableType.ManaPot, itemSheet) { Vacuumable = true, Position = new Vector2(200) });




            cam.Position = player.CenterBox;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            player.Update(gameTime);
            loManager.Update(gameTime, player);
            popManager.Update(gameTime);
            cam.Position = player.CenterBox;
            Globals.ScreenPosition = new Vector2(cam.Position.X - (Globals.ScreenWidth / 2), cam.Position.Y - (Globals.ScreenHeight / 2));

            if (!player.Exist)
                ExitScreen();

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Input.s > Input.sO)
            {
                cam.Zoom += (float)0.1;
                Console.WriteLine(cam.Zoom);
            }
            if (Input.s < Input.sO)
            {
                cam.Zoom -= (float)0.1;
                Console.WriteLine(cam.Zoom);
            }
            if (Input.KeyClick(Keys.Back))
                cam.Reset();
            Input.sO = Input.s;


            if (Input.KeyClick(Keys.Enter))
                if (spawnEnemy)
                    spawnEnemy = false;
                else spawnEnemy = true;

            if (spawnEnemy)
            {
                delay += delta;
                if (delay > 3)
                {
                    loManager.AddEnemy(new Enemy(goblinSheet, ScreenManager.GraphicsDevice) { Position = new Vector2(Rng.Noxt(Globals.ScreenWidth), Rng.Noxt(Globals.ScreenHeight)) });

                    delay -= 1;
                }
            }

            if (Input.KeyHold(Keys.NumPad1))
            {
                items.Add(new Usable(UsableType.HealthPot, itemSheet) { Vacuumable = true, Position = new Vector2(Rng.Noxt(Globals.ScreenWidth), Rng.Noxt(Globals.ScreenHeight)) });
            }
            if (Input.KeyHold(Keys.NumPad2))
            {
                items.Add(new Usable(UsableType.ManaPot, itemSheet) { Vacuumable = true, Position = new Vector2(Rng.Noxt(Globals.ScreenWidth), Rng.Noxt(Globals.ScreenHeight)) });

            }
            items.RemoveAll(i => !i.Exist);
            foreach (var i in items)
            {
                i.Update(gameTime);
                if (player.LootRadius.Intersects(i.Rectangle) && !player.inventory.IsFull)
                    i.IsBeingLooted = true;
                else if (player.inventory.IsFull) i.IsBeingLooted = false;
                if (i.IsBeingLooted)
                    i.VacuumLoot(gameTime, i, player.Position, player.inventory);
                else { if (i.Speed >= 0f) i.Speed -= 400f * (float)gameTime.ElapsedGameTime.TotalSeconds; if (i.Speed < 0) i.Speed = 0f; }
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (Input.KeyClick(Keys.Escape))
                ExitScreen();

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, new Color(35, 35, 35), 0, 0);
            SpriteBatch sb = ScreenManager.SpriteBatch;

            //////////////////////////////////////////////////////
            ////////////////////DRAW WORLD////////////////////////
            //////////////////////////////////////////////////////
            #region DRAW WORLD
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, null, cam.get_transformation(ScreenManager.GraphicsDevice));
            // draw here ---------------------------------------

            foreach (var i in items)
            {
                i.Draw(sb);
            }

            loManager.Draw(sb, gameTime);
            player.Draw(sb, gameTime);

            sb.Draw(ScreenManager.box, Input.mWorldPos(cam, ScreenManager.GraphicsDevice)-new Vector2(16), Color.MonoGameOrange);

            sb.End();
            #endregion
            //////////////////////////////////////////////////////
            ////////////////////DRAW SCREEN///////////////////////
            //////////////////////////////////////////////////////
            #region DRAW SCREEN
            sb.Begin();

            player.DrawInventory(sb);
            popManager.Draw(sb);


            sb.DrawString(ScreenManager.DebugFont, "" + player.swingBox, new Vector2(0, 60), Color.White);
            sb.DrawString(ScreenManager.DebugFont, "" + player.Swing.Origin, new Vector2(0, 80), Color.White);
            sb.DrawString(ScreenManager.DebugFont, "" + player.Swing.Rotation, new Vector2(0, 100), Color.White);
            Extras.DrawDebug(sb, $"mPos:{Input.mWorldPos(cam, ScreenManager.GraphicsDevice)} | tilePos:{Helper.FixPos(Input.mWorldPos(cam, ScreenManager.GraphicsDevice), 32)} | tile:{Helper.FixPos(Input.mWorldPos(cam, ScreenManager.GraphicsDevice), 32) / 32}", 10);
            Extras.DrawDebug(sb, $"spawn enemy: {spawnEnemy}  (enter)", 11);
            Extras.DrawDebug(sb, $"tl:{Globals.ScreenTopLeft} tr:{Globals.ScreenTopRight} bl:{Globals.ScreenBotLeft} br:{Globals.ScreenBotRight}", 20);
            Extras.DrawDebug(sb, $"tl:{Globals.ScreenTopLeft} tr:{Globals.ScreenTopRight} bl:{Globals.ScreenBotLeft} br:{Globals.ScreenBotRight}", 21);

            sb.DrawString(ScreenManager.DebugFont, "playing", Vector2.One, Color.Black);
            sb.DrawString(ScreenManager.DebugFont, "playing", Vector2.Zero, Color.White);
            sb.End();
            #endregion




        }
    }
}
