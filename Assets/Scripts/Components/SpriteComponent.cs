// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct SpritePivotComponent : IComponentData {
    public float3 Value;
}

[Serializable]
public struct SpriteStateComponent : IComponentData {
    public int hash;
    public int oldHash;
    public float frame;
}

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

[Serializable]
public struct SpriteMeshPresetComponent : ISharedComponentData, IEquatable<SpriteMeshPresetComponent> {
    public Mesh mesh;
    public Material material;

    public bool Equals(SpriteMeshPresetComponent other) {
        return ReferenceEquals(mesh, other.mesh) && ReferenceEquals(material, other.material);
    }

    public override int GetHashCode() {
        return (ReferenceEquals(mesh, null) ? 0 : mesh.GetHashCode()) ^
            (ReferenceEquals(material, null) ? 0 : material.GetHashCode());
    }
}
