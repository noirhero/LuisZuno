// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct VelocityComponent : IComponentData {
    public float velocity;
}
