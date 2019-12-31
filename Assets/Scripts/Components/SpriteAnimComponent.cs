﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct SpriteAnimComponent : IComponentData {
    public int nameHash;
    public int frame;
    public float accumTime;
}
