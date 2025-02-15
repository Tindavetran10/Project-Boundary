using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerArcherCapsuleColliderUtility : CapsuleColliderUtility
{
    [field: SerializeField] public PlayerTriggerColliderData TriggerColliderData { get; private set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        TriggerColliderData.Initialize();
    }
}
