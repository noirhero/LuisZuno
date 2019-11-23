// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct MadnessComponent : IComponentData {
    public float totalTransitionValue;
    public float transitionStartMadness;
    public float transitionAccel;
    public float elapsedTransitionTime;
}
