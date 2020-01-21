using System;

using Kaymak.Anim;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Entities {
    class Fireball : Entity {
        private Vector2 Velocity;
        public Vector2 Direction;

        private float velPerSecond = 240;
        public float damage = 50;
        public bool isHit;
        public float knockbackPower = 5f;

        private Animation anim;
        private Random random = new Random();

        public Fireball(World world) : base(world, EntityType.FIREBALL) {
            Velocity = new Vector2(0, 0);
            Direction = new Vector2(0, 1);
            Position.X = random.Next(150, 1000);
            Position.Y = 0;

            boundBox.Width = 20; boundBox.Height = 14;
            velPerSecond = random.Next(200, 360);
        }

        public override void LoadContent() {
            sprite = CM.Load<Texture2D>("fireball");
            anim = new Animation(100, 4, 32, 32, 0);
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, anim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime) {
            boundBox.X = (int) Position.X + 8;
            boundBox.Y = (int) Position.Y + 12;

            if (Position.Y >= world.Map.Height * 32)
                isHit = true;

            anim.Update(gameTime);

            Velocity = Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity;
        }
    }
}
