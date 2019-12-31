// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MovementComponent : IComponentData {
    public float3 value;
    public int targetEntityIndex;

    public MovementComponent(int targetIndex) {
        value = float3.zero;
        targetEntityIndex = targetIndex;
    }
}
