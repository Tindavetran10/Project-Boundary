using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float SpeedModifier { get; private set; } = 0.25f;

    [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }

}
