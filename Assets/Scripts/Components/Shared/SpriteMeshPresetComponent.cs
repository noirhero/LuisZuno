// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine;

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
