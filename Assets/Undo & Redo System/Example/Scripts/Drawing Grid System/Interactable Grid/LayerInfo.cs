using UnityEngine;
using UnityEngine.UI;

namespace RedRats.DrawingGrid
{
    /// <summary>
    /// Holds data for a <see cref="InteractableEditorGrid"/> layer.
    /// </summary>
    [System.Serializable]
    public struct LayerInfo
    {
        public Image layer;
        public Color outOfFocusColor;
    }
}