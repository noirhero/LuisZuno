// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;
using UnityEngine;

[Serializable]
public struct SceneInfoComponent : IComponentData {
    public TeleportPointStruct[] teleportPoints;
    [HideInInspector] public SubSceneType subSceneType;
}
