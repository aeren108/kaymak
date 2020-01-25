using System;
using System.Collections.Generic;

using Kaymak.Entities;
using Kaymak.Map;
using static Kaymak.Main;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Kaymak.Effect;
using kaymak.Kaymak.Entities;

namespace Kaymak {
    class World : GameObject {
        public TiledMap Map;
        public Camera Camera;
        public ScreenShaker Shaker;

        public List<Entity> entities;
        public Entity player;

        private GraphicsDevice graphics;
        private Song gameTheme;

        private int prevScroll = 0;
        private double fireballTimer = 0;
        private double laserTimer = 0;

        private double laserTreshold = 1.2f;

        private Random random = new Random();
        private bool canPress;

        public World(GraphicsDevice graphicsDevice) {
            this.graphics = graphicsDevice;

            entities = new List<Entity>();
        }

        public void LoadContent() {
            Map = new TiledMap(this, "/dungeon.json");
            Camera = new Camera(graphics);
            player = new Player(this);
            Shaker = new ScreenShaker();

            Map.LoadContent();
            player.LoadContent();
            entities.Add(player);

            gameTheme = CM.Load<Song>("hero_immortal");
            MediaPlayer.Volume = 0.05f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(gameTheme);
        }

        public void Render(SpriteBatch batch) {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.Transform);

            Map.Render(batch, 0);

            for (int i = 0; i < entities.Count; i++) {
                entities[i].Render(batch);
            }

            Map.Render(batch, 1);
            Map.Render(batch, 2);

            batch.End();
        }

        public void Update(GameTime gameTime) {
            MouseState state = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            Camera.Pos = player.Position;
            fireballTimer += gameTime.ElapsedGameTime.TotalSeconds;
            laserTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (Camera.Pos.X < graphics.Viewport.Width / 2) {
                Camera.Pos.X = graphics.Viewport.Width / 2;
            } else if (Camera.Pos.X > Map.Width * Map.TileSize - graphics.Viewport.Width / 2) {
                Camera.Pos.X = Map.Width * Map.TileSize - graphics.Viewport.Width / 2;
            }

            if (Camera.Pos.Y < graphics.Viewport.Height / 2) {
                Camera.Pos.Y = graphics.Viewport.Height / 2;
            } else if (Camera.Pos.Y > Map.Height * Map.TileSize - graphics.Viewport.Height / 2) {
                Camera.Pos.Y = Map.Height * Map.TileSize - graphics.Viewport.Height / 2;
            }

            if (keyState.IsKeyDown(Keys.Space)) {
                if (canPress)
                    Map.ChangeTileset();
                canPress = false;
            } if (keyState.IsKeyUp(Keys.Space))
                canPress = true;

            if (state.ScrollWheelValue < prevScroll)
                MediaPlayer.Volume -= .05f;
            else if (state.ScrollWheelValue > prevScroll)
                MediaPlayer.Volume += .05f;

            prevScroll = state.ScrollWheelValue;

            if (MediaPlayer.Volume < 0) MediaPlayer.Volume = 0;
            else if (MediaPlayer.Volume > 1) MediaPlayer.Volume = 1;

            if (MediaPlayer.Volume == 0 && MediaPlayer.State == MediaState.Playing) 
                MediaPlayer.Pause();
            else if (MediaPlayer.Volume != 0 && MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();

            //generating fireballs every 0.2 seconds
            if (fireballTimer >= 0.2f) {
                fireballTimer = 0;
                
                for (int i = 0; i < 2; i++) {
                    int dir = random.Next(0, 2);
                    Fireball fb = null;

                    if (dir == 0) 
                        fb = new Fireball(this, FireballDirection.HORIZONTAL);
                    else if (dir == 1)
                        fb = new Fireball(this, FireballDirection.VERTICAL);

                    fb.LoadContent();
                    entities.Insert(0, fb);
                }
                //Console.WriteLine(entities.Count);
            }

            if (laserTimer >= laserTreshold) {
                for (int i = 0; i < 2; i++) {
                    int dir = random.Next(0, 2);
                    Laser l = null;

                    if (dir == 0)
                        l = new Laser(this, LaserDirection.HORIZONTAL);
                    else if (dir == 1)
                        l = new Laser(this, LaserDirection.VERTICAL);

                    l.LoadContent();
                    entities.Add(l);

                    laserTreshold = random.NextDouble() * (1.5d - 0.8d) + 0.8d;
                    laserTimer = 0;
                }
            }

            foreach (Entity e in entities.ToArray()) {
                e.Update(gameTime);

                if (e.entityType == EntityType.FIREBALL) {
                    if (((Fireball) e).isHit) {
                        entities.Remove(e);
                    }
                } else if (e.entityType == EntityType.LASER) {
                    if ((e as Laser).IsFinished)
                        entities.Remove(e);
                }
            }

            Map.Update(gameTime);
            Shaker.Update(gameTime, Camera);
            Camera.Update();  
        }
    }
}
