// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;
using UnityEngine;

[Serializable]
public struct PlayerPresetComponent : ISharedComponentData, IEquatable<PlayerPresetComponent> {
    public PlayerPreset preset;
    public Sprite sprite;

    public bool Equals(PlayerPresetComponent other) {
        return ReferenceEquals(preset, other.preset);
    }

    public override int GetHashCode() {
        return ReferenceEquals(preset, null) ? 0 : preset.GetHashCode();
    }
}


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

[Serializable]
public struct PlayerStatusComponent : IComponentData {
    [HideInInspector] public CharacterBackground status;
    public float maxMadness;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float madnessWeight;
    [HideInInspector] public float searchingWeight;

    public PlayerStatusComponent(ref PlayerStatusComponent rhs) {
        status = rhs.status;
        maxMadness = rhs.maxMadness;
        moveSpeed = 1.0f;
        madnessWeight = 1.0f;
        searchingWeight = 1.0f;
    }
}
