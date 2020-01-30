// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct NewSpritePresetComponent : ISharedComponentData, IEquatable<NewSpritePresetComponent> {
    public NewSpritePreset preset;

    public bool Equals(NewSpritePresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }

    public override int GetHashCode() {
        return ReferenceEquals(null, preset) ? 0 : preset.GetHashCode();
    }
}
