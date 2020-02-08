// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Mathematics;
using Unity.Entities;


[Serializable]
public struct CameraSyncComponent : IComponentData {
    public float3 syncPos;

    public CameraSyncComponent(float3 inPos) {
        syncPos = inPos;
    }
}
