using System;
using System.Collections.Generic;
using RedRats.DrawingGrid;
using RedRats.DrawingGrid.Tools;
using UnityEngine;

namespace RedRats.ActionHistory
{
    /// <summary>
    /// An action that uses the <see cref="BucketTool{T}"/> on a grid.
    /// </summary>
    public class UseBucketToolAction<T> : ActionBase<T> where T : IComparable
    {
        private readonly BucketTool<T> tool;
        private readonly ObjectGrid<T> grid;
        private readonly Vector2Int position;
        private readonly int layer;
        
        private readonly T value;
        private readonly T lastValue;
        private readonly Sprite graphicValue;
        private readonly Sprite lastGraphicValue;
        private ISet<Vector2Int> affectedPositions;
        
        public UseBucketToolAction(BucketTool<T> tool, ObjectGrid<T> grid, Vector2Int position, T value, T lastValue, Sprite graphicValue, Sprite lastGraphicValue, int layer, Action<T> fallback) : base(fallback)
        {
            this.tool = tool;
            this.grid = grid;
            this.position = position;
            this.value = value;
            this.lastValue = lastValue;
            this.layer = layer;
            this.graphicValue = graphicValue;
            this.lastGraphicValue = lastGraphicValue;
        }
        
        protected override void ExecuteSelf()
        {
            tool.ApplyEffect(grid, position, value, graphicValue, layer);
            affectedPositions = new HashSet<Vector2Int>(tool.LastProcessedPositions);
        }

        protected override void UndoSelf() => tool.ApplyEffect(grid, affectedPositions, lastValue, lastGraphicValue, layer);

        public override bool NothingChanged() => value.CompareTo(lastValue) == 0;

        public override object AffectedConstruct => grid;
        public override T Value { get => value; }
        public override T LastValue { get => lastValue; }

        public override string ToString() => $"{tool}: {lastValue} -> {value} at {layer}-{position}";
    }
}