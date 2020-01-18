using System;

using Kaymak.Entities;
using Kaymak.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Kaymak {
    class World : GameObject {
        public TiledMap Map;
        public Camera Camera;

        private Player player;

        private GraphicsDevice graphics;
        private Song gameTheme;

        public World(GraphicsDevice graphicsDevice) {
            this.graphics = graphicsDevice;
        }

        public void LoadContent(ContentManager content) {
            Map = new TiledMap(this, content.RootDirectory + "/dungeon.json");
            Camera = new Camera(graphics);
            player = new Player(this);

            Map.LoadContent(content);
            player.LoadContent(content);
            gameTheme = content.Load<Song>("gametheme");
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(gameTheme);
        }

        public void Render(SpriteBatch batch) {
            
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.Transform);

            Map.Layers[0].Render(batch);
            Map.Layers[2].Render(batch);
            player.Render(batch);
            Map.Layers[1].Render(batch);

            batch.End();
        }

        public void Update(GameTime gameTime) {
            Camera.Pos = player.Position;
            //Console.WriteLine(player.Position.ToString());

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

            player.Update(gameTime);
            Camera.Update();
        }
    }
}
