// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using Unity.Entities;

[Serializable]
public struct SpriteAnimComponent : IComponentData {
    public int nameHash;
    public int index;
    public int frame;
    public float accumTime;
    public Vector4 rect;
}
