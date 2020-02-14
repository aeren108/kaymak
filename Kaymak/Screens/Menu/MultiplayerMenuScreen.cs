using Kaymak.Screens;
using Kaymak.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Kaymak.Main;

namespace Kaymak.Screens.Menu {
    class MultiplayerMenuScreen : Screen {
        private SpriteFont font;
        private Texture2D button;

        private ComponentManager compManager;
        private InputBox username;
        private InputBox host;
        private Button connect;
        private Button back;

        public MultiplayerMenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            compManager = new ComponentManager();
        }

        public override void LoadContent() {
            username = new InputBox();
            host = new InputBox();
            connect = new Button();
            back = new Button();

            connect.Text = "Connect";
            back.Text = "Back";

            connect.Click += HandleClicks;
            back.Click += HandleClicks;

            username.Hint = "Username";
            host.Hint = "Host's IP Address";

            username.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 64);
            host.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 16);
            connect.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 + 32);
            back.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 + 96);

            compManager.AddComponent(username);
            compManager.AddComponent(host);
            compManager.AddComponent(connect);
            compManager.AddComponent(back);

            compManager.ClearFocuses();
        }

        public override void Render(SpriteBatch batch) {
            graphicsDevice.Clear(Color.FloralWhite);

            batch.Begin();
            compManager.Render(batch);
            batch.End();
        }

        public override void Update(GameTime gameTime) {
            compManager.Update(gameTime);
        }

        public void HandleClicks(object sender, EventArgs args) {
            if (sender == connect) {
                screenManager.AddScreen(new MultiplayerScreen(screenManager, graphicsDevice));
            } else if (sender == back) {
                screenManager.RemoveCurrent();
            }
        }
    }
}
