// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct PanicComponent : IComponentData {
    public float panicTime;
    public float elapsedPanicTime;
    public AnimationType panicAnim;

    // 패닉 시스템에서 매드니스 컴포넌트를 붙여주기 위한 전달값
    public float madness;
    public float madnessDuration;
}
