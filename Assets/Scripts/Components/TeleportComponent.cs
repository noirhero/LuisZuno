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
    public ScenarioType scenarioType;
    public int pointID;
    public float elapsedTeleportTime;
    public float teleportTime;
    public float fadeInOutTime;

    public TeleportComponent(ref TeleportInfoComponent rhs) {
        scenarioType = rhs.scenarioType;
        pointID = rhs.pointID;
        elapsedTeleportTime = 0.0f;
        teleportTime = rhs.teleportTime;
        fadeInOutTime = rhs.fadeInOutTime;
    }
}

[Serializable]
public struct TeleportInfoComponent : IComponentData {
    public ScenarioType scenarioType;
    public int pointID;
    public float teleportTime;
    public float fadeInOutTime;

    public TeleportInfoComponent(ScenarioType inType, int inPoint, float inTime, float inFadeTime) {
        scenarioType = inType;
        pointID = inPoint;
        teleportTime = inTime;
        fadeInOutTime = inFadeTime;
    }
}
