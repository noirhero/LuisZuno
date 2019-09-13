// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct SpritePresetComponent : ISharedComponentData, IEquatable<SpritePresetComponent> {
    public readonly SpritePreset preset;

    public SpritePresetComponent(SpritePreset inPreset) {
        preset = inPreset;
    }

    public bool Equals(SpritePresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
