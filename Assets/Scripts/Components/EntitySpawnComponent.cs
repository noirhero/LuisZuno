// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

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
        prefab = rhs.prefab;
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
    public Entity prefab;
    public int number;
    public float velocityMin;
    public float velocityMax;
    public float posOffsetMin;
    public float posOffsetMax;
    public float lifetime;
    public Entity spawnEffect;
    public Entity destroyEffect;

    public EntitySpawnInfoComponent(ref EntitySpawnInfoComponent rhs) {
        prefab = Entity.Null;
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
