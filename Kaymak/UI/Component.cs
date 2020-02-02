using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kaymak.UI {
    class Component : GameObject {
        protected Texture2D texture;
        protected bool isHovering;

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public event EventHandler Click;
        public Rectangle Rectangle {
            get {
                return new Rectangle((int) Position.X, (int) Position.Y, texture.Width, texture.Height);
            }
        }

        public Component(Texture2D texture) {
            this.texture = texture;
        }

        public virtual void LoadContent() {

        }

        public virtual void Render(SpriteBatch batch) {

        }

        public virtual void Update(GameTime gameTime) {
            MouseState mState = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mState.X, mState.Y, 1, 1);

            if (mouseRect.Intersects(Rectangle)) {
                isHovering = true;

                if (mState.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            } else
                isHovering = false;
        }
    }
}
