// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using GlobalDefine;

[Serializable]
public struct NPCAIPresetComponent : ISharedComponentData, IEquatable<NPCAIPresetComponent> {
    public NPCAIPreset preset;
    private int _currentIndex;

    public int CurrentIndex {
        get => _currentIndex;
        set => _currentIndex = value;
    }

    public bool Equals(NPCAIPresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }

    public override int GetHashCode() {
        return ReferenceEquals(null, preset) ? 0 : preset.GetHashCode();
    }
}
