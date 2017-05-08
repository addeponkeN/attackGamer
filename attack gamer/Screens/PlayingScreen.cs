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
    public class PlayingScreen : GameScreen
    {
        ContentManager content;
        Camera cam;
        Player player;
        Map map;
        public static PopupManager popManager;

        public static GridSheet playerSheet, swingSheet, skeleSheet, goblinSheet, itemSheet, tileSheet;
        LivingObjectManager loManager;
        public List<Item> items = new List<Item>();

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
            swingSheet = content.Load<GridSheet>("SpriteSheets/swingNew");
            goblinSheet = content.Load<GridSheet>("SpriteSheets/goblinSheet");
            skeleSheet = content.Load<GridSheet>("SpriteSheets/skeleSheet");
            itemSheet = content.Load<GridSheet>("SpriteSheets/itemSheet");
            tileSheet = content.Load<GridSheet>("SpriteSheets/tileSheet");

            cam = new Camera();
            map = new Map();
            map.LoadMap(LoadType.Fill, 100, 100);

            player = new Player(playerSheet, swingSheet, ScreenManager.GraphicsDevice, cam) { Position = new Vector2((map.Width * 32) / 2, (map.Height * 32) / 2) };
            loManager = new LivingObjectManager();
            popManager = new PopupManager();
            //loManager.AddEnemy(new Enemy(goblinSheet, ScreenManager.GraphicsDevice) { Position = new Vector2(400) });

            items.Add(new Usable(UsableType.HealthPot, itemSheet) { Vacuumable = true, Position = new Vector2(450) });
            items.Add(new Usable(UsableType.ManaPot, itemSheet) { Vacuumable = true, Position = new Vector2(200) });

            cam.Position = player.CenterBox;
            cam.Zoom = 2.0f;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            player.Update(gameTime, this);
            loManager.Update(gameTime, this);
            popManager.Update(gameTime);
            cam.Position = player.CenterBox;
            Globals.ScreenPosition = new Vector2(cam.Position.X - (Globals.ScreenWidth / 2), cam.Position.Y - (Globals.ScreenHeight / 2));

            if (!player.Exist)
                ExitScreen();
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.s > Input.sO)
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

            if (Input.KeyClick(Keys.Space))
                items.Clear();

            if (Input.KeyClick(Keys.Enter))
                if (spawnEnemy)
                    spawnEnemy = false;
                else spawnEnemy = true;

            if (spawnEnemy)
            {
                delay += delta;
                if (delay > 3)
                {
                    loManager.AddEnemy(new Enemy(goblinSheet, ScreenManager.GraphicsDevice, player) { Position = new Vector2(player.Position.X + Rng.Noxt(-Globals.ScreenWidth / 2, Globals.ScreenWidth / 2), player.Position.Y + Rng.Noxt(-Globals.ScreenHeight / 2, Globals.ScreenHeight / 2)) });

                    delay -= 1;
                }
            }

            if (Input.KeyHold(Keys.NumPad1))
            {
                items.Add(new Usable(UsableType.HealthPot, itemSheet) { Position = new Vector2(player.Position.X + Rng.Noxt(-Globals.ScreenWidth / 2, Globals.ScreenWidth / 2), player.Position.Y + Rng.Noxt(-Globals.ScreenHeight / 2, Globals.ScreenHeight / 2)) });
            }
            if (Input.KeyHold(Keys.NumPad2))
            {
                items.Add(new Usable(UsableType.ManaPot, itemSheet) { Position = new Vector2(player.Position.X + Rng.Noxt(-Globals.ScreenWidth / 2, Globals.ScreenWidth / 2), player.Position.Y + Rng.Noxt(-Globals.ScreenHeight / 2, Globals.ScreenHeight / 2)) });
            }
            items.RemoveAll(i => !i.Exist);
            foreach (var i in items)
            {
                i.Update(gameTime);
                if (player.LootRadius.Intersects(i.Rectangle) && !player.inventory.IsFull && i.IsLootable)
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
            //sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, cam.get_transformation(ScreenManager.GraphicsDevice));
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, null, cam.get_transformation(ScreenManager.GraphicsDevice));
            // draw here ---------------------------------------
            map.Draw(sb, gameTime);
            foreach (var i in items)
                i.Draw(sb);
            loManager.Draw(sb, gameTime);
            player.Draw(sb, gameTime);
            
            sb.Draw(ScreenManager.box, new Rectangle(Helper.ToPoint(Input.mWorldPos(cam, ScreenManager.GraphicsDevice) - new Vector2(2)), new Point(4)), Color.MonoGameOrange);

            sb.End();
            #endregion
            //////////////////////////////////////////////////////
            ////////////////////DRAW SCREEN///////////////////////
            //////////////////////////////////////////////////////
            #region DRAW SCREEN
            sb.Begin();

            popManager.Draw(sb);
            player.DrawInventory(sb);
            Extras.DrawDebug(sb, "playing", 0);

            Extras.DrawDebug(sb, $"hp:{player.Health}/{player.MaxHealth}  mana:{player.Mana}/{player.MaxMana}  xp:{player.Exp}/{player.MaxExp}  lv:{player.Level}", 3);
            Extras.DrawDebug(sb, $"mPos:{new Vector2((int)Input.mWorldPos(cam, ScreenManager.GraphicsDevice).X, (int)Input.mWorldPos(cam, ScreenManager.GraphicsDevice).Y) } | tilePos:{Helper.FixPos(Input.mWorldPos(cam, ScreenManager.GraphicsDevice), 32)} | tile:{Helper.FixPos(Input.mWorldPos(cam, ScreenManager.GraphicsDevice), 32) / 32}", 5);
            Extras.DrawDebug(sb, $"spawn enemy: {spawnEnemy}  (enter)", 6);


            sb.End();
            #endregion




        }
    }
}
