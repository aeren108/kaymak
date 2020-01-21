using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Map.Tiles {
    public class DestructibleTile : AnimatedTile {

        public bool isDestructed = false;
        private bool hasAnimPlayed = false;
        private bool canBeRemoved = false;

        public DestructibleTile(Vector2 pos, int[] ids, Texture2D tileSheet, float delay) : base(pos, ids, delay) {

        }

        public override void Update(GameTime gameTime) {
            if (!isDestructed) return;

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer >= delay) {
                timer = 0;

                if (hasAnimPlayed)
                    canBeRemoved = true;

                if (curTile < tiles.Length - 1) { 
                    curTile++;
                } else {
                    curTile = 0;
                    hasAnimPlayed = true;
                }
            }
        }
    }
}
