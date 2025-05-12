using System;
using System.Collections.Generic;
using RedRats.Core;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// The Bucket Fill Tool, that fills a local area of cells.
    /// </summary>
    public class BucketTool<T> : ToolBase<T> where T : IComparable
    {
        private ObjectGrid<T> grid;
        private T currentValue;
        private T valueToOverride;
        private readonly IList<Vector2Int> criticalPositions;
        private readonly ISet<Vector2Int> lastProcessedPositions;

        private Sprite graphicValue;
        private int layerIndex;

        public BucketTool(Action<int, Vector2Int, Sprite> whenGraphicDrawn, Action<int> whenEffectFinished) : base(whenGraphicDrawn, whenEffectFinished)
        {
            criticalPositions = new List<Vector2Int>();
            lastProcessedPositions = new HashSet<Vector2Int>();
        }

        public override void ApplyEffect(ObjectGrid<T> grid, Vector2Int position, T value, Sprite graphicValue, int layer)
        {
            Preconditions.IsNotNull(grid, "grid");
            
            this.grid = grid;
            this.layerIndex = layer;
            this.currentValue = value;
            this.valueToOverride = GetFrom(position);
            this.graphicValue = graphicValue;
            this.criticalPositions.Clear();
            this.lastProcessedPositions.Clear();

            //Return if value on the grid is the same as the bucket fill value.
            if (valueToOverride.CompareTo(currentValue) == 0) return;
            
            //Go to the left-most pixel
            Vector2Int startPos = GetLeftmostPosition(position);

            //Iterative go to the right
            ProcessLoop(startPos);
        }

        /// <summary>
        /// Applies the effect of the tool on a list of positions.
        /// </summary>
        /// <param name="grid">The grid to affect.</param>
        /// <param name="positionsToDraw"></param>
        /// <param name="value">The value to set.</param>
        /// <param name="graphicValue">The graphic that represents the data.</param>
        /// <param name="layer">The graphic layer index to draw the <see cref="graphicValue"/>.</param>
        public void ApplyEffect(ObjectGrid<T> grid, ISet<Vector2Int> positionsToDraw, T value, Sprite graphicValue, int layer)
        {
            if (positionsToDraw == null || positionsToDraw.Count == 0) return;
            foreach (Vector2Int pos in positionsToDraw)
            {
                grid.SetTo(pos, value);
                whenGraphicDrawn?.Invoke(layer, pos, graphicValue);
            }
            whenEffectFinished?.Invoke(layer);
        }
        
        /// <summary>
        /// Processes the algorithm for individual position.
        /// </summary>
        /// <param name="pos">The position to process.</param>
        private void ProcessLoop(Vector2Int pos)
        {
            // Check if grid pixel on top/bottom is critical. If it is, add it to a list.
            CheckCellIsCritical(pos + Vector2Int.up);
            CheckCellIsCritical(pos + Vector2Int.down);
            
            //Set value to cell.
            grid.SetTo(pos, currentValue);
            whenGraphicDrawn?.Invoke(layerIndex, pos, graphicValue);
            lastProcessedPositions.Add(pos);
            
            //Once we reach the end, jump to on of the critical pixels.
            if (pos.x >= grid.Width - 1 || GetFrom(pos + Vector2Int.right).CompareTo(valueToOverride) != 0)
            {
                if (criticalPositions.Count > 0)
                {
                    Vector2Int newPosition = criticalPositions[^1];
                    criticalPositions.RemoveAt(criticalPositions.Count-1);
                    ProcessLoop(newPosition);
                }
                else
                {
                    whenEffectFinished?.Invoke(layerIndex);
                    return;
                }
            }
            //Increment pixel x
            else ProcessLoop(pos + Vector2Int.right);
        }
        
        /// <summary>
        /// Checks if a pixel is considered "critical." If it is, it's added to teh critical pixel list
        /// and later used as a starting position for filling.
        /// </summary>
        /// <param name="pos">The position to check for.</param>
        private void CheckCellIsCritical(Vector2Int pos)
        {
            if (pos.x < 0) return;
            if (pos.y >= grid.Height || pos.y < 0) return;
            
            T value = GetFrom(pos);

            //Is the cell not filled?
            if (value.CompareTo(valueToOverride) != 0) return;
            
            //Grab the edge cell and add it to the list.
            Vector2Int edgeCellPosition = GetLeftmostPosition(pos);
            criticalPositions.Add(edgeCellPosition);
        }
        
        /// <summary>
        /// Finds the leftmost position in the current row.
        /// </summary>
        /// <param name="currentPos">The position to check from.</param>
        /// <returns>The leftmost position found.</returns>
        /// <exception cref="RecursionException">Is thrown when the method fails to return a position or find a new one.</exception>
        private Vector2Int GetLeftmostPosition(Vector2Int currentPos)
        {
            if (currentPos.x <= 0) return currentPos;
            
            T valueToTheLeft = GetFrom(currentPos + Vector2Int.left);
            if (valueToTheLeft.CompareTo(valueToOverride) != 0) return currentPos;

            return GetLeftmostPosition(currentPos + Vector2Int.left);
        }

        /// <summary>
        /// Returns a value from the current grid.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private T GetFrom(Vector2Int pos) => grid.GetAt(pos);
        
        public override string ToString() => "Bucket Tool";
        
        public ISet<Vector2Int> LastProcessedPositions { get => lastProcessedPositions; }
        
    }
}