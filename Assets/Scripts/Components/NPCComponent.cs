// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NPCComponent : IComponentData {
    public float npcDirection;        // -1 or 1
    public AnimationType currentAnim;

    public NPCComponent(float initialDir) {
        npcDirection = initialDir;
        currentAnim = AnimationType.Idle;
    }
}
