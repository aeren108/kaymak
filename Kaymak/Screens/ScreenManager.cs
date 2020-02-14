using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Screens {

    class ScreenManager : GameObject {
        private List<Screen> Screens;

        public ScreenManager() {
            Screens = new List<Screen>();
        }

        public void AddScreen(Screen screen) {
            Screens.Add(screen);
            screen.LoadContent();
        }

        public void LoadContent() {
            
        }

        public void Render(SpriteBatch batch) {
            Screens[Screens.Count - 1].Render(batch);

            for (int i = Screens.Count - 1; i > 0; i--) {
                if (Screens[i].isPopup)
                    Screens[i].Render(batch);
            }  
        }

        public void Update(GameTime gameTime) {
            Screens[Screens.Count - 1].Update(gameTime);

            for (int i = Screens.Count - 1; i > 0; i--) {
                if (Screens[i].isPopup)
                    Screens[i].Update(gameTime);
            }
        }

        public void RemoveCurrent() {
            Screens.RemoveAt(Screens.Count - 1);
        }
    }
}
