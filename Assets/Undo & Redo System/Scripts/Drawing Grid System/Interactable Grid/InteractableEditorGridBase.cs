using UnityEngine;

namespace RedRats.DrawingGrid
{
    public abstract class InteractableEditorGridBase : MonoBehaviour
    {
        /// <summary>
        /// Loads the editor grid with colors.
        /// </summary>
        /// <param name="colorArray">A color array to read from.</param>
        /// <param name="indexGrid">The grid of indexes to read.</param>
        public abstract void LoadWithColors(ObjectGrid<int> indexGrid, Color[] colorArray);

        /// <summary>
        /// Updates a cell with a new value.
        /// </summary>
        /// <param name="position">The grid cell to update.</param>
        /// <param name="value">The value to set.</param>
        public abstract void UpdateCell(Vector2Int position, Sprite value);
        /// <summary>
        /// Updates a cell with a new value.
        /// </summary>
        /// <param name="layer">The index of the layer to update.</param>
        /// <param name="position">The grid cell to update.</param>
        /// <param name="value">The value to set.</param>
        public abstract void UpdateCell(int layer, Vector2Int position, Sprite value);

        /// <summary>
        /// Returns a value of a cell.
        /// </summary>
        /// <param name="position">The cell's grid position.</param>
        /// <returns>The sprite value.</returns>
        public abstract Sprite GetCell(Vector2Int position);
        /// <summary>
        /// Returns a value of a cell.
        /// </summary>
        /// <param name="layer">The layer index, from which to take the value.</param>
        /// <param name="position">The cell's grid position.</param>
        /// <returns>The sprite value.</returns>
        public abstract Sprite GetCell(int layer, Vector2Int position);
        
        /// <summary>
        /// Applies grid changes.
        /// </summary>
        public abstract void Apply(int layer);

        /// <summary>
        /// Clears all elements on the active grid.
        /// </summary>
        public abstract void ClearAllCells();
        
        public abstract int ActiveLayer { get; }
        public abstract Sprite ActiveLayerSprite { get; }
    }
}