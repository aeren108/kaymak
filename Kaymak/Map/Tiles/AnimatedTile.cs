using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Kaymak.Map.Tiles {
    public class AnimatedTile : Tile {
        protected Tile[] tiles;
        protected double delay;
        protected double timer;
        protected int curTile;

        public AnimatedTile(Vector2 pos, int[] ids, float delay) : base(pos, ids[0]) {
            tiles = new Tile[ids.Length];
            this.delay = delay;
            curTile = 0;

            for (int i = 0; i < ids.Length; i++) {
                tiles[i] = new Tile(pos, ids[i]); 
            }
        }

        public override void Render(SpriteBatch batch) {
            tiles[curTile].Render(batch);
        }

        public override void Update(GameTime gameTime) {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer >= delay) {
                timer = 0;

                if (curTile < tiles.Length - 1) 
                    curTile++;
                else
                    curTile = 0;
            }
        }
    }
}
