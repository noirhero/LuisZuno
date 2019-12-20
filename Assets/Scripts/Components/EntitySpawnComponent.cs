// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct EntitySpawnComponent : IComponentData {
    public Entity prefab;
    public int spawnNumber;
    public float3 spawnPosition;
    public float3 spawnDirection;
    //public int spawnLimit;
}
