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

        public int Width, Height;
        public int SheetWidth, SheetHeight;
        public int TileSize;
        public string SpriteSheetPath;

        public Texture2D SpriteSheet;

        private List<Layer> layers;
        public int[] BlockedTiles;
        public List<Rectangle> colBlocks;
        
        public TiledMap(String path) {
            map = JObject.Parse(json: File.ReadAllText(path));
            layers = new List<Layer>();
            colBlocks = new List<Rectangle>();
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
            BlockedTiles = ((JArray) map["blockedTiles"]).Select(c => (int) c).ToArray();

            JArray layerArray = (JArray) map["layers"];
            JObject[] layerObjects = layerArray.Select(c => (JObject) c).ToArray();

            foreach (var layer in layerObjects) {
                int[] tiles = layer["tiles"].Select(c => (int) c).ToArray();
                bool isVisible = (bool) layer["isVisible"];

                layers.Add(new Layer(this, tiles, isVisible));
            }

            JArray colArray = (JArray) map["collisionObjects"];
            JObject[] rectObjects = colArray.Select(c => (JObject) c).ToArray();

            foreach (var rectObject in rectObjects) {
                int x = (int) rectObject["@x"];
                int y = (int) rectObject["@y"];
                int width = (int) rectObject["@width"];
                int height = (int) rectObject["@height"];

                colBlocks.Add(new Rectangle(x, y, width, height));
            }

            colBlocks.ForEach(delegate (Rectangle r) {
                Console.WriteLine(r.ToString());
            });
        }

        public int GetTile(int x, int y) {
            return layers[layers.Count - 1].GetId(x, y);
        }

        public void Render(SpriteBatch batch) {
            foreach (var layer in layers) {
                if (layer.isVisible)
                    layer.Render(batch);
            }
        }

        public void Update(GameTime gameTime) {

        }
    }
}
