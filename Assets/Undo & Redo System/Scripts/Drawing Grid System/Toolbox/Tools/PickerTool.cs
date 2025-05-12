using System;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// The Color Picker Tool, that selects an item from the palette based on whats on the canvas.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PickerTool<T> : ToolBase<T> where T : IComparable
    {
        public event Action<T> OnPickValue; 

        public PickerTool(Action<int, Vector2Int, Sprite> whenGraphicDrawn, Action<int> whenEffectFinished) : base(whenGraphicDrawn, whenEffectFinished) { }
        
        public override void ApplyEffect(ObjectGrid<T> grid, Vector2Int position, T value, Sprite graphicValue, int layer)
        {
            T valueFromGrid = grid.GetAt(position);
            OnPickValue?.Invoke(valueFromGrid);
        }

        public override string ToString() => "Picker Tool";

    }
}