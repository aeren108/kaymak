using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static Kaymak.Main;

namespace Kaymak.UI {
    class Component : GameObject {
        protected Texture2D texture;
        protected Texture2D focusTexture;
        protected Texture2D normTexture;

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public event EventHandler Click;
        public bool IsHovering { get; set; }
        public bool IsFocused { get; set; }
        public Rectangle Rectangle {
            get {
                return new Rectangle((int) Position.X, (int) Position.Y, texture.Width, texture.Height);
            }
        }

        public Component() {
            texture = normTexture;
        }

        public virtual void LoadContent() {

        }

        public virtual void Render(SpriteBatch batch) {

        }

        public virtual void Update(GameTime gameTime) {
            MouseState mState = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mState.X, mState.Y, 1, 1);

            if (mouseRect.Intersects(Rectangle)) {
                IsHovering = true;

                if (mState.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            } else
                IsHovering = false;
        }
    }
}
