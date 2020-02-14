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

namespace Kaymak.Screens.Menu {
    class MainMenuScreen : Screen {
        private SpriteFont font;
        private Texture2D button;

        private ComponentManager compManager;
        private List<Component> components; //TODO: add components to this list and handle via this list
        private Button singleplayer;
        private Button multiplayer;
        private Button quit;

        public MainMenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            compManager = new ComponentManager();
            components = new List<Component>();
        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");
            button = CM.Load<Texture2D>("button");

            singleplayer = new Button();
            multiplayer = new Button();
            quit = new Button();

            singleplayer.Text = "Singleplayer";
            multiplayer.Text = "Multiplayer";
            quit.Text = "Quit";

            singleplayer.Click += ButtonClick;
            multiplayer.Click += ButtonClick;
            quit.Click += ButtonClick;

            singleplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 64);
            multiplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 16);
            quit.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 + 32);

            compManager.AddComponent(singleplayer);
            compManager.AddComponent(multiplayer);
            compManager.AddComponent(quit);
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

        private void ButtonClick(object sender, EventArgs e) {
            if (sender == singleplayer) {
                isActive = false;
                screenManager.AddScreen(new SingleplayerScreen(screenManager, graphicsDevice));
            } else if (sender == multiplayer) {
                screenManager.AddScreen(new MultiplayerMenuScreen(screenManager, graphicsDevice));
            } else if (sender == quit) {
                ExitGame = true;
            }
        }
    }
}
