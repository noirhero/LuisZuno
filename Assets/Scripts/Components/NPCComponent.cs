// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NPCComponent : IComponentData {
    public float npcDirection;
    public float speed;
    public AnimationType currentAnim;
    private float _aiElapsedTime;
    private int _aiCurrentIndex;

    public float AIElapsedTIme {
        get => _aiElapsedTime;
        set => _aiElapsedTime = value;
    }

    public int AICurrentIndex {
        get => _aiCurrentIndex;
        set => _aiCurrentIndex = value;
    }

    public NPCComponent(float initialDir, float inSpeed) {
        npcDirection = initialDir;
        speed = inSpeed;
        currentAnim = AnimationType.Walk;
        _aiElapsedTime = 0.0f;
        _aiCurrentIndex = 0;
    }
}
