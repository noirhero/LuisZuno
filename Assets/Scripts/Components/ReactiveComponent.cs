// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct ReactiveComponent : IComponentData {
    // searching
    public float searchingTime;
    public AnimationType searchingAnim;

    // panic
    public float panicTime;
    public AnimationType panicAnim;

    // madness
    public float madness;
    public float madnessDuration;

    // items
    public Int64 itemID;

    public ReactiveComponent(ref ReactiveComponent rhs) {
        searchingTime = rhs.searchingTime;
        searchingAnim = rhs.searchingAnim;
        panicTime = rhs.panicTime;
        panicAnim = rhs.panicAnim;
        madness = rhs.madness;
        madnessDuration = rhs.madnessDuration;
        itemID = rhs.itemID;
    }
}
