using RedRats.ActionHistory;
using RedRats.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RedRats.Input
{
    /// <summary>
    /// Overseers all input profiles and deals with their switching.
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public class InputSystem : MonoSingleton<InputSystem>
    {
        private TestInputActions input;
        
        private InputProfileUI inputUI;
        private Vector2 pointerPosition;

        protected override void Awake()
        {
            base.Awake();
            ClearAllInput();
        }

        public void ClearAllInput()
        {
            UI?.Disable();
            input = new TestInputActions();
            inputUI = new InputProfileUI(input);
            UI!.Enable();
            
            UI.PointerPosition.OnPressed += UpdatePointerPosition;
            
            //Force grouping on click/right click
            UI.Select.OnPress -= ActionHistorySystem.StartNewGroup;
            UI.ContextSelect.OnPress -= ActionHistorySystem.StartNewGroup;
            UI.Select.OnRelease -= ActionHistorySystem.EndCurrentGroup;
            UI.ContextSelect.OnRelease -= ActionHistorySystem.EndCurrentGroup;
            
            UI.Select.OnPress += ActionHistorySystem.StartNewGroup;
            UI.ContextSelect.OnPress += ActionHistorySystem.StartNewGroup;
            UI.Select.OnRelease += ActionHistorySystem.EndCurrentGroup;
            UI.ContextSelect.OnRelease += ActionHistorySystem.EndCurrentGroup;
        }
        
        private void UpdatePointerPosition(Vector2 value) => pointerPosition = value;
        
        public Vector2 PointerPosition { get => pointerPosition; }
        public InputProfileUI UI { get => inputUI; }
        public string KeyboardBindingGroup { get => input.KeyboardMouseScheme.bindingGroup; }
        public string GamepadBindingGroup { get => input.GamepadScheme.bindingGroup;}
    }
}