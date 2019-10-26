// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct ReactiveComponent : IComponentData {
    public EntityType type;
    public float reactionTime;
    public Int32 reactionLimitCount;
    private float reactionElapsedTime;
    private Int32 reactedCount;

    public float ReactionElapsedTime {
        get { return reactionElapsedTime; }
        set { reactionElapsedTime = value; }
    }

    public Int32 ReactedCount {
        get { return reactedCount; }
        set { reactedCount = value; }
    }
}
