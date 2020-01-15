using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaymak {
    interface GameObject {
        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void Render(SpriteBatch batch);
    }
}
