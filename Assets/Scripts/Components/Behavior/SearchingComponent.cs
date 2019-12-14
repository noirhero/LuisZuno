// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct SearchingComponent : IComponentData {
    public float searchingTime;
    public AnimationType searchingAnim;
    public float elapsedSearchingTime;

    public SearchingComponent(SearchingComponent rhs) {
        searchingTime = rhs.searchingTime;
        searchingAnim = rhs.searchingAnim;
        elapsedSearchingTime = 0.0f;
    }
}
