using System;
using System.Collections.Generic;
using Kaymak.Entities;
using Kaymak.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Kaymak {
    class World : GameObject {
        public TiledMap Map;
        public Camera Camera;

        private List<Entity> entities;
        private Entity player;

        private GraphicsDevice graphics;
        private Song gameTheme;

        private int prevScroll = 0;

        public World(GraphicsDevice graphicsDevice) {
            this.graphics = graphicsDevice;

            entities = new List<Entity>();
        }

        public void LoadContent(ContentManager content) {
            Map = new TiledMap(this, "/dungeon.json");
            Camera = new Camera(graphics);
            player = new Player(this);

            Map.LoadContent(content);
            player.LoadContent(content);
            entities.Add(player);

            gameTheme = content.Load<Song>("gametheme");
            MediaPlayer.Volume = 0.05f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(gameTheme);
        }

        public void Render(SpriteBatch batch) {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.Transform);

            Map.Render(batch, 0);

            entities.ForEach((Entity e) => e.Render(batch));

            Map.Render(batch, 1);
            Map.Render(batch, 2);

            batch.End();
        }

        public void Update(GameTime gameTime) {
            MouseState state = Mouse.GetState();

            Camera.Pos = player.Position;

            if (Camera.Pos.X < graphics.Viewport.Width / 2) {
                Camera.Pos.X = graphics.Viewport.Width / 2;
            } else if (Camera.Pos.X > Map.Width * Map.TileSize - graphics.Viewport.Width / 2) {
                Camera.Pos.X = Map.Width * Map.TileSize - graphics.Viewport.Width / 2;
            }
            if (Camera.Pos.Y < graphics.Viewport.Height / 2) {
                Camera.Pos.Y = graphics.Viewport.Height / 2;
            } else if (Camera.Pos.Y > Map.Height * Map.TileSize - graphics.Viewport.Height / 2) {
                Camera.Pos.Y = Map.Height * Map.TileSize - graphics.Viewport.Height / 2;
            }

            if (state.ScrollWheelValue < prevScroll)
                MediaPlayer.Volume -= .1f;
            else if (state.ScrollWheelValue > prevScroll)
                MediaPlayer.Volume += .1f;

            prevScroll = state.ScrollWheelValue;

            if (MediaPlayer.Volume < 0) MediaPlayer.Volume = 0;
            else if (MediaPlayer.Volume > 1) MediaPlayer.Volume = 1;

            if (MediaPlayer.Volume == 0 && MediaPlayer.State == MediaState.Playing) 
                MediaPlayer.Pause();
            else if (MediaPlayer.Volume != 0 && MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();

            entities.ForEach((Entity e) => e.Update(gameTime));
            Camera.Update();
        }
    }
}
