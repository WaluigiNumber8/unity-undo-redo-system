using System;
using UnityEngine;

namespace RedRats.DrawingGrid.Tools 
{
    /// <summary>
    /// A base for all toolboxes.
    /// </summary>
    public interface IToolBox
    {
        public event Action<ToolType> OnSwitchTool; 

        /// <summary>
        /// Switches to a new tool.
        /// </summary>
        /// <param name="tool">Tool Type of the new tool.</param>
        /// <exception cref="InvalidOperationException">Is thrown when the Tool is not supported.</exception>
        void SwitchTool(ToolType tool);

        /// <summary>
        /// Draws on the UI grid.
        /// </summary>
        /// <param name="layerIndex">The layer to draw onto.</param>
        /// <param name="position">The position to draw on.</param>
        /// <param name="value">The value to draw.</param>
        public void WhenDrawOnUIGrid(int layerIndex, Vector2Int position, Sprite value);
    }
}