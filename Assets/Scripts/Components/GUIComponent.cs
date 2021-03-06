﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using GlobalDefine;
using Unity.Entities;

[Serializable]
public struct GUIPresetComponent : ISharedComponentData, IEquatable<GUIPresetComponent> {
    public readonly GUIPreset preset;

    public GUIPresetComponent(GUIPreset inPreset) {
        preset = inPreset;
    }

    public bool Equals(GUIPresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}

[Serializable]
public struct GUIComponent : IComponentData {
    public int currentUI;
}

public struct GUISyncComponent : IComponentData {
    public GUIGroupType currentGroup;

    public GUISyncComponent(GUIGroupType inType) {
        currentGroup = inType;
    }
}
