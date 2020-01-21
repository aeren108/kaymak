using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kaymak.Map.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace Kaymak.Map {
    class TiledMap : GameObject {
        private JObject map;
        private String path;
        private World world;

        public int Width, Height;
        public int SheetWidth, SheetHeight;
        public int TileSize;
        public string SpriteSheetPath;

        public Texture2D SpriteSheet;

        private List<Layer> Layers;
        public Rectangle[] Blocks;
        public List<int> BlockedTiles;

        public int tileCount = 0;

        public TiledMap(World world, String path) {
            this.path = path;
            this.world = world;

            Layers = new List<Layer>();
        }
        public void LoadContent() {
            map = JObject.Parse(json: File.ReadAllText(Main.CM.RootDirectory + path));

            ParseMap();

            SpriteSheet = Main.CM.Load<Texture2D>(SpriteSheetPath);

            SheetWidth = SpriteSheet.Width / TileSize;
            SheetHeight = SpriteSheet.Height / TileSize;

            tileCount = SheetWidth * SheetHeight;

            foreach (var layer in Layers)
                layer.LoadContent();
        }

        private void ParseMap() {
            Width = (int) map["width"];
            Height = (int) map["height"];

            SpriteSheetPath = (string) map["spritesheet"];
            TileSize = (int) map["tileSize"];

            JArray layerArray = (JArray) map["layers"];
            JObject[] layerObjects = layerArray.Select(c => (JObject) c).ToArray();

            foreach (var layer in layerObjects) {
                int[] tiles = layer["tiles"].Select(c => (int) c).ToArray();
                bool isVisible = (bool) layer["isVisible"];

                Layers.Add(new Layer(this, tiles, isVisible, world.Camera));
            }

            JArray blockedTiles = (JArray) map["blockedTiles"];
            BlockedTiles = blockedTiles.Select(c => (int) c).ToList();

            JArray colArray = (JArray) map["collisionObjects"];
            JObject[] rectObjects = colArray.Select(c => (JObject) c).ToArray();
            List<Rectangle> colBlocks = new List<Rectangle>();

            foreach (var rectObject in rectObjects) {
                int x = (int) rectObject["x"];
                int y = (int) rectObject["y"];
                int width = (int) rectObject["width"];
                int height = (int) rectObject["height"];

                colBlocks.Add(new Rectangle(x, y, width, height));
            }

            Blocks = colBlocks.ToArray();
        }

        public int GetId(int x, int y) {
            return Layers[Layers.Count - 1].GetId(x, y);
        }

        public Tile GetTile(int x, int y) {
            return Layers[Layers.Count - 1].GetTile(x, y);
        }

        public bool IsBlocked(int x, int y) {
            for (int i = 0; i < Layers.Count; i++) {
                if (Layers[i].IsBlocked(x, y))
                    return true;
            }

            return false;
        }

        public void Render(SpriteBatch batch) {
            for (int i = 0; i < Layers.Count; i++) {
                if (Layers[i].isVisible)
                    Layers[i].Render(batch);
            }
        }

        public void Render(SpriteBatch batch, int i) {
            Layers[i].Render(batch);
        }

        public void Update(GameTime gameTime) {
            for (int i = 0; i < Layers.Count; i++) {
                Layers[i].Update(gameTime);
            }
        }
    }
}
