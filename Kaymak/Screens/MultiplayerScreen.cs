using Kaymak.Network;
using Kaymak.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Screens {
    class MultiplayerScreen : Screen {
        private World world;
        private Client client;

        public MultiplayerScreen(ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            world = new World(graphicsDevice);
        }

        public override void LoadContent() {
            
        }

        public override void Render(SpriteBatch batch) {
            
        }

        public override void Update(GameTime gameTime) {
            
        }
    }
}
