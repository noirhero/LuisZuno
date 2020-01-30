// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct SpriteStateComponent : IComponentData {
    public int hash;
    public int oldHash;
    public float frame;
}
