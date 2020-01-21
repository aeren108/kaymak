using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Kaymak {
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;

        SpriteFont font;

        public static ContentManager CM;
        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.ApplyChanges();
            CM = Content;
        }

        protected override void Initialize() {
            
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            world = new World(graphics.GraphicsDevice);
            world.LoadContent();

            font = Content.Load<SpriteFont>("font");

            base.LoadContent();
        }

        protected override void UnloadContent() {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            world.Render(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Volume: " + (MediaPlayer.Volume * 100).ToString("0.0"), Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "FPS: " + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0.000"), new Vector2(0, 20), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
