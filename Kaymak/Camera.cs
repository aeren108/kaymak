using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak {
    class Camera {
        public float Zoom = 1.0f;
        public float Rotation = 0.0f;
        public Vector2 Pos = Vector2.Zero;
        public Matrix Transform;
        public Rectangle bounds;
        private GraphicsDevice graphics;

        public Camera(GraphicsDevice graphics) {
            this.graphics = graphics;
            bounds = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
        }

        public void Update() {
            Transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2, 0));
        }

        public void WorldPosition(ref Vector2 vec) {
            vec = Vector2.Transform(vec, Matrix.Invert(Transform));
        }
    }
}
