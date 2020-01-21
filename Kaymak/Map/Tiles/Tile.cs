using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Kaymak.Map.Tiles {
    public class Tile : GameObject {
        public Vector2 Position;
        public static Texture2D tileSheet;
        protected int id = 0;
        protected int x, y;
        public bool IsSolid;

        public Rectangle sourceRectangle;
        public Rectangle boundBox;

        public Tile(Vector2 pos, int id) {
            this.id = id;
            this.Position = pos * 32;

            x = id % 8;
            y = id / 8;

            sourceRectangle = new Rectangle(x * 32, y * 32, 32, 32);
        }

        public virtual void Render(SpriteBatch batch) {
            batch.Draw(tileSheet, Position, sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f); ;
        }

        public virtual void Update(GameTime gameTime) {
            /* tile logic in here;
             
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= 1900)
                timer = 0;

            */
        }

        public void LoadContent() { }
    }
}
