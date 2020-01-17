using System;
using kaymak.Map.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kaymak.Map {
    class Layer : GameObject {
        public bool isVisible;
        public int[] tiles;
        private TiledMap map;
        private Camera camera;

        private Rectangle tileRect = new Rectangle(0, 0, 32, 32);
        private Rectangle cameraBound = new Rectangle(0, 0, 800, 640);
        private Vector2 origin = new Vector2(0, 0);

        public Layer(TiledMap map, int[] tiles, bool isVisible, Camera camera) {
            this.map = map;
            this.tiles = tiles;
            this.isVisible = isVisible;
            this.camera = camera;
        }

        public void LoadContent(ContentManager content) {
            Tile.tileSheet = map.SpriteSheet;
        }

        public int GetId(int x, int y) {
            int xa = x / map.TileSize;
            int ya = y / map.TileSize;

            try {
                return tiles[xa + ya * map.Width];
            } catch (IndexOutOfRangeException) {
                return 0;
            }
        }

        public void Render(SpriteBatch batch) {
            for (int j = 0; j < 64; j++) {
                for (int i = 0; i < 64; i++) {
                    origin = Vector2.Zero;

                    int index = i + j * 64;
                    int id = tiles[index]-1;

                    if (id == -1) continue;

                    tileRect.X = i * 32; tileRect.Y = j * 32;
                    camera.WorldPosition(ref origin);

                    cameraBound.X = (int) origin.X;
                    cameraBound.Y = (int) origin.Y;
                    
                    if (tileRect.Intersects(cameraBound))
                        new Tile(id, map.SpriteSheet).Render(batch, i , j);
                }
            }
        }

        public void Update(GameTime gameTime) {

        }
    }
}
