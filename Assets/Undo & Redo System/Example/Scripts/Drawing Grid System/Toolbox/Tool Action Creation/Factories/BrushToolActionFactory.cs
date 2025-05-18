using System;
using RedRats.ActionHistory;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// Creates new <see cref="ActionBase{T}"/>s for the <see cref="BrushTool{T}"/>.
    /// </summary>
    public class BrushToolActionFactory<T> : IToolActionFactory<T> where T : IComparable
    {
        public ActionBase<T> Create(ToolBase<T> tool, ObjectGrid<T> grid, Vector2Int position, T value, T lastValue, Sprite graphicValue, Sprite lastGraphicValue, int layer, Action<T> fallback)
        {
            return new UseToolAction<T>(tool as BrushTool<T>, grid, position, value, lastValue, graphicValue, lastGraphicValue, layer, fallback);
        }
    }
}