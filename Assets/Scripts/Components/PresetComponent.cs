// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct PresetComponent : ISharedComponentData, IEquatable<PresetComponent> {
    public int guid;
    public SpritePreset preset;
    public NPCAIPreset npcAIPreset;

    private bool SpritePresetEquals(PresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }
    private bool NPCAIPresetEquals(PresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }
    public bool Equals(PresetComponent other) {
        return SpritePresetEquals(other) && NPCAIPresetEquals(other);
    }

    private int GetSpritePresetHashCode() {
        return ReferenceEquals(null, preset) ? 0 : preset.GetHashCode();
    }
    private int GetNPCAIPresetHashCode() {
        return ReferenceEquals(null, npcAIPreset) ? 0 : npcAIPreset.GetHashCode();
    }
    public override int GetHashCode() {
        return GetSpritePresetHashCode() + GetNPCAIPresetHashCode();
    }
}
