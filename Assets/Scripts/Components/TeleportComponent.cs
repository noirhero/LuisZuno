// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

[Serializable]
public struct TeleportPointComponent : IComponentData {
    public Int64 id;
    public float3 point;
}

[Serializable]
public struct TeleportComponent : IComponentData {
    public SceneType sceneType;
    public int pointID;
    public float elapsedTeleportTime;
    public float teleportTime;
    public float fadeInOutTime;

    public TeleportComponent(ref TeleportInfoComponent rhs) {
        sceneType = rhs.sceneType;
        pointID = rhs.pointID;
        elapsedTeleportTime = 0.0f;
        teleportTime = rhs.teleportTime;
        fadeInOutTime = rhs.fadeInOutTime;
    }
}

[Serializable]
public struct TeleportInfoComponent : IComponentData {
    public SceneType sceneType;
    public int pointID;
    public float teleportTime;
    public float fadeInOutTime;

    public TeleportInfoComponent(SceneType inType, int inPoint, float inTime, float inFadeTime) {
        sceneType = inType;
        pointID = inPoint;
        teleportTime = inTime;
        fadeInOutTime = inFadeTime;
    }
}
