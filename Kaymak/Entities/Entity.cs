using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Kaymak.Entities {
    abstract class Entity : GameObject {
        public EntityType entityType;

        protected World world;
        public Vector2 Position;
        public Rectangle boundBox;

        protected Texture2D sprite;

        public Entity(World world, EntityType entityType) {
            this.world = world;
            this.entityType = entityType;

            Position = new Vector2(0, 0);
            boundBox = new Rectangle(0, 0, 0, 0);
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Render(SpriteBatch batch);
        public abstract void Update(GameTime gameTime);
    }
}
