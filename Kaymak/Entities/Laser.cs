using Kaymak;
using Kaymak.Entities;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace kaymak.Kaymak.Entities {
    class Laser : Entity {
        private Texture2D Sprite;
        private SoundEffectInstance LaserSound;

        private AudioEmitter emitter;

        public bool IsActive = false;
        private double timer;

        private Rectangle sourceRectangle;
        public LaserDirection direction;

        private Random random = new Random();

        public float KnockbackPower = 3f;
        private int length;
        private bool isHorizontal;
        public bool IsFinished;

        private int xOffset, yOffset;

        public Laser(World world, LaserDirection direction) : base(world, EntityType.LASER) {
            this.direction = direction;
            sourceRectangle = new Rectangle(0, (int) direction * 32, 32, 32);
            isHorizontal = (direction == LaserDirection.HORIZONTAL);

            if (isHorizontal) {
                length = world.Map.Width;
                boundBox = new Rectangle(0, 0, length * 32, 32);
                Position = new Vector2(0, random.Next(0, world.Map.Height) * 32);
                xOffset = 0;
                yOffset = 10;
            } else {
                length = world.Map.Height;
                boundBox = new Rectangle(0, 0, 32, length * 32);
                Position = new Vector2(random.Next(0, world.Map.Width) * 32, 0);
                xOffset = 10;
                yOffset = 0;
            }
        }

        public override void LoadContent() {
            Sprite = CM.Load<Texture2D>("laser");
            LaserSound = CM.Load<SoundEffect>("laser_sound").CreateInstance();

            LaserSound.Volume = 1f;
            LaserSound.Pitch = 0.2f;

            emitter = new AudioEmitter();
          
        }

        public override void Render(SpriteBatch batch) {
            for (int i = 0; i < length; i++) {
                if (isHorizontal)
                    batch.Draw(Sprite, new Vector2(i * 32, Position.Y), sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                else
                    batch.Draw(Sprite, new Vector2(Position.X, i * 32), sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
        }

        public override void Update(GameTime gameTime) {
            int active = IsActive ? 1 : 0;
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            boundBox.X = (int) Position.X + xOffset;
            boundBox.Y = (int) Position.Y + yOffset;

            sourceRectangle.X = active * 32;

            if (!IsActive) {
                if (timer >= 1f) {
                    IsActive = true;
                    timer = 0;
                }
            } else {
                if (timer >= 2.5f) {
                    timer = 0;
                    IsFinished = true;
                }
            }

            emitter.Position = isHorizontal ? new Vector3((world.player as Player).Listener.Position.X, Position.Y + 16, 0) : new Vector3(Position.X + 16, (world.player as Player).Listener.Position.Y, 0);
            emitter.DopplerScale = 2.5f;

            LaserSound.Apply3D((world.player as Player).Listener, emitter);
            
            if (IsActive && LaserSound.State != SoundState.Playing) {
                LaserSound.Play();
            }
        }
    }

    enum LaserDirection {
        HORIZONTAL, VERTICAL
    }

}
