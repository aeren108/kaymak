using Kaymak.Entities;
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
        private string[] data;
        private double timer = 0;

        public MultiplayerScreen(string[] data, ScreenManager screenManager, GraphicsDevice graphicsDevice) : base(screenManager, graphicsDevice, false) {
            this.data = data;

            world = new World(graphicsDevice);
            world.singleplayer = false;
            client = new Client(world, data[0], data[1], 2020);
        }

        public override void LoadContent() {
            world.LoadContent();
            (world.player as Player).Username = data[0];
        }

        public override void Render(SpriteBatch batch) {
            graphicsDevice.Clear(Color.Maroon);

            world.Render(batch);
        }

        public override void Update(GameTime gameTime) {
            world.Update(gameTime);
            client.Update();
            if (world.player.IsMoving)
                client.SendMove(world.player);

        }
    }
}
