using Kaymak;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Effect {
    class ScreenShaker {
        private double elapsedTime = 0;
        private float power, duration;
        private Random random = new Random();
        private bool isShaking = false;
        public void shake(float power, float duration) {
            if (isShaking) {
                elapsedTime *= 0.8f;//make it longer
                return;
            }

            this.power = power;
            this.duration = duration;
            elapsedTime = 0f;

            isShaking = true;
        }

        public void Update(GameTime gameTime, Camera camera) {
            if (elapsedTime < duration) {
                double currentPower = power  * ((duration - elapsedTime) / duration);
                Vector2 shake = new Vector2((float) ((random.NextDouble() - 0.5f) * 2 * currentPower), (float)((random.NextDouble() - 0.5f) * 2 * currentPower));
                camera.Pos += shake;

                elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            } else {
                isShaking = false;
            }
        }
    }
}
