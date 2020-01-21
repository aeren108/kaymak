﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Kaymak {
    interface GameObject {
        void LoadContent();
        void Update(GameTime gameTime);
        void Render(SpriteBatch batch);
    }
}
