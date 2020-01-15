// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct FadeInComponent: IComponentData {
    public float time;
    public float elapsedTime;

    public FadeInComponent(FadeInComponent rhs) {
        time = rhs.time;
        elapsedTime = 0.0f;
    }

    public FadeInComponent(float t) {
        time = t;
        elapsedTime = 0.0f;
    }
}
