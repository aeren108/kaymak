using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using kaymak.Anim;
using Microsoft.Xna.Framework.Input;

namespace kaymak.Entities {
    class Player : GameObject {
        public Vector2 Position;

        private Vector2 Velocity;
        private Vector2 Direction;

        private Rectangle BoundBoxX;
        private Rectangle BoundBoxY;

        private World World;

        Texture2D sprite;

        Animation RightWalk;
        Animation LeftWalk;
        Animation IdleLeft;
        Animation IdleRight;

        Animation CurAnim;

        public Player(World world) {
            this.World = world;

            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Direction = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager content) {
            sprite = content.Load<Texture2D>("player2");

            RightWalk = new Animation(75, 8, 64, 64, 1);
            LeftWalk = new Animation(75, 8, 64, 64, 9);
            IdleLeft = new Animation(150, 13, 64, 64, 8);
            IdleRight = new Animation(150, 13, 64, 64, 0);

            BoundBoxX = new Rectangle(0, 0, 16, 20);
            BoundBoxY = new Rectangle(0, 0, 16, 20);

            CurAnim = IdleRight;

            Console.WriteLine(Position.ToString());
        }

        public void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, CurAnim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void Update(GameTime gameTime) {
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

            Velocity = Direction * 180 * (float) gameTime.ElapsedGameTime.TotalSeconds;

            HandleCollision();
            SetAnimations();

            Position += Velocity;
            CurAnim.Update(gameTime);
        }

        private void HandleCollision() {
            BoundBoxX.X = (int) (Position + Velocity).X + 24;
            BoundBoxX.Y = (int) Position.Y + 44;

            BoundBoxY.X = (int) Position.X + 24;
            BoundBoxY.Y = (int) (Position + Velocity).Y + 44;

            Rectangle[] blocks = World.Map.Blocks;

            if (blocks != null) {
                for (int i = 0; i < blocks.Length; i++) {
                    if (BoundBoxX.Intersects(blocks[i]))
                        Velocity.X = 0;
                    if (BoundBoxY.Intersects(blocks[i]))
                        Velocity.Y = 0;
                }
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
