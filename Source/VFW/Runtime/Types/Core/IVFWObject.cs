using UnityEngine;

namespace Vexe.Runtime.Types
{
    /// <summary>
    /// Implement this interface in your <see cref="MonoBehaviour"/>s and <see cref="ScriptableObject"/>s to get the full editor power of Vfw
    /// This interface does not define any custom serialization so it has no extra runtime overhead
    /// </summary>
    public interface IVFWObject
    {
        int GetPersistentId();

        CategoryDisplay GetDisplayOptions();

#if UNITY_EDITOR
        EditorRecord Prefs { get; set; }

        // This field is also required for implementers.
        // [HideInInspector] public bool dbg;
#endif
    }
}