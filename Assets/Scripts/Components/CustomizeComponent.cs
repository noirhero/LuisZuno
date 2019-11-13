// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct CustomizeComponent : IComponentData {
    public BackgroundType backgroundType;
    public ValuesType valuesType;
    public GoalType goalType;
}
