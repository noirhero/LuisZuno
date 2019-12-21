// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct NoneAvatarStatusComponent : IComponentData {
    public float madness;
    public float search;

    public NoneAvatarStatusComponent(ref NoneAvatarStatusComponent rhs) {
        madness = rhs.madness;
        search = rhs.search;
    }
}
