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
using Kaymak.UI;

namespace Kaymak.Screens {
    class MenuScreen : Screen {
        private SpriteFont font;
        private Texture2D button;

        private List<Component> components; //TODO: add components to this list and handle via this list
        private Button singleplayer;
        private Button multiplayer;

        public MenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            components = new List<Component>();
        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");
            button = CM.Load<Texture2D>("button");

            singleplayer = new Button(button);
            multiplayer = new Button(button);

            singleplayer.Text = "Singleplayer";
            multiplayer.Text = "Multiplayer";

            singleplayer.Click += SingleplayerClick;
            multiplayer.Click += MultiplayerClick;

            singleplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 64);
            multiplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 16);

            singleplayer.LoadContent();
            multiplayer.LoadContent();
        }

        public override void Render(SpriteBatch batch) {
            graphicsDevice.Clear(Color.FloralWhite);

            batch.Begin();

            singleplayer.Render(batch);
            multiplayer.Render(batch);

            batch.End();
        }

        public override void Update(GameTime gameTime) {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.S)) {
                isActive = false;
                screenManager.AddScreen(new SingleplayerScreen(screenManager, graphicsDevice));
            } else if (keyState.IsKeyDown(Keys.M)) {
                isActive = false;
                screenManager.AddScreen(new MultiplayerScreen(screenManager, graphicsDevice));
            }

            singleplayer.Update(gameTime);
            multiplayer.Update(gameTime);
        }

        private void SingleplayerClick(object sender, EventArgs e) {
            isActive = false;
            screenManager.AddScreen(new SingleplayerScreen(screenManager, graphicsDevice));
        }

        private void MultiplayerClick(object sender, EventArgs e) {
            isActive = false;
            screenManager.AddScreen(new MultiplayerScreen(screenManager, graphicsDevice));
        }
    }
}
