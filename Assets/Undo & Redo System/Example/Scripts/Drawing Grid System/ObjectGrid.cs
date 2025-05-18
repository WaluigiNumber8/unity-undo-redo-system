using System;
using RedRats.Core;
using UnityEngine;

namespace RedRats.DrawingGrid
{
    /// <summary>
    /// An object that stores T-type in a grid array.
    /// </summary>
    public class ObjectGrid<T>
    {
        private readonly int width;
        private readonly int height;
        private readonly T[,] cellArray;

        private readonly Func<T> createDefaultObject;

        public ObjectGrid(int width, int height, Func<T> createDefaultObject)
        {
            this.width = width;
            this.height = height;
            cellArray = new T[width, height];
            this.createDefaultObject = createDefaultObject;

            InitializeGrid();
        }

        public ObjectGrid(ObjectGrid<T> grid)
        {
            width = grid.Width;
            height = grid.Height;
            createDefaultObject = grid.createDefaultObject;
            cellArray = (T[,])grid.cellArray.Clone();
        }

        /// <summary>
        /// Change a value in a specific position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="value">Value to set.</param>
        public void SetTo(Vector2Int position, T value)
        {
            SetTo(position.x, position.y, value);
        }
        /// <summary>
        /// Change a value in a specific position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="value">Value to set.</param>
        public void SetTo(int x, int y, T value)
        {
            Preconditions.IsIntInRange(x, 0, width, "Grid X");
            Preconditions.IsIntInRange(y, 0, height, "Grid Y");
            cellArray[x, y] = value;
        }

        /// <summary>
        /// Get a value from a specific grid cell.
        /// </summary>
        /// <param name="position">The position on the grid.</param>
        /// <returns>The value on that position.</returns>
        public T GetAt(Vector2Int position) => GetAt(position.x, position.y);

        /// <summary>
        /// Get a value from a specific grid cell.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T GetAt(int x, int y)
        {
            Preconditions.IsIntInRange(x, 0, width-1, "Grid X");
            Preconditions.IsIntInRange(y, 0, height-1, "Grid Y");
            return cellArray[x, y];
        }

        /// <summary>
        /// Fills the grid with values from another grid of the same size.
        /// </summary>
        /// <param name="grid">The grid to copy from (Must have same WIDTH and HEIGHT).</param>
        public void SetFrom(ObjectGrid<T> grid)
        {
            Preconditions.IsIntEqual(grid.Width, width, "Grid Width");
            Preconditions.IsIntEqual(grid.Height, height, "Grid Height");

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cellArray[i, j] = grid.cellArray[i, j];
                }
            }
        }
        
        /// <summary>
        /// Clears the entire grid by setting all values to default. 
        /// </summary>
        public void ClearAllCells() => InitializeGrid();

        /// <summary>
        /// Returns TRUE if the grid contains a specific value.
        /// </summary>
        /// <param name="value">The value to check for.</param>
        /// <returns>TRUE if the value is contained on the grid at least once.</returns>
        public bool Contains(T value)
        {
            for (int i = 0; i < cellArray.GetLength(0); i++)
            {
                for (int j = 0; j < cellArray.GetLength(1); j++)
                {
                    if (cellArray[i, j].Equals(value)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Upon Creation, initialize the grid a default form of T.
        /// </summary>
        private void InitializeGrid()
        {
            for (int i = 0; i < cellArray.GetLength(0); i++)
            {
                for (int j = 0; j < cellArray.GetLength(1); j++)
                {
                    cellArray[i, j] = createDefaultObject();
                }
            }
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not ObjectGrid<T> other) return false;
            
            if (other.Width != width || other.Height != height) return false;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!cellArray[i, j].Equals(other.cellArray[i, j])) return false;
                }
            }
            return true;
        }

        public override int GetHashCode() => HashCode.Combine(width, height, cellArray);

        public override string ToString() => $"{width}x{height}";

        public int Width { get => width; }
        public int Height { get => height; }
        public T[,] GetCellsCopy { get => (T[,])cellArray.Clone(); }
    }
}