// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct FadeOutComponent: IComponentData {
    public float time;
    public float elapsedTime;
}
