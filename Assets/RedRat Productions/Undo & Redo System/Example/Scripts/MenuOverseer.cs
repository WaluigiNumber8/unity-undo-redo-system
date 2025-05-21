using RedRats.Core;
using RedRats.DrawingGrid;
using RedRats.DrawingGrid.Tools;
using UnityEngine;

namespace RedRats.Example.Core
{
    /// <summary>
    /// Overseer actions happening in the menu.
    /// </summary>
    public class MenuOverseer : MonoSingleton<MenuOverseer>
    {
        [SerializeField] private InteractableEditorGrid grid;
        [SerializeField] private RectTransform propertiesContent;
        [Space]
        [SerializeField] private Sprite brushSprite;
        
        private ToolBox<int> toolbox;
        private ObjectGrid<int> data;
        private ToolType currentTool;

        protected override void Awake()
        {
            base.Awake();
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
            grid.OnClick += UseTool;
            grid.OnClickAlternative += UseEraser;
        }

        private void OnDisable()
        {
            grid.OnClick -= UseTool;
            grid.OnClickAlternative -= UseEraser;
        }

        public void SwitchTool(ToolType tool)
        {
            currentTool = tool;
        }
        
        private void UseEraser(Vector2Int position) => UseTool(ToolType.Eraser, position);
        private void UseTool(Vector2Int position) => UseTool(currentTool, position);
        private void UseTool(ToolType tool, Vector2Int position)
        {
            Sprite sprite = tool == ToolType.Eraser ? new SpriteBuilder().WithSingleColorTexture(Color.clear, 16, 16).WithPPU(16).Build() : brushSprite;
            toolbox.ApplySpecific(tool, data, position, 0, sprite, grid.ActiveLayer);
        }
    }
}