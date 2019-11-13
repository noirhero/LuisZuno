// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct ReactiveComponent : IComponentData {
    public EntityType type;
    public float reactionTime;
    public int reactionLimitCount;
    private float _reactionElapsedTime;
    private int _reactedCount;

    public float ReactionElapsedTime {
        get => _reactionElapsedTime;
        set => _reactionElapsedTime = value;
    }

    public int ReactedCount {
        get => _reactedCount;
        set => _reactedCount = value;
    }
}
