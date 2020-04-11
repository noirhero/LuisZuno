// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct PropStatusComponent : IComponentData {
    public float search;

    public PropStatusComponent(ref PropStatusComponent rhs) {
        search = rhs.search;
    }
}
