using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Map Config", menuName = "Map/Map Config", order = 0)]
    public class MapConfigScriptable : ScriptableObject
    {
        [Header("Debug Settings")]
        public bool flyCam = false;
        public bool lockYToSpline = false;
        public bool disconnectAi = false;
    }
}
