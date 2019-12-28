// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct SearchingComponent : IComponentData {
    public float searchingTime;
    public float elapsedSearchingTime;
    public AnimationType searchingAnim;
}
