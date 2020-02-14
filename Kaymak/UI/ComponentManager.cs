using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kaymak.UI {
    class ComponentManager : GameObject {
        private List<Component> components;

        public ComponentManager() {
            components = new List<Component>();
        }

        public void LoadContent() {
            
        }

        public void Render(SpriteBatch batch) {
            for (int i = 0; i < components.Count; i++) {
                components[i].Render(batch);
            }
        }

        public void Update(GameTime gameTime) {
            MouseState mState = Mouse.GetState();

            for (int i = 0; i < components.Count; i++) {
                components[i].Update(gameTime);
            }

            bool isClickedBlank = false;

            if (mState.LeftButton == ButtonState.Pressed && IsOnBlank()) 
                isClickedBlank = true;
            

            if (isClickedBlank) {
                foreach (Component c in components) {
                    c.IsFocused = false;
                }
            }
        }

        private bool IsOnBlank() {
            foreach (Component c in components) {
                if (c.IsHovering) {
                    return false;
                }
            }

            return true;
        }

        public void AddComponent(Component c) {
            components.Add(c);
            c.LoadContent();
            c.Click += HandleFocus;
        }

        public void HandleFocus(object sender, EventArgs args) {
            Component c = sender as Component;

            foreach (Component ct in components) {
                ct.IsFocused = false;
            }

            c.IsFocused = true;
            Console.WriteLine(c.IsFocused);
        }

        public void ClearFocuses() {
            foreach (Component c in components) {
                c.IsFocused = false;
            }
        }
    }
}
