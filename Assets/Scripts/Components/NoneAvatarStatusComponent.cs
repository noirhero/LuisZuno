// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NoneAvatarStatusComponent : IComponentData {
    public Int32 chakra;        // 차크라 ^~^
    public Int32 temptation;    // 이것이 얼마나 갖고 싶게 생겼는가
    public Int32 reactedCount;  // 몇 번이나 상호작용 되었는가
}
