using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Kaymak.Anim;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Kaymak.Entities {
    class Player : Entity {
        private Vector2 Velocity;
        private Vector2 Direction;

        private Animation RightWalk;
        private Animation LeftWalk;
        private Animation IdleLeft;
        private Animation IdleRight;

        private Animation CurAnim;

        private SoundEffectInstance FootStep;
        private float velPerSecond = 180f;

        public Player(World world) : base(world, EntityType.PLAYER) {
            this.world = world;

            Position = new Vector2(130, 150);
            Velocity = new Vector2(0, 0);
            Direction = new Vector2(0, 0);
        }

        public override void LoadContent(ContentManager content) {
            sprite = content.Load<Texture2D>("cat_fighter");
            FootStep = content.Load<SoundEffect>("footsteps").CreateInstance();

            RightWalk = new Animation(70, 8, 64, 64, 2);
            LeftWalk = new Animation(70, 8, 64, 64, 3);
            IdleLeft = new Animation(120, 4, 64, 64, 1);
            IdleRight = new Animation(120, 4, 64, 64, 0);

            CurAnim = IdleRight;
            FootStep.Volume = .2f;
            FootStep.Pitch = .1f;
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, CurAnim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
                Direction.Y = -1;
            else if (state.IsKeyDown(Keys.S)) 
                Direction.Y = 1;
            else
                Direction.Y = 0;
            if (state.IsKeyDown(Keys.A)) 
                Direction.X = -1;
            else if (state.IsKeyDown(Keys.D))
                Direction.X = 1;
            else
                Direction.X = 0;

            Velocity = Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;

            HandleTileCollision();
            HandleSoundEffects();
            SetAnimations();
            
            Position += Velocity;
            CurAnim.Update(gameTime);
        }

        private void HandleTileCollision() {
            bool blockedX = false;
            bool blockedY = false;

            if (Direction.X == 1) {
                blockedX = world.Map.IsBlocked((int) (Position + Velocity).X + 38, (int) (Position).Y + 38);
            } else if (Direction.X == -1) {
                blockedX = world.Map.IsBlocked((int) (Position + Velocity).X + 24, (int) (Position).Y + 38);
            } if (Direction.Y == 1) {
                blockedY = world.Map.IsBlocked((int) (Position).X + 24, (int) (Position + Velocity).Y + 50);
            } else if (Direction.Y == -1) {
                blockedY = world.Map.IsBlocked((int) (Position).X + 24, (int) (Position + Velocity).Y + 38);
            }

            if (blockedX) Velocity.X = 0;
            if (blockedY) Velocity.Y = 0;
        }

        private void HandleSoundEffects() {
            if (Velocity.X != 0 || Velocity.Y != 0) {
                if (FootStep.State != SoundState.Playing)
                    FootStep.Play();
            } else {
                FootStep.Stop();
            }
        }

        private void SetAnimations() {
            if (Direction.X == 1) {
                CurAnim = RightWalk;
            } else if (Direction.X == -1) {
                CurAnim = LeftWalk;
            } else {
                if (Direction.Y == 0) {
                    if (CurAnim == LeftWalk)
                        CurAnim = IdleLeft;
                    else if (CurAnim == RightWalk)
                        CurAnim = IdleRight;
                }
            } if (Direction.Y == 1 || Direction.Y == -1) {
                if (CurAnim == IdleLeft)
                    CurAnim = LeftWalk;
                else if (CurAnim == IdleRight)
                    CurAnim = RightWalk;
            }
        }
    }
}
