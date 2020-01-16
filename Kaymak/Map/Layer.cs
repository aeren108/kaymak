using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                    int x = id % map.SheetWidth * 32;
                    int y = id / map.SheetHeight * 32;

                    tileRect.X = i * 32; tileRect.Y = j * 32;
                    camera.WorldPosition(ref origin);

                    cameraBound.X = (int) origin.X;
                    cameraBound.Y = (int) origin.Y;


                    if (tileRect.Intersects(cameraBound))
                        batch.Draw(map.SpriteSheet, new Vector2(i * 32, j * 32), new Rectangle(x, y, 32, 32), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        public void Update(GameTime gameTime) {

        }
    }
}
