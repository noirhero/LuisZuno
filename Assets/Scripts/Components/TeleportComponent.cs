// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[Serializable]
public struct TeleportComponent : IComponentData {
    public float elapsedTeleportTime;
    public float teleportTime;
    public Translation destination;
    public Entity startEffect;
    public Entity endEffect;

    public TeleportComponent(ref TeleportInfoComponent rhs) {
        elapsedTeleportTime = 0.0f;
        teleportTime = rhs.teleportTime;
        destination = rhs.destination;
        startEffect = rhs.startEffect;
        endEffect = rhs.endEffect;
    }
}

[Serializable]
public struct TeleportInfoComponent : IComponentData {
    public float teleportTime;
    public Translation destination;
    public Entity startEffect;
    public Entity endEffect;

    public TeleportInfoComponent(float3 inDest, float inTime) {
        teleportTime = inTime;
        destination.Value = inDest;
        startEffect = Entity.Null;
        endEffect = Entity.Null;
    }
}
