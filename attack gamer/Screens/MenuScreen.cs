using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attack_gamer
{
    class MenuScreen : GameScreen
    {
        ContentManager content;
        Texture2D box;
        Button btnStart;

        public MenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(2);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            box = content.Load<Texture2D>("box");

            btnStart = new Button(ScreenManager.Font, (Helper.Center(Globals.ScreenCenter, new Vector2(128, 32))), new Vector2(128, 32), "start", box);
        }

        public override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (btnStart.IsReleased)
            {
                //LoadingScreen.Load(ScreenManager, false, null, new PlayingScreen());
                ScreenManager.AddScreen(new PlayingScreen(), null);
                ExitScreen();
            }
            if (Input.KeyClick(Keys.Escape))
                ScreenManager.Game.Exit();

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch sb = ScreenManager.SpriteBatch;
            sb.Begin();

            btnStart.Draw(sb);

            sb.End();
        }
    }
}
