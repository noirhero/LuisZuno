// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NoneAvatarStatusComponent : IComponentData {
    public Int32 madness;

    public NoneAvatarStatusComponent(ref NoneAvatarStatusComponent rhs) {
        madness = rhs.madness;
    }
}
