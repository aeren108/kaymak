using Microsoft.Xna.Framework.Graphics;
using static Kaymak.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Kaymak.UI {

    class Button : Component{
        private SpriteFont font;
        private Vector2 textPos;
        private Color textColor;

        public Button() {
            textColor = Color.BurlyWood;
        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");
            normTexture = CM.Load<Texture2D>("button");
            focusTexture = CM.Load<Texture2D>("buttonfocus");

            texture = normTexture;

            base.LoadContent();
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(texture, Position);
            batch.DrawString(font, Text, textPos, textColor);

            base.Render(batch);
        }

        public override void Update(GameTime gameTime) {
            textPos = new Vector2(Position.X + texture.Width / 2 - font.MeasureString(Text).X / 2, Position.Y + texture.Height / 2 - font.MeasureString(Text).Y / 2);

            if (IsHovering || IsFocused) {
                textColor = Color.White;
                texture = focusTexture;
            } else {
                textColor = Color.BurlyWood;
                texture = normTexture;
            }

            base.Update(gameTime);
        }
    }
}
