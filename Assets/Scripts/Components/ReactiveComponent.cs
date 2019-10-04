// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct ReactiveComponent : IComponentData {
    public float colliderSizeX;
    public float colliderSizeY;
    public EntityType type;
}
