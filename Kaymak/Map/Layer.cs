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
        public List<Rectangle> blocks;
        private TiledMap map;

        public Layer(TiledMap map, int[] tiles, bool isVisible) {
            this.map = map;
            this.tiles = tiles;
            this.isVisible = isVisible;

            blocks = new List<Rectangle>();
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
                    int index = i + j * 64;
                    int id = tiles[index]-1;

                    if (id == -1) continue;

                    int x = id % map.SheetWidth;
                    int y = id / map.SheetHeight;

                    batch.Draw(map.SpriteSheet, new Vector2(i * 32, j * 32), new Rectangle(x*32, y*32, 32, 32), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
            }
        }

        public void Update(GameTime gameTime) {

        }
    }
}
