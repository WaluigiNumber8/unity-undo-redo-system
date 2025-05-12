using System;
using System.Collections.Generic;
using RedRats.Core;
using RedRats.DrawingGrid.SpriteDrawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedRats.DrawingGrid
{
    /// <summary>
    /// A type of grid the player can interact with via a pointer.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class InteractableEditorGrid : InteractableEditorGridBase, IPointerClickHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int> OnClick;
        public event Action<Vector2Int> OnClickAlternative;
        public event Action OnPointerLeave;
        public event Action OnPointerComeIn;

        [SerializeField] private int spriteSize = 16;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private LayerInfo[] layers;
        
        private RectTransform ttransform;
        private SpriteDrawer drawer;
        private Camera cam;
        
        private int activeLayerIndex;
        
        private Vector2 cellSize;
        private Vector2Int selectedPos;
        
        private void Awake()
        {
            Preconditions.IsIntBiggerOrEqualTo(gridSize.x, 1, "Grid Size X");
            Preconditions.IsIntBiggerOrEqualTo(gridSize.y, 1, "Grid Size Y");
            Preconditions.IsIntBiggerThan(layers.Length, 0, "Layers amount");
            
            ttransform = GetComponent<RectTransform>();
            drawer = new SpriteDrawer(gridSize, new Vector2Int(spriteSize, spriteSize), spriteSize);
            cam = Camera.main;
            
            cellSize = new Vector2(ttransform.rect.width / gridSize.x, ttransform.rect.height / gridSize.y);
            
            PrepareLayers();
            SwitchActiveLayer(0);
        }

        public void OnPointerEnter(PointerEventData eventData) => OnPointerComeIn?.Invoke();
        public void OnPointerExit(PointerEventData eventData) => OnPointerLeave?.Invoke();
        
        public void OnPointerMove(PointerEventData eventData)
        {
            RecalculateSelectedPosition(eventData.position);
            //If the pointer is over the grid and the left mouse button is held, invoke the OnClick event.
            if (Input.GetMouseButton(0))
            {
                OnClick?.Invoke(selectedPos);
                return;
            }
            if (Input.GetMouseButton(1))
            {
                OnClickAlternative?.Invoke(selectedPos);
                return;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            RecalculateSelectedPosition(eventData.position);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnClick?.Invoke(selectedPos);
                    return;
                case PointerEventData.InputButton.Right:
                    OnClickAlternative?.Invoke(selectedPos);
                    return;
            }
        }

        public override void LoadWithColors(ObjectGrid<int> indexGrid, Color[] colorArray)
        {
            Preconditions.IsIntEqual(indexGrid.Width, gridSize.x, "Grid Width");
            Preconditions.IsIntEqual(indexGrid.Height, gridSize.y, "Grid Height");
            GetActiveLayer().sprite = drawer.Draw(indexGrid, colorArray);
        }
        
        public override void UpdateCell(Vector2Int position, Sprite value) => UpdateCell(activeLayerIndex, position, value);
        public override void UpdateCell(int layer, Vector2Int position, Sprite value)
        {
            drawer.DrawTo(layers[layer].layer.sprite, position, value);
        }

        public override Sprite GetCell(Vector2Int position) => GetCell(activeLayerIndex, position);
        public override Sprite GetCell(int layer, Vector2Int position)
        {
            return drawer.Get(layers[layer].layer.sprite, position);
        }

        public override void Apply(int layer) => drawer.Apply(layers[layer].layer.sprite);

        public override void ClearAllCells()
        {
            drawer.ClearAllCells(GetActiveLayer().sprite);
            Apply(activeLayerIndex);
        }

        /// <summary>
        /// Switches the currently active layer.
        /// </summary>
        /// <param name="layerIndex">The index of the active layer.</param>
        public void SwitchActiveLayer(int layerIndex)
        {
            Preconditions.IsIndexWithingCollectionRange(layers, layerIndex, nameof(layers));
            
            activeLayerIndex = layerIndex;
            RefreshLayerColors(layerIndex);
        }
        
        /// <summary>
        /// Prepares the grid layers.
        /// </summary>
        private void PrepareLayers()
        {
            foreach (LayerInfo info in layers)
            {
                info.layer.color = Color.white;
                info.layer.sprite = new SpriteBuilder()
                                        .WithSingleColorTexture(Color.clear, spriteSize * gridSize.x, spriteSize * gridSize.y)
                                        .WithPPU(spriteSize)
                                        .Build();
            }
        }
        
        /// <summary>
        /// Updates the currently selected grid position based on the pointer.
        /// </summary>
        private void RecalculateSelectedPosition(Vector2 pointerPos)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ttransform, pointerPos, cam, out Vector2 pos)) return;
            
            int x = (int)Mathf.Floor(pos.x / cellSize.x);
            int y = (int)Mathf.Floor(pos.y / cellSize.y);
            x = Mathf.Clamp(x, 0, gridSize.x - 1);
            y = Mathf.Clamp(y, 0, gridSize.y - 1);
            
            selectedPos = new Vector2Int(x, y);
        }

        private Image GetActiveLayer() => layers[activeLayerIndex].layer;
        
        /// <summary>
        /// Refreshes the color of all layers.
        /// </summary>
        /// <param name="layerIndex">The active layer index.</param>
        private void RefreshLayerColors(int layerIndex)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].layer.color = (i == layerIndex) ? Color.white : layers[i].outOfFocusColor;
            }
        }
        
        public Vector2Int Size { get => gridSize; }
        public Vector2 CellSize { get => cellSize; }
        public Vector2Int SelectedPosition { get => selectedPos; }
        public override int ActiveLayer { get => activeLayerIndex; }
        public override Sprite ActiveLayerSprite { get => GetActiveLayer().sprite; }
    }
}