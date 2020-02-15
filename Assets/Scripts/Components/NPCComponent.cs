// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NPCComponent : IComponentData {
    public float npcDirection;
    public float speed;
    public AnimationType currentAnim;

    public NPCComponent(float initialDir, float inSpeed) {
        npcDirection = initialDir;
        speed = inSpeed;
        currentAnim = AnimationType.Walk;
    }
}
