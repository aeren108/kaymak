using Kaymak.Entities;
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
        Texture2D cursor;
        Vector2 cursorPos;

        public static ContentManager CM;
        string dash;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            //IsMouseVisible = true;

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
            Player p = world.player as Player;

            if (p.dashReady) {
                dash = "Dash: Ready";
            } else {
                dash = "Dash: " + (p.dashCooldown - p.dashCooldownTimer).ToString("0.00");
            }

            cursorPos = new Vector2(mState.X - 16, mState.Y - 16);

            world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.TransparentBlack);

            world.Render(spriteBatch);
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Volume: " + (MediaPlayer.Volume * 100).ToString("0.0"), new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(font, "FPS: " + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0.000"), Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, dash, new Vector2(0, 40), Color.White);
            spriteBatch.Draw(cursor, cursorPos);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
