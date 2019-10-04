// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using System;
using Unity.Entities;

[Serializable]
public struct TargetComponent : IComponentData {
    public int targetIndex;
    public float targetDistance;
}
