// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[Serializable]
public struct TeleportComponent : IComponentData {
    public Translation destination;
    public Entity startEffect;
    public Entity endEffect;

    public TeleportComponent(ref TeleportInfoComponent rhs) {
        destination = rhs.destination;
        startEffect = rhs.startEffect;
        endEffect = rhs.endEffect;
    }
}

[Serializable]
public struct TeleportInfoComponent : IComponentData {
    public Translation destination;
    public Entity startEffect;
    public Entity endEffect;

    public TeleportInfoComponent(float3 inDest) {
        destination.Value = inDest;
        startEffect = Entity.Null;
        endEffect = Entity.Null;
    }
}
