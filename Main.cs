using kaymak.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace kaymak {
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TiledMap map;
        Camera camera;

        //TODO: Create a class for player
        Vector2 player;
        Vector2 vel = Vector2.Zero;
        Texture2D plTex;

        float x = 0, y = 0;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            graphics.ApplyChanges();
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera();
            map = new TiledMap("D:\\workspaces\\csharp_workspace\\kaymak\\kaymak\\Content\\map_demo.json");
            plTex = Content.Load<Texture2D>("block_purple");
            map.LoadContent(Content);

            player = new Vector2(0, 0);
        }

        protected override void UnloadContent() {

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();
            MouseState mState = Mouse.GetState();
            camera.Pos = player;

            if (state.IsKeyDown(Keys.W)) {
                vel.Y = -5;
            } else if (state.IsKeyDown(Keys.S)) {
                vel.Y = 5;
            } else {
                vel.Y = 0;
            } if (state.IsKeyDown(Keys.A)) {
                vel.X = -5;
            } else if (state.IsKeyDown(Keys.D)) {
                vel.X = 5;
            } else {
                vel.X = 0;
            }

            if (state.IsKeyDown(Keys.R)) {
                player.X = 0;
                player.Y = 0;
            }

            Rectangle colBoxX = new Rectangle((int)(player + vel).X, (int)player.Y, 32, 32);
            Rectangle colBoxY = new Rectangle((int) player.X, (int) (player + vel).Y, 32, 32);

            foreach (var rect in map.colBlocks) {
                if (colBoxX.Intersects(rect)) {
                    vel.X = 0;
                } 

                if (colBoxY.Intersects(rect)) {
                    vel.Y = 0;
                }
            }

            if (camera.Pos.X < graphics.GraphicsDevice.Viewport.Width / 2) {
                camera.Pos.X = graphics.GraphicsDevice.Viewport.Width / 2;
            } else if (camera.Pos.X > map.Width * map.TileSize - graphics.GraphicsDevice.Viewport.Width / 2) {
                camera.Pos.X = map.Width * map.TileSize - graphics.GraphicsDevice.Viewport.Width / 2;
            } if (camera.Pos.Y < graphics.GraphicsDevice.Viewport.Height / 2) {
                camera.Pos.Y = graphics.GraphicsDevice.Viewport.Height / 2;
            } else if (camera.Pos.Y > map.Height * map.TileSize - graphics.GraphicsDevice.Viewport.Height / 2) {
                camera.Pos.Y = map.Height * map.TileSize - graphics.GraphicsDevice.Viewport.Height / 2;
            }

            if (mState.LeftButton == ButtonState.Pressed) {
                Vector2 mouse = new Vector2(mState.X, mState.Y);
                camera.WorldPosition(ref mouse);

                int tileId = map.GetTile((int)mouse.X, (int)mouse.Y);
                Console.WriteLine(tileId);
            }

            player += vel;
            camera.Update(graphics.GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.AlphaBlend,null,null,null,null,camera.Transform);
            map.Render(spriteBatch);
            spriteBatch.Draw(plTex, player);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
