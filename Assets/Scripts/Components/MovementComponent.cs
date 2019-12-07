// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MovementComponent : IComponentData {
    public float3 value;
    public float xValue;
    public int targetIndex;

    public MovementComponent(int targetIndex) {
        value = float3.zero;
        xValue = 0;
        targetIndex = targetIndex;
    }
}
