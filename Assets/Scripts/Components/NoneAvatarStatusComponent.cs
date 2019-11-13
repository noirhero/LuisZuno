// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct NoneAvatarStatusComponent : IComponentData {
    public int madness;

    public NoneAvatarStatusComponent(ref NoneAvatarStatusComponent rhs) {
        madness = rhs.madness;
    }
}
