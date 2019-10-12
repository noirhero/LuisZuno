// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct CameraComponent : ISharedComponentData, IEquatable<CameraComponent> {
    public readonly Camera preset;
    public readonly Transform myTransform;
    public readonly Vector3 defaultPos;
    public readonly float velocity;

    public CameraComponent(Camera inPreset, float inVelocity) {
        preset = inPreset;
        velocity = inVelocity;
        myTransform = inPreset.transform;
        defaultPos = inPreset.transform.localPosition;
    }

    public bool Equals(CameraComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
