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

        public Button(Texture2D texture) : base(texture) {
            textColor = Color.BurlyWood;
        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");

            base.LoadContent();
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(texture, Position);
            batch.DrawString(font, Text, textPos, textColor);

            base.Render(batch);
        }

        public override void Update(GameTime gameTime) {
            textPos = new Vector2(Position.X + texture.Width / 2 - font.MeasureString(Text).X / 2, Position.Y + texture.Height / 2 - font.MeasureString(Text).Y / 2);

            if (isHovering)
                textColor = Color.White;
            else
                textColor = Color.BurlyWood;

            base.Update(gameTime);
        }
    }
}
