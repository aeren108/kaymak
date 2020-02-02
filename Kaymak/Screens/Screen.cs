using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Screens {
    abstract class Screen : GameObject {
        protected GraphicsDevice graphicsDevice;
        protected ScreenManager screenManager;

        public bool isPopup;
        public bool isActive = true;

        public Screen(ScreenManager screenManager, GraphicsDevice graphicsDevice, bool isPopup) {
            this.graphicsDevice = graphicsDevice;
            this.screenManager = screenManager;
            this.isPopup = isPopup;
        }

        public abstract void LoadContent();

        public abstract void Render(SpriteBatch batch);

        public abstract void Update(GameTime gameTime);

        public virtual void SaveState() {
            //TODO: Handle state saving;
        }
    }
}
