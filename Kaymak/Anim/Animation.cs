using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaymak.Anim {
    class Animation : GameObject {
        private double Delay;
        private int FrameCount;
        private int FrameWidth;
        private int FrameHeight;
        private double ElapsedTime;
        public Rectangle SourceRectangle;

        int CurrentFrame = 0;

        public Animation(float delay, int frameCount, int frameWidth, int frameHeight, int startY) {
            this.Delay = delay;
            this.FrameCount = frameCount;
            this.FrameHeight = frameHeight;
            this.FrameWidth = frameWidth;

            SourceRectangle = new Rectangle(0, startY * frameHeight, FrameWidth, FrameHeight);
        }

        public void LoadContent(ContentManager content) {
            
        }

        public void Render(SpriteBatch batch) {
            
        }

        public void Update(GameTime gameTime) {
            ElapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElapsedTime >= Delay) {
                ElapsedTime = 0;

                if (CurrentFrame < FrameCount-1)
                    CurrentFrame++;
                else
                    CurrentFrame = 0;

                SourceRectangle.X = CurrentFrame * FrameWidth;
            }
        }
    }
}
