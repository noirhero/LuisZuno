// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct MovementComponent : IComponentData {
    public float3 value;
    public float xValue;

    public MovementComponent(float x) {
        value = float3.zero;
        xValue = x;
    }
}
