using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaymak.Assets {
    class SpriteSheet {
        private Texture2D Sprite;
        private Rectangle Bounds;

        private int SpriteWidth, SpriteHeight;

        public SpriteSheet(Texture2D sprite, Rectangle bounds) {
            this.Sprite = sprite;
            this.Bounds = bounds;
        }

        public void Render(SpriteBatch batch, int x, int y) {

        }
    }
}
