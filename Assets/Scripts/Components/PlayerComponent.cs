// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct PlayerComponent : IComponentData {
    public float playerDirection;        // -1 or 1
}
