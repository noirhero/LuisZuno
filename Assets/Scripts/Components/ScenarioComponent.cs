// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct ScenarioClearComponent : IComponentData {
    public TeleportInfoComponent teleportInfoComp;
}

[Serializable]
public struct SceneInformationPresetComponent : ISharedComponentData, IEquatable<SceneInformationPresetComponent> {
    public SceneInfomationPreset preset;

    public bool Equals(SceneInformationPresetComponent other) {
        return ReferenceEquals(other.preset, preset);
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
