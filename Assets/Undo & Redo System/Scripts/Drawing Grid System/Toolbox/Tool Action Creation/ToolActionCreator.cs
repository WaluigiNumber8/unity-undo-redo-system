using System;
using System.Collections.Generic;
using RedRats.ActionHistory;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// Creates <see cref="ActionBase{T}"/>s for the toolbox.
    /// </summary>
    public static class ToolActionCreator<T> where T : IComparable
    {
        private static readonly IDictionary<Type, IToolActionFactory<T>> factories;

        static ToolActionCreator()
        {
            factories = new Dictionary<Type, IToolActionFactory<T>>();
            factories.Add(typeof(BrushTool<T>), new BrushToolActionFactory<T>());
            factories.Add(typeof(BucketTool<T>), new BucketToolActionFactory<T>());
            factories.Add(typeof(PickerTool<T>), new SilentToolsActionFactory<T>());
            factories.Add(typeof(SelectionTool<T>), new SilentToolsActionFactory<T>());
        }

        public static ActionBase<T> Create(ToolBase<T> tool, ObjectGrid<T> grid, Vector2Int position, T value, T lastValue, Sprite graphicValue, Sprite lastGraphicValue, int layer, Action<T> fallback)
        {
            factories.TryGetValue(tool.GetType(), out IToolActionFactory<T> factory);
            return factory?.Create(tool, grid, position, value, lastValue, graphicValue, lastGraphicValue, layer, fallback);
        }
    }
}