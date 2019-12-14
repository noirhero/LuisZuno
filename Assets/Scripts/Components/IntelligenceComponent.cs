﻿// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct IntelligenceComponent : IComponentData {
    public int targetEntityIndex;
    public bool hasDoneSettingCopmonents;

    public IntelligenceComponent(int targetIndex) {
        targetEntityIndex = targetIndex;
        hasDoneSettingCopmonents = false;
    }
}
