using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kaymak.Entities;
using kaymak.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kaymak {
    class World : GameObject {
        public TiledMap Map;
        public Camera Camera;

        private Player player;

        private GraphicsDevice graphics;

        public World(GraphicsDevice graphicsDevice) {
            this.graphics = graphicsDevice;
        }

        public void LoadContent(ContentManager content) {
            Map = new TiledMap(this, "D:\\workspaces\\csharp_workspace\\kaymak\\kaymak\\Content\\map_demo.json");
            Camera = new Camera(graphics);
            player = new Player(this);

            Map.LoadContent(content);
            player.LoadContent(content);
        }

        public void Render(SpriteBatch batch) {
            
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            Map.Render(batch);
            player.Render(batch);

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
