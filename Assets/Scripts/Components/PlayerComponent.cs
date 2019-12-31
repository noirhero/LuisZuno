// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct PlayerComponent : IComponentData {
    public float playerDirection;        // -1 or 1
    public int currentBehaviors;		 // GlobalDefine.BehaviorState 참고
    public AnimationType currentAnim;

    public PlayerComponent(float initialDir) {
        playerDirection = initialDir;
        currentBehaviors = 0;
        currentAnim = AnimationType.Idle;
    }
}
