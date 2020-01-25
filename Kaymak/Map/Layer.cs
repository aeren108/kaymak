using System;
using Kaymak.Map.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Map {
    class Layer : GameObject {
        public bool isVisible;
        public int[] tiles;
        private TiledMap map;
        private Camera camera;

        private Rectangle tileRect = new Rectangle(0, 0, 32, 32);
        private Rectangle cameraBound = new Rectangle(0, 0, 1920, 1080);
        private Vector2 origin = new Vector2(0, 0);

        private Tile[] Tiles;

        public Layer(TiledMap map, int[] tiles, bool isVisible, Camera camera) {
            this.map = map;
            this.tiles = tiles;
            this.isVisible = isVisible;
            this.camera = camera;

            Tiles = new Tile[tiles.Length];
        }

        public void LoadContent() {
            Tile.tileSheet = map.SpriteSheets[map.curTileset];
            InitTiles();
        }

        private void InitTiles() {
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    int index = x + y * map.Width;
                    int id = tiles[index] - 1;

                    Tile tile = new Tile(new Vector2(x, y), id);
                    tile.IsSolid = map.BlockedTiles.Contains(id + 1);

                    Tiles[index] = tile;
                }
            }
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

        public Tile GetTile(int x, int y) {
            int xa = x / map.TileSize;
            int ya = y / map.TileSize;

            try {
                return Tiles[xa + ya * map.Width];
            } catch (IndexOutOfRangeException) {
                return new Tile(Vector2.Zero, 0);
            }
        }

        public bool IsBlocked(int x, int y) {
            return GetTile(x, y).IsSolid;
        }

        public void Render(SpriteBatch batch) {
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    origin = Vector2.Zero;

                    int index = x + y * map.Width;
                    int id = tiles[index]-1;

                    if (id == -1) continue;

                    Tile tile = Tiles[index];

                    tileRect.X = x * map.TileSize; tileRect.Y = y * map.TileSize;
                    camera.WorldPosition(ref origin);

                    cameraBound.X = (int) origin.X;
                    cameraBound.Y = (int) origin.Y;

                    if (tileRect.Intersects(cameraBound))
                        tile.Render(batch);
                }
            }
        }

        public void Update(GameTime gameTime) {
            for (int i = 0; i < map.tileCount; i++) {
                if (Tiles[i] != null)
                    Tiles[i].Update(gameTime);
            }
        }
    }
}
