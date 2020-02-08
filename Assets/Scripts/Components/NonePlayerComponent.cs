// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NonePlayerComponent : IComponentData {
    public AnimationType currentAnim;
}
