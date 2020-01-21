using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Kaymak.Anim;
using static Kaymak.Main;

namespace Kaymak.Entities {
    class Player : Entity {
        private Vector2 Velocity;
        private Vector2 Direction;
        private float velPerSecond = 180f;

        private Animation RightWalk;
        private Animation LeftWalk;
        private Animation IdleLeft;
        private Animation IdleRight;

        private Animation CurAnim;

        private SoundEffectInstance FootStep;
        private SoundEffectInstance Knockback;

        private bool IsKnockbacked = false;
        private Vector2 KnockbackSpeed;
        private double knockbackTimer;
        private float KnockbackResistance = 2f; //out of 10
        private float knockbackDuration;

        public object REctangle { get; private set; }

        public Player(World world) : base(world, EntityType.PLAYER) {
            this.world = world;

            Position = new Vector2(130, 150);
            Velocity = new Vector2(0, 0);
            Direction = new Vector2(0, 0);
            KnockbackSpeed = new Vector2(0, 0);

            boundBox.Width = 32; boundBox.Height = 32;
        }

        public override void LoadContent() {
            sprite = Main.CM.Load<Texture2D>("cat_fighter");
            FootStep = Main.CM.Load<SoundEffect>("footsteps").CreateInstance();
            Knockback = Main.CM.Load<SoundEffect>("knockback").CreateInstance();

            RightWalk = new Animation(70, 8, 64, 64, 2);
            LeftWalk = new Animation(70, 8, 64, 64, 3);
            IdleLeft = new Animation(120, 4, 64, 64, 1);
            IdleRight = new Animation(120, 4, 64, 64, 0);

            CurAnim = IdleRight;
            FootStep.Volume = .1f;
            FootStep.Pitch = .05f;

            Knockback.Volume = 0.3f;
            Knockback.Pitch = -0.2f;
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

            boundBox.X = (int) Position.X + 24;
            boundBox.Y = (int) Position.Y + 38;

            Velocity = Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (IsKnockbacked) {
                knockbackTimer += gameTime.ElapsedGameTime.TotalSeconds;
                Velocity *= 0.3f;
                Velocity += KnockbackSpeed;

                Direction.X = Velocity.X; Direction.Y = Velocity.Y;
                Direction.Normalize();

                if (knockbackTimer >= 0.2f) {
                    IsKnockbacked = false;
                    knockbackTimer = 0;
                }
            }

            HandleTileCollision();
            HandleEntityCollision();
            HandleSoundEffects();
            SetAnimations();
            
            Position += Velocity;
            CurAnim.Update(gameTime);
        }

        private void HandleTileCollision() {
            bool blockedX = false;
            bool blockedY = false;

            if (Direction.X > 0) {
                blockedX = world.Map.IsBlocked((int) (Position + Velocity).X + 38, (int) (Position).Y + 38);
            } else if (Direction.X < 0) {
                blockedX = world.Map.IsBlocked((int) (Position + Velocity).X + 24, (int) (Position).Y + 38);
            } if (Direction.Y > 0) {
                blockedY = world.Map.IsBlocked((int) (Position).X + 24, (int) (Position + Velocity).Y + 50);
            } else if (Direction.Y < 0) {
                blockedY = world.Map.IsBlocked((int) (Position).X + 24, (int) (Position + Velocity).Y + 38);
            }

            if (blockedX) Velocity.X = 0;
            if (blockedY) Velocity.Y = 0;
            
            if ((blockedX || blockedY) && IsKnockbacked) {
                //player hit somewere while on knockback so it will harm player

                knockbackTimer *= 1.05f;
                //health -= KnockbackSpeeed.Length * 10
            }
        }

        private void HandleEntityCollision() {
            for (int i = 0; i < world.entities.Count; i++) {
                Entity e = world.entities[i];

                switch(e.entityType) {
                    case EntityType.FIREBALL:
                        if (boundBox.Intersects(e.boundBox)) {
                            (e as Fireball).isHit = true;
                            ApplyKnockback((e as Fireball).Direction, (e as Fireball).knockbackPower);
                            
                            //health -= e.damage;
                        }
                        break;

                    case EntityType.PLAYER:
                        break;

                    case EntityType.LASER:
                        break;
                }
            }
        }

        private void ApplyKnockback(Vector2 direction, float speed) {
            if (IsKnockbacked)
                knockbackTimer = 0;

            IsKnockbacked = true;
            KnockbackSpeed = speed * direction;
        } 

        private void HandleSoundEffects() {
            if (Velocity.X != 0 || Velocity.Y != 0) {
                if (!IsKnockbacked) {
                    Knockback.Stop();

                    if (FootStep.State != SoundState.Playing)
                        FootStep.Play();

                } else {
                    if (Knockback.State != SoundState.Playing)
                        Knockback.Play();
                }
            } else {
                FootStep.Stop();
            }
        }

        private void SetAnimations() {
            if (!IsKnockbacked) {
                if (Direction.X > 0) {
                    CurAnim = RightWalk;
                } else if (Direction.X < 0) {
                    CurAnim = LeftWalk;
                } else {
                    if (Direction.Y == 0) {
                        if (CurAnim == LeftWalk)
                            CurAnim = IdleLeft;
                        else if (CurAnim == RightWalk)
                            CurAnim = IdleRight;
                    }
                }
                if (Direction.Y > 0 || Direction.Y < 0) {
                    if (CurAnim == IdleLeft)
                        CurAnim = LeftWalk;
                    else if (CurAnim == IdleRight)
                        CurAnim = RightWalk;
                }
            } else {
                if (CurAnim == LeftWalk)
                    CurAnim = IdleLeft;
                else if (CurAnim == RightWalk)
                    CurAnim = RightWalk;
            }
        }
    }
}
