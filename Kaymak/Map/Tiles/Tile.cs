using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Kaymak.Map.Tiles {
    public class Tile {
        public static Texture2D tileSheet;
        protected int id;
        protected int x, y;

        public Rectangle sourceRectangle;

        public static Tile[] tiles = new Tile[1024];

        public Tile(int id, Texture2D tileSheet) {
            this.id = id;
            Tile.tileSheet = tileSheet;

            x = id % 8;
            y = id / 8;

            sourceRectangle = new Rectangle(x * 32, y * 32, 32, 32);
        }

        public virtual void Render(SpriteBatch batch, int x, int y) {
            batch.Draw(tileSheet, new Vector2(x * 32, y * 32), sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
        
        public static Tile GetTile(int id) {
            if (tiles[id] != null)
                return tiles[id];

            tiles[id] = new Tile(id, Tile.tileSheet);
            return tiles[id];
        }

        public virtual void Update(GameTime gameTime) {

        }
    }
}
