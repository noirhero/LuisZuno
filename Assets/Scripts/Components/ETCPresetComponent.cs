// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
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
public struct TablePresetComponent : ISharedComponentData, IEquatable<TablePresetComponent> {
    public readonly TablePreset preset;

    public TablePresetComponent(TablePreset inPreset) {
        preset = inPreset;
    }

    public bool Equals(TablePresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
