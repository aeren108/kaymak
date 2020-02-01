using Kaymak.Entities;
using Kaymak.Screens;
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
        ScreenManager screenManager;

        Texture2D cursor;
        Vector2 cursorPos;

        public static ContentManager CM;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            //IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = 720;// GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.ApplyChanges();
            CM = Content;
        }

        protected override void Initialize() {
            
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenManager = new ScreenManager();

            screenManager.AddScreen(new MenuScreen(screenManager, GraphicsDevice));
            
            cursor = Content.Load<Texture2D>("cursor");

            base.LoadContent();
        }

        protected override void UnloadContent() {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mState = Mouse.GetState();
            cursorPos = new Vector2(mState.X, mState.Y);

            screenManager.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            screenManager.Render(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(cursor, cursorPos);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
