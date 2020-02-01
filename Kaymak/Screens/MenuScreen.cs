using Kaymak.Screens;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Screens {
    class MenuScreen : Screen {
        private SpriteFont font;

        public MenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {

        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");
        }

        public override void Render(SpriteBatch batch) {
            graphicsDevice.Clear(Color.DarkCyan);

            batch.Begin();

            batch.DrawString(font, "Press 'S' for singleplayer \n Press 'M' for multiplayer", new Vector2(graphicsDevice.Viewport.Width / 2 - 100, graphicsDevice.Viewport.Height / 2), Color.Black);

            batch.End();
        }

        public override void Update(GameTime gameTime) {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Enter)) {
                isActive = false;
                screenManager.AddScreen(new SingleplayerScreen(screenManager, graphicsDevice));
            }
        }
    }
}
