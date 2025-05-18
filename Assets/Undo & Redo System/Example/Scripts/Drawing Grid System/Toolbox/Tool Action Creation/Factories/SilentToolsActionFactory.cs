using System;
using RedRats.ActionHistory;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// Creates null <see cref="ActionBase{T}"/> so that it doesn't get added to Action History and uses the tool the normal way.
    /// </summary>
    public class SilentToolsActionFactory<T> : IToolActionFactory<T> where T : System.IComparable
    {
        public ActionBase<T> Create(ToolBase<T> tool, ObjectGrid<T> grid, Vector2Int position, T value, T lastValue, Sprite graphicValue, Sprite lastGraphicValue, int layer, Action<T> fallback)
        {
            tool.ApplyEffect(grid, position, value, graphicValue, layer);
            return null;
        }
    }
}