// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public struct SpriteMeshComponent : ISharedComponentData, IEquatable<SpriteMeshComponent> {
    public Mesh mesh;
    public Material material;
    public Texture[] textures;

    public NativeArray<int> nameHashes;
    public NativeArray<float> lengths;
    public NativeArray<float> frameRates;
    public NativeArray<int> offsets;
    public NativeArray<int> counts;
    public NativeArray<float3> scales;
    public NativeArray<float3> posOffsets;

    public NativeArray<Vector4> rects;

    public bool Equals(SpriteMeshComponent other) {
        int hash = 0;
        if(null != nameHashes) {
            foreach (var nameHash in nameHashes) {
                hash += nameHash;
            }
        }

        int otherHash = 0;
        foreach(var nameHash in other.nameHashes) {
            otherHash += nameHash;
        }

        return hash == otherHash;
    }

    public override int GetHashCode() {
        int hash = 0;
        foreach (var nameHash in nameHashes) {
            hash += nameHash;
        }
        return hash;
    }
}
