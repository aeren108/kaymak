using Kaymak;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.UI {

    //TODO: Create textures for buttons
    //TODO: Handle click detection
    class Button : GameObject {
        public Vector2 Position;
        public Rectangle Bounds;

        public Button(Rectangle bound) {
            this.Bounds = bound;
        }

        public void LoadContent() {
            
        }

        public void Render(SpriteBatch batch) {
            //batch.
        }

        public void Update(GameTime gameTime) {
            KeyboardState keyState = Keyboard.GetState();
        }
    }
}
