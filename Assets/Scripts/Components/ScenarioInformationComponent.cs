// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct ScenarioInformationPresetComponent : ISharedComponentData, IEquatable<ScenarioInformationPresetComponent> {
    public readonly ScenarioInfomationPreset preset;

    public ScenarioInformationPresetComponent(ScenarioInfomationPreset inPreset) {
        preset = inPreset;
    }

    public bool Equals(ScenarioInformationPresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
