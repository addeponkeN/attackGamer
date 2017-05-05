using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace attack_gamer
{
    class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? controllingPlayer, params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                    if (screen != null)
                        ScreenManager.AddScreen(screen, ControllingPlayer);

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (Globals.ScreenSize - textSize) / 2;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, new Vector2(textPosition.X + 1, textPosition.Y + 1), new Color(35, 35, 35, TransitionAlpha));
                spriteBatch.DrawString(font, message, textPosition, new Color(255, 255, 255, TransitionAlpha));

                spriteBatch.End();
            }
        }
    }
}
