// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct AvatarStatusComponent : IComponentData {
    public Int32 chakra;        // 차크라 ^~^
    public Int32 temptation;    // 이것이 얼마나 갖고 싶게 생겼는가
    public Int32 sane;          // 현재 이성 수치
    public Int32 curiosity;     // 호기심 수치
    public bool bInPanic;       // 패닉 상태
}
