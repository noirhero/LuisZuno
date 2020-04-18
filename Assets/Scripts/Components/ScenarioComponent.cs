// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct ScenarioClearComponent : IComponentData {

}

[Serializable]
public struct SceneInformationPresetComponent : ISharedComponentData, IEquatable<SceneInformationPresetComponent> {
    public readonly SceneInfomationPreset preset;

    public SceneInformationPresetComponent(SceneInfomationPreset inPreset) {
        preset = inPreset;
    }

    public bool Equals(SceneInformationPresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
