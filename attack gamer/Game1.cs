using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace attack_gamer
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = Globals.ScreenX;
            graphics.PreferredBackBufferHeight = Globals.ScreenY;

            // Create the screen manager component.
            var screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new PlayingScreen(), null);

            // Activate the first screens.
            //screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new MainMenuScreen(), null);

        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(35, 35, 35));

            base.Draw(gameTime);
        }
    }
}
