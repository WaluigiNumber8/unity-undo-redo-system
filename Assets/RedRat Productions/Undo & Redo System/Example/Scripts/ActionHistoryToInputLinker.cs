using RedRats.ActionHistory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedRats.Example.Core
{
    public class ActionHistoryToInputLinker : MonoBehaviour
    {
        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
            {
                ActionHistorySystem.StartNewGroup();
                return;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
            {
                ActionHistorySystem.EndCurrentGroup();
                return;
            }
        }
    }
}