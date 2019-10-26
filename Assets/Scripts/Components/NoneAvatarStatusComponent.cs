// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NoneAvatarStatusComponent : IComponentData {
    public Int32 madness;
    public Int32 attractiveness;     // 이목을 끄는 수치 (눈에 얼마나 띄는가)
    public Int32 reactionLimitCount; // 상호작용 제한 횟수

    public NoneAvatarStatusComponent(ref NoneAvatarStatusComponent rhs) {
        madness = rhs.madness;
        attractiveness = rhs.attractiveness;
        reactionLimitCount = rhs.reactionLimitCount;
    }
}
