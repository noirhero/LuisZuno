// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct PanicComponent : IComponentData {
    public float panicTime;
    public AnimationType panicAnim;
    public float elapsedPanicTime;

    public PanicComponent(float time, AnimationType anim) {
        panicTime = time;
        panicAnim = anim;
        elapsedPanicTime = 0.0f;
    }
}
