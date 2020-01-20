using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaymak.Anim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Entities {
    class Fireball : Entity {
        private Vector2 Velocity;
        private Vector2 Direction;
        private float velPerSecond;

        private Animation anim;

        private bool isHit;
        public Fireball(Vector2 dir) : base(null, EntityType.FIREBALL) {
            Velocity = new Vector2(0, 0);
            Direction = dir;
        }

        public override void LoadContent(ContentManager content) {
            //TODO: prepare sprites
        }

        public override void Render(SpriteBatch batch) {
            //TODO: implement rendering with animation
        }

        public override void Update(GameTime gameTime) {

            Velocity *= Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity;
        }

        private void HandleCollision() {

        }
    }
}
