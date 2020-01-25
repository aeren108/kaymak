using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Kaymak.Anim;
using static Kaymak.Main;
using kaymak.Kaymak.Entities;

namespace Kaymak.Entities {
    class Player : Entity {
        private Vector2 Velocity;
        private Vector2 Direction;
        private float velPerSecond = 150f;

        private Animation RightWalk;
        private Animation LeftWalk;
        private Animation IdleLeft;
        private Animation IdleRight;

        private Animation CurAnim;

        private SoundEffectInstance FootStep;
        private SoundEffectInstance Knockback;
        private SoundEffectInstance Dash;
        public AudioListener Listener;

        private bool IsKnockbacked = false;
        private Vector2 KnockbackVelocity;
        private double knockbackTimer;
        private float KnockbackResistance = 2f; //out of 10
        private float knockbackDuration; //TODO: calculate duration based on knocback power and resistance.

        private bool IsDashing = false;
        private Vector2 DashVelocity;
        private double dashTimer;
        public bool dashReady = true;
        public double dashCooldownTimer;
        public double dashCooldown = 1f;

        private bool CanCollide = true;
        private double collisionTimer;
        private double collisionCooldown = 1d;

        public Player(World world) : base(world, EntityType.PLAYER) {
            this.world = world;

            Position = new Vector2(130, 150);
            Velocity = new Vector2(0, 0);
            Direction = new Vector2(0, 0);
            KnockbackVelocity = new Vector2(0, 0);
            DashVelocity = new Vector2(1, 1);

            boundBox.Width = 32; boundBox.Height = 42;
        }

        public override void LoadContent() {
            sprite = CM.Load<Texture2D>("cat_fighter");
            FootStep = CM.Load<SoundEffect>("footsteps").CreateInstance();
            Knockback = CM.Load<SoundEffect>("knockback").CreateInstance();
            Dash = CM.Load<SoundEffect>("whoosh").CreateInstance();

            RightWalk = new Animation(70, 8, 64, 64, 2);
            LeftWalk = new Animation(70, 8, 64, 64, 3);
            IdleLeft = new Animation(120, 4, 64, 64, 1);
            IdleRight = new Animation(120, 4, 64, 64, 0);

            CurAnim = IdleRight;
            FootStep.Volume = .1f;
            FootStep.Pitch = .05f;

            Knockback.Volume = 0.4f;
            Knockback.Pitch = -0.2f;

            Dash.Volume = .2f;
            Dash.Pitch = .2f;

            Listener = new AudioListener();
            Listener.Position = new Vector3(Position.X + 32, Position.Y + 32, 0);
        }

        public override void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, CurAnim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime) {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.W))
                Direction.Y = -1;
            else if (keyState.IsKeyDown(Keys.S)) 
                Direction.Y = 1;
            else
                Direction.Y = 0;
            if (keyState.IsKeyDown(Keys.A)) 
                Direction.X = -1;
            else if (keyState.IsKeyDown(Keys.D))
                Direction.X = 1;
            else
                Direction.X = 0;

            if ((mouseState.LeftButton == ButtonState.Pressed) && dashReady && !IsKnockbacked) {
                IsDashing = true;
                Vector2 mouseVec = new Vector2(mouseState.X, mouseState.Y);
                world.Camera.WorldPosition(ref mouseVec);

                DashVelocity = mouseVec - new Vector2(Position.X + 32, Position.Y + 32);
                DashVelocity.Normalize();
                DashVelocity *= 13f;
            } 

            boundBox.X = (int) Position.X + 24;
            boundBox.Y = (int) Position.Y + 24;

            Velocity = Direction * velPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (!dashReady) {
                dashCooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (dashCooldownTimer >= dashCooldown) {
                    dashReady = true;
                    dashCooldownTimer = 0;
                }
            }

            if (IsDashing) {
                dashTimer += gameTime.ElapsedGameTime.TotalSeconds;
                Velocity *= 0;
                Velocity += DashVelocity;
                Direction.X = Velocity.X; Direction.Y = Velocity.Y;
                Direction.Normalize();

                if (dashTimer >= 0.15f || IsKnockbacked) {
                    IsDashing = false;
                    dashReady = false;
                    dashTimer = 0;
                }
            }

            if (IsKnockbacked) {
                knockbackTimer += gameTime.ElapsedGameTime.TotalSeconds;
                Velocity *= 0.3f;
                Velocity += KnockbackVelocity;

                Direction.X = Velocity.X; Direction.Y = Velocity.Y;
                Direction.Normalize();

                if (knockbackTimer >= 0.3f) {
                    IsKnockbacked = false;
                    knockbackTimer = 0;
                }
            }

            if (!CanCollide) {
                collisionTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (collisionTimer >= collisionCooldown) {
                    CanCollide = true;
                    collisionTimer = 0;
                }
            }


            HandleTileCollision();
            HandleEntityCollision();
            HandleSoundEffects();
            SetAnimations();
            
            Listener.Position = new Vector3(Position.X + 32, Position.Y + 32, 0);

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
                world.Shaker.shake(3f, .2f);
                knockbackTimer *= 1.05f;
                //health -= KnockbackSpeeed.Length * 10
            }
        }

        private void HandleEntityCollision() {
            for (int i = 0; i < world.entities.Count; i++) {
                Entity e = world.entities[i];

                switch(e.entityType) {
                    case EntityType.FIREBALL:
                        Fireball f = e as Fireball;

                        if (CanCollide && boundBox.Intersects(f.boundBox)) {
                            f.isHit = true;
                            ApplyKnockback(f.Direction, f.knockbackPower);
                            world.Shaker.shake(2.5f, .18f);
                            //health -= e.damage;
                        }
                        break;

                    case EntityType.PLAYER:
                        break;

                    case EntityType.LASER:
                        Laser l = e as Laser;

                        if (CanCollide && !IsDashing && l.IsActive && boundBox.Intersects(l.boundBox)) {
                            Vector2 knockbackDir;
                            if (l.direction == LaserDirection.HORIZONTAL) {
                                if (Position.Y > l.Position.Y) {
                                    //player is under the laser
                                    knockbackDir = new Vector2(0, 1);
                                } else {
                                    //laser is under the player
                                    knockbackDir = new Vector2(0, -1);
                                }
                            } else {
                                if (Position.X < l.Position.X) {
                                    // player is to the left of the laser
                                    knockbackDir = new Vector2(-1, 0);
                                } else {
                                    // player is to the right of the laser
                                    knockbackDir = new Vector2(1, 0);
                                }
                            }

                            ApplyKnockback(knockbackDir, l.KnockbackPower);
                            world.Shaker.shake(2f, 0.15f);
                            CanCollide = false;
                            IsDashing = false;
                        }

                        break;
                }
            }
        }

        private void ApplyKnockback(Vector2 direction, float speed) {
            if (IsKnockbacked)
                knockbackTimer = 0;

            IsKnockbacked = true;
            KnockbackVelocity = speed * direction;
        } 

        private void HandleSoundEffects() {
            if (Velocity.X != 0 || Velocity.Y != 0) {
                if (IsKnockbacked) {
                    FootStep.Stop();
                    if (Knockback.State != SoundState.Playing)
                        Knockback.Play();
                } else if (IsDashing) {
                    Knockback.Stop();
                    FootStep.Stop();
                    if (Dash.State != SoundState.Playing)
                        Dash.Play();
                } else {
                    Knockback.Stop();
                    if (FootStep.State != SoundState.Playing)
                        FootStep.Play();
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
