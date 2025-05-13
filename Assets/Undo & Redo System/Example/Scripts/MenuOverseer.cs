using RedRats.Core;
using RedRats.DrawingGrid;
using RedRats.DrawingGrid.Tools;
using UnityEngine;

namespace RedRats.Example.Core
{
    /// <summary>
    /// Overseer actions happening in the menu.
    /// </summary>
    public class MenuOverseer : MonoBehaviour
    {
        [SerializeField] private InteractableEditorGrid grid;
        [SerializeField] private RectTransform propertiesContent;
        [Space]
        [SerializeField] private Color brushColor = Color.cornflowerBlue;

        [SerializeField] private Sprite brushSprite;
        
        
        private ToolBox<int> toolbox;
        private ObjectGrid<int> data;

        private void Awake()
        {
            toolbox = new ToolBox<int>(grid, grid.UpdateCell, -1);
            data = new ObjectGrid<int>(grid.Size.x, grid.Size.y, () => -1);
        }

        private async void Start()
        {
            PropertyColumnBuilder b = new(propertiesContent);
            await Awaitable.NextFrameAsync();
            b.Build();       
        }

        private void OnEnable()
        {
            grid.OnClick += UseBrush;
            grid.OnClickAlternative += UseEraser;
        }

        private void OnDisable()
        {
            grid.OnClick -= UseBrush;
            grid.OnClickAlternative -= UseEraser;
        }

        public void UseBrush(Vector2Int position)
        {
            toolbox.ApplySpecific(ToolType.Brush, data, position, 0, brushSprite, grid.ActiveLayer);
        }
        
        public void UseEraser(Vector2Int position)
        {
            Sprite emptySprite = new SpriteBuilder().WithSingleColorTexture(Color.clear, 16, 16).WithPPU(16).Build();
            toolbox.ApplySpecific(ToolType.Eraser, data, position, 0, emptySprite, grid.ActiveLayer);
        }
    }
}