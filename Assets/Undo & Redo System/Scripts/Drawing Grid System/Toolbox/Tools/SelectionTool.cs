using System;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// A tool that is able to select assets placed in the grid and update UI accordingly.
    /// </summary>
    /// <typeparam name="T">Any type of <see cref="IComparable"/>.</typeparam>
    public class SelectionTool<T> : ToolBase<T> where T : IComparable
    {
        public event Action<T> OnSelectValue;
        
        public SelectionTool(Action<int, Vector2Int, Sprite> whenGraphicDrawn, Action<int> whenEffectFinished) : base(whenGraphicDrawn, whenEffectFinished) { }
        
        public override void ApplyEffect(ObjectGrid<T> grid, Vector2Int position, T value, Sprite graphicValue, int layer)
        {
            T selected = grid.GetAt(position);
            OnSelectValue?.Invoke(selected);
        }

        public override string ToString() => "Selection Tool";

    }
}