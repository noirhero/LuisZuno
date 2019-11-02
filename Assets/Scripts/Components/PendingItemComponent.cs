// Copyright 2018-2019 TAP, Inc. All Rights Resered.

using System;
using Unity.Entities;

[Serializable]
public struct PendingItemComponent : IComponentData {
    public Int64 pendingItemID;
}
