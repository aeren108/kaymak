using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Content;

namespace kaymak.Map {
    class TiledMap : GameObject {
        private JObject map;
        private World world;

        public int Width, Height;
        public int SheetWidth, SheetHeight;
        public int TileSize;
        public string SpriteSheetPath;

        public Texture2D SpriteSheet;

        private List<Layer> layers;
        public Rectangle[] Blocks;
        
        public TiledMap(World world, String path) {
            map = JObject.Parse(json: File.ReadAllText(path));
            layers = new List<Layer>();

            this.world = world;
        }
        public void LoadContent(ContentManager content) {
            LoadMap();

            SpriteSheet = content.Load<Texture2D>(SpriteSheetPath);

            SheetWidth = SpriteSheet.Width / TileSize;
            SheetHeight = SpriteSheet.Height / TileSize;
        }

        private void LoadMap() {
            Width = (int) map["width"];
            Height = (int) map["height"];

            SpriteSheetPath = (string) map["spritesheet"];
            TileSize = (int) map["tileSize"];

            JArray layerArray = (JArray) map["layers"];
            JObject[] layerObjects = layerArray.Select(c => (JObject) c).ToArray();

            foreach (var layer in layerObjects) {
                int[] tiles = layer["tiles"].Select(c => (int) c).ToArray();
                bool isVisible = (bool) layer["isVisible"];

                layers.Add(new Layer(this, tiles, isVisible, world.Camera));
            }

            JArray colArray = (JArray) map["collisionObjects"];
            JObject[] rectObjects = colArray.Select(c => (JObject) c).ToArray();
            List<Rectangle> colBlocks = new List<Rectangle>();

            foreach (var rectObject in rectObjects) {
                int x = (int) rectObject["@x"];
                int y = (int) rectObject["@y"];
                int width = (int) rectObject["@width"];
                int height = (int) rectObject["@height"];

                colBlocks.Add(new Rectangle(x, y, width, height));
            }

            Blocks = colBlocks.ToArray();
            Console.WriteLine(Blocks == null);
        }

        public int GetTile(int x, int y) {
            return layers[layers.Count - 1].GetId(x, y);
        }

        public void Render(SpriteBatch batch) {
            for (int i = 0; i < layers.Count; i++) {
                if (layers[i].isVisible)
                    layers[i].Render(batch);
            }
        }

        public void Update(GameTime gameTime) {
            
        }
    }
}
