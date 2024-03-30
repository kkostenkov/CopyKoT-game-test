using DYP;
using UnityEngine;

namespace Controllers.Modules
{
    [System.Serializable]
    class OnLadderState
    {
        public bool IsInLadderArea = false;
        public Bounds Area = new Bounds(Vector3.zero, Vector3.zero);
        public Bounds BottomArea = new Bounds(Vector3.zero, Vector3.zero);
        public Bounds TopArea = new Bounds(Vector3.zero, Vector3.zero);
        public LadderZone AreaZone = LadderZone.Bottom;

        public bool HasRestrictedArea = false;
        public Bounds RestrictedArea = new Bounds();
        public Vector2 RestrictedAreaTopRight = new Vector2(Mathf.Infinity, Mathf.Infinity);
        public Vector2 RestrictedAreaBottomLeft = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
    }
}