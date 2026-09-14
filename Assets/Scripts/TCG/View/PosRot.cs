using UnityEngine;

namespace TCG.View
{
    public readonly struct PosRot
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public PosRot(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}