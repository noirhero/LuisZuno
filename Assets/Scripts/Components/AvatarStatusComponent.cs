// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct AvatarStatusComponent : IComponentData {
    public Int32 health;
    public Int32 madness;
    public float moveSpeed;
    public Int32 attractiveness;    // 이목을 끄는 수치 (눈에 얼마나 띄는가)
    public Int32 curiosity;         // 호기심

    public AvatarStatusComponent(ref AvatarStatusComponent rhs) {
        health = rhs.health;
        madness = rhs.madness;
        moveSpeed = rhs.moveSpeed;
        attractiveness = rhs.attractiveness;
        curiosity = rhs.curiosity;
    }
}
