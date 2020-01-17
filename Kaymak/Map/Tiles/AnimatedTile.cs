using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaymak.Map.Tiles {
    class AnimatedTile : Tile {
        private Tile[] tiles;
        private double delay;
        private double timer;
        private int curTile;

        public AnimatedTile(int[] ids, Texture2D tileSheet, float delay) : base(ids[0], tileSheet){
            tiles = new Tile[ids.Length];
            this.delay = delay;
            curTile = 0;

            for (int i = 0; i < ids.Length; i++) {
                tiles[i] = new Tile(ids[i], tileSheet);
            }
        }

        public override void Render(SpriteBatch batch, int x, int y) {
            tiles[curTile].Render(batch, x, y);
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
