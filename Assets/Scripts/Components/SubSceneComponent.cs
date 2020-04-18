// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;

[Serializable]
public struct SubScenePresetComponent : ISharedComponentData, IEquatable<SubScenePresetComponent> {
    public SubScenePreset preset;

    public bool Equals(SubScenePresetComponent other) {
        return ReferenceEquals(preset, other.preset);
    }

    public override int GetHashCode() {
        return ReferenceEquals(preset, null) ? 0 : preset.GetHashCode();
    }
}

[Serializable]
[GenerateAuthoringComponent]
public struct SubSceneControlComponent : IComponentData {
}

[Serializable]
public struct SubSceneLoadComponent : IComponentData {
    public int type;
}

[Serializable]
public struct SubSceneUnLoadComponent : IComponentData {
    public int type;
}
