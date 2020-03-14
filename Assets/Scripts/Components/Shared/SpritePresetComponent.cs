// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct SpritePresetComponent : ISharedComponentData, IEquatable<SpritePresetComponent> {
    public SpritePreset preset;

    public bool Equals(SpritePresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }

    public override int GetHashCode() {
        return ReferenceEquals(null, preset) ? 0 : preset.GetHashCode();
    }
}
