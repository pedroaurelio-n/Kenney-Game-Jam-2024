using System;
using UnityEngine;

[Serializable]
public struct RecordedTransform
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }

    public RecordedTransform (
        Vector3 position,
        Quaternion rotation
    )
    {
        Position = position;
        Rotation = rotation;
    }
}
