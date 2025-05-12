using UnityEngine;

namespace RedRats.UndoRedo.UIElements
{
    /// <summary>
    /// Helper methods for UI Properties.
    /// </summary>
    public static class UIPropertyUtils
    {
        public static void ReleaseAllProperties(this Transform content, bool includeSelf = false)
        {
            IPBase[] p = content.GetComponentsInChildren<IPBase>();
            foreach (IPBase property in p)
            {
                if (includeSelf == false && property.transform == content) continue;
                property.ReleaseToPool();
            }
        }
    }
}