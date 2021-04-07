using System;
using UnityEngine;

namespace Config
{
    public class MapSettings : MonoBehaviour
    {
        public MapConfigScriptable configScriptable;

        public static MapSettings Instance;

        private void Awake()
        {
            if (Instance != this) Instance = this;
        }
    }
}
