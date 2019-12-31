// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct EntitySpawnComponent : IComponentData {
    public float3 spawnPosition;
    public Entity prefab;
    public int number;
    public float velocityMin;
    public float velocityMax;
    public float posOffsetMin;
    public float posOffsetMax;
}
