using System;

using Kaymak.Anim;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Entities {
    class Fireball : Entity {
        private Vector2 Velocity;
        public Vector2 Direction;

        private FireballDirection dir;
        bool isHorizontal;

        private float velPerSecond = 240;
        public float damage = 50;
        public bool isHit;
        public float knockbackPower = 5f;

        private int xOffset;
        private int yOffset;

        private Animation anim;
        private Random random = new Random();

        public Fireball(World world, FireballDirection direction) : base(world, EntityType.FIREBALL) {
            this.dir = direction;
            isHorizontal = (direction == FireballDirection.HORIZONTAL);
            Velocity = new Vector2(0, 0);
            Direction = isHorizontal ? new Vector2(1, 0) : new Vector2(0, 1);
            Position = isHorizontal ? new Vector2(0, random.Next(100, 1500)) : new Vector2(random.Next(150, 2400), 0);

            velPerSecond = random.Next(200, 360);

            boundBox.Width = isHorizontal ? 12 : 20;
            boundBox.Height = isHorizontal ? 18 : 14;
            xOffset = isHorizontal ? 12 : 8;
            yOffset = isHorizontal ? 8 : 12;

        }

        public override void LoadContent() {
            sprite = CM.Load<Texture2D>("fireball");
            anim = new Animation(100, 4, 32, 32, (int) dir);
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, anim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime) {
            boundBox.X = (int) Position.X + xOffset;
            boundBox.Y = (int) Position.Y + yOffset;

            if (Position.Y >= world.Map.Height * 32)
                isHit = true;

            anim.Update(gameTime);

            Velocity = Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity;
        }
    }

    enum FireballDirection {
        VERTICAL, HORIZONTAL
    }
}
