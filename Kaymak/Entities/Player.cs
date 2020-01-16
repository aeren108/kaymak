using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            sprite = content.Load<Texture2D>("player");

            RightWalk = new Animation(90, 4, 32, 48, 0);
            LeftWalk = new Animation(90, 4, 32, 48, 1);
            IdleLeft = new Animation(200, 2, 32, 48, 2);
            IdleRight = new Animation(200, 2, 32, 48, 3);

            BoundBoxX = new Rectangle(0, 0, 32, 48);
            BoundBoxY = new Rectangle(0, 0, 32, 48);

            CurAnim = IdleRight;

            Console.WriteLine(Position.ToString());
        }

        public void Render(SpriteBatch batch) {
            batch.Draw(sprite, Position, CurAnim.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W)) {
                Direction.Y = -1;
                if (CurAnim == IdleLeft) {
                    CurAnim = LeftWalk;
                } else if (CurAnim == IdleRight) {
                    CurAnim = RightWalk;
                }
            } else if (state.IsKeyDown(Keys.S)) {
                Direction.Y = 1;
                if (CurAnim == IdleLeft) {
                    CurAnim = LeftWalk;
                } else if (CurAnim == IdleRight) {
                    CurAnim = RightWalk;
                }
            } else {
                Direction.Y = 0;
            } if (state.IsKeyDown(Keys.A)) {
                Direction.X = -1;
                CurAnim = LeftWalk;
            } else if (state.IsKeyDown(Keys.D)) {
                Direction.X = 1;
                CurAnim = RightWalk;
            } else {
                Direction.X = 0;
            }

            if (Direction.X == 0 && Direction.Y == 0) {
                if (CurAnim == LeftWalk) {
                    CurAnim = IdleLeft;
                } else if (CurAnim == RightWalk) {
                    CurAnim = IdleRight;
                }
            } /*else
                Direction.Normalize();*/
            
            Velocity = Direction * 5;

            HandleCollision();

            Position += Velocity;
            CurAnim.Update(gameTime);
        }

        public void HandleCollision() {
            BoundBoxX.X = (int) (Position + Velocity).X;
            BoundBoxX.X = (int) Position.Y;

            BoundBoxX.X = (int) Position.X;
            BoundBoxX.X = (int) (Position + Velocity).Y;

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
    }
}
