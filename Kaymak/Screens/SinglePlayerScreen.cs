using Kaymak.Entities;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Screens {
    class SingleplayerScreen : Screen {
        private World world;

        private string dash = "";
        private double fps;

        private SpriteFont font;

        public SingleplayerScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            world = new World(graphicsDevice);
        }

        public override void LoadContent() {
            world.LoadContent();
            font = CM.Load<SpriteFont>("font");
        }

        public override void Render(SpriteBatch spriteBatch) {
            graphicsDevice.Clear(Color.TransparentBlack);

            world.Render(spriteBatch);
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Volume: " + (MediaPlayer.Volume * 100).ToString("0.0"), new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(font, "FPS: " + fps.ToString("0.000"), Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, dash, new Vector2(0, 40), Color.White);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime) {
            Player p = world.player as Player;

            fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            dash = p.dashReady ? "Dash: Ready" : "Dash: " + (p.dashCooldown - p.dashCooldownTimer).ToString("0.00");

            world.Update(gameTime);
        }
    }
}
