using System;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    public abstract class ToolBase<T>
    {
        protected readonly Action<int, Vector2Int, Sprite> whenGraphicDrawn;
        protected readonly Action<int> whenEffectFinished;

        protected ToolBase(Action<int, Vector2Int, Sprite> whenGraphicDrawn, Action<int> whenEffectFinished)
        {
            this.whenEffectFinished = whenEffectFinished;
            this.whenGraphicDrawn = whenGraphicDrawn;
        }

        /// <summary>
        /// Applies the effect of the tool.
        /// </summary>
        /// <param name="grid">The grid to affect.</param>
        /// <param name="position">The position on the grid to affect.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="layer">The graphic layer index to draw the <see cref="graphicValue"/>.</param>
        /// <param name="graphicValue">The graphic that represents the data.</param>
        public abstract void ApplyEffect(ObjectGrid<T> grid, Vector2Int position, T value, Sprite graphicValue, int layer);
    }
}