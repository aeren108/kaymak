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
        private Button quit;

        public MenuScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            components = new List<Component>();
        }

        public override void LoadContent() {
            font = CM.Load<SpriteFont>("font");
            button = CM.Load<Texture2D>("button");

            singleplayer = new Button(button);
            multiplayer = new Button(button);
            quit = new Button(button);

            singleplayer.Text = "Singleplayer";
            multiplayer.Text = "Multiplayer";
            quit.Text = "Quit";

            singleplayer.Click += ButtonClick;
            multiplayer.Click += ButtonClick;
            quit.Click += ButtonClick;

            singleplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 64);
            multiplayer.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 - 16);
            quit.Position = new Vector2(graphicsDevice.Viewport.Width / 2 - 16 * 5, graphicsDevice.Viewport.Height / 2 + 32);

            components.Add(singleplayer);
            components.Add(multiplayer);
            components.Add(quit);

            foreach (var c in components)
                c.LoadContent();
        }

        public override void Render(SpriteBatch batch) {
            graphicsDevice.Clear(Color.FloralWhite);

            batch.Begin();

            for (int i = 0; i < components.Count; i++) 
                components[i].Render(batch);

            batch.End();
        }

        public override void Update(GameTime gameTime) {
            for (int i = 0; i < components.Count; i++)
                components[i].Update(gameTime);
        }

        private void ButtonClick(object sender, EventArgs e) {
            if (sender == singleplayer) {
                isActive = false;
                screenManager.AddScreen(new SingleplayerScreen(screenManager, graphicsDevice));
            } else if (sender == multiplayer) {
                //TODO: Prepare multiplayer screen
            } else if (sender == quit) {
                ExitGame = true;
            }
        }
    }
}
