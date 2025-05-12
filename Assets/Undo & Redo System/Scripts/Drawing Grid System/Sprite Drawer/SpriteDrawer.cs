using System;
using RedRats.Core;
using UnityEngine;

namespace RedRats.DrawingGrid.SpriteDrawing
{
    /// <summary>
    /// Draws sprites onto other sprites.
    /// </summary>
    public class SpriteDrawer
    {
        private readonly Vector2Int size;
        private readonly Vector2Int unitSize;
        private readonly int pixelsPerUnit;
        
        private readonly Color[] emptyPixels;
        private readonly bool overdrawTransparent;
        
        public SpriteDrawer(Vector2Int size, Vector2Int unitSize, int pixelsPerUnit, bool overdrawTransparent = true)
        {
            this.size = size;
            this.unitSize = unitSize;
            this.pixelsPerUnit = pixelsPerUnit;
            this.overdrawTransparent = overdrawTransparent;

            int width = unitSize.x * size.x;
            int height = unitSize.y * size.y;
            emptyPixels = new Color[width * height];
            for (int index = 0; index < emptyPixels.Length; index++)
            {
                emptyPixels[index] = Color.clear;
            }
        }

        /// <summary>
        /// Loads a sprite based on color data.
        /// </summary>
        /// <param name="indexGrid">The grid of IDs to read.</param>
        /// <param name="colorArray">The array of colors to take data from.</param>
        public Sprite Draw(ObjectGrid<int> indexGrid, Color[] colorArray)
        {
            Texture2D tex = new TextureBuilder().WithSize(indexGrid.Width * pixelsPerUnit, indexGrid.Height * pixelsPerUnit).Build();
            Sprite sprite = new SpriteBuilder().WithTexture(tex).WithPPU(pixelsPerUnit).Build();
            ClearAllCells(sprite);
            
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    int id = indexGrid.GetAt(x, y);
                    
                    if (id == -1) continue;

                    try
                    {
                        DrawTo(sprite, new Vector2Int(x, y), colorArray[id]);
                    }
                    catch (InvalidOperationException)
                    {
                        DrawTo(sprite, new Vector2Int(x, y), Color.clear);
                    }
                }
            }
            
            Apply(sprite);
            return sprite;
        }

        public void DrawTo(Sprite canvas, Vector2Int pos, Color value)
        {
            Sprite colorValue = new SpriteBuilder().WithSingleColorTexture(value, unitSize.x, unitSize.y).WithPPU(pixelsPerUnit).Build();
            DrawTo(canvas, pos, colorValue);
        }
        
        /// <summary>
        /// Draws a new 16x16 sprite on a layer.
        /// </summary>
        /// <param name="canvas">The sprite to draw on.</param>
        /// <param name="pos">The position (in units) to draw at.</param>
        /// <param name="sprite">The sprite to draw (MUST be same size as unit).</param>
        public void DrawTo(Sprite canvas, Vector2Int pos, Sprite sprite)
        {
            Texture2D canvasTex = canvas.texture;
            Texture2D tex = sprite.texture;
            Preconditions.IsIntEqual(tex.width, 16, $"{tex.name}'s width");
            Preconditions.IsIntEqual(tex.height, 16, $"{tex.name}'s height");

            int startX = pos.x * unitSize.x;
            int startY = pos.y * unitSize.y;
            
            Color[] texPixels = tex.GetPixels();
            Color[] layerPixels = canvasTex.GetPixels(startX, startY, tex.width, tex.height);

            for (int i = 0; i < texPixels.Length; i++)
            {
                if (!overdrawTransparent && texPixels[i].a == 0) continue; // if the pixel is not transparent
                layerPixels[i] = texPixels[i];
            }

            canvasTex.SetPixels(startX, startY, tex.width, tex.height, layerPixels);
        }

        /// <summary>
        /// Returns a section of the sprite based on the unitSize.
        /// </summary>
        /// <param name="canvas">The sprite to read from.</param>
        /// <param name="pos">The starting position to read from. Width and Height are unitSize.</param>
        public Sprite Get(Sprite canvas, Vector2Int pos)
        {
            Texture2D canvasTex = canvas.texture;
            
            int startX = pos.x * unitSize.x;
            int startY = pos.y * unitSize.y;
            Color[] spritePixels = canvasTex.GetPixels(startX, startY, unitSize.x, unitSize.y);

            Texture2D tex = new TextureBuilder().WithSize(unitSize).Build();
            tex.SetPixels(spritePixels);
            tex.Apply();

            return new SpriteBuilder().WithTexture(tex).WithPPU(pixelsPerUnit).Build();
        }
        
        /// <summary>
        /// Refreshes the sprite's pixel changes (costly).
        /// </summary>
        /// <param name="sprite">The sprite to refresh.</param>
        public void Apply(Sprite sprite) => sprite.texture.Apply();
        
        /// <summary>
        /// Resets all cells to their empty state.
        /// </summary>
        /// <param name="sprite">The sprite to clear.</param>
        public void ClearAllCells(Sprite sprite) => sprite.texture.SetPixels(emptyPixels);
    }
}