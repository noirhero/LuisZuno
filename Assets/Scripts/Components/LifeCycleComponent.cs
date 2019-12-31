// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct LifeCycleComponent : IComponentData {
    public Entity spawnEffect;
    public Entity destroyEffect;
    public float lifetime;
    public float duration;
}
