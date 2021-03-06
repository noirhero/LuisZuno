﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine.Serialization;

[Serializable]
public struct EffectSpawnComponent : IComponentData {
    public Entity prefab;
    public float lifetime;
    public float duration;
}

[Serializable]
public struct EntitySpawnComponent : IComponentData {
    public Entity prefab;
    public int number;
    public float velocityMin;
    public float velocityMax;
    public float posOffsetMin;
    public float posOffsetMax;
    public float lifetime;
    public Entity spawnEffect;
    public Entity destroyEffect;

    public EntitySpawnComponent(ref EntitySpawnInfoComponent rhs) {
        prefab = rhs.preset;
        number = rhs.number;
        velocityMin = rhs.velocityMin;
        velocityMax = rhs.velocityMax;
        posOffsetMin = rhs.posOffsetMin;
        posOffsetMax = rhs.posOffsetMax;
        lifetime = rhs.lifetime;
        spawnEffect = rhs.spawnEffect;
        destroyEffect = rhs.destroyEffect;
    }
}

[Serializable]
public struct EntitySpawnInfoComponent : IComponentData {
    public Entity preset;
    public int number;
    public float velocityMin;
    public float velocityMax;
    public float posOffsetMin;
    public float posOffsetMax;
    public float lifetime;
    public Entity spawnEffect;
    public Entity destroyEffect;

    public EntitySpawnInfoComponent(ref EntitySpawnInfoComponent rhs) {
        preset = Entity.Null;
        number = rhs.number;
        velocityMin = rhs.velocityMin;
        velocityMax = rhs.velocityMax;
        posOffsetMin = rhs.posOffsetMin;
        posOffsetMax = rhs.posOffsetMax;
        lifetime = rhs.lifetime;
        spawnEffect = Entity.Null;
        destroyEffect = Entity.Null;
    }
}

[Serializable]
public struct LifeCycleComponent : IComponentData {
    public Entity spawnEffect;
    public Entity destroyEffect;
    public float lifetime;
    public float duration;
}
