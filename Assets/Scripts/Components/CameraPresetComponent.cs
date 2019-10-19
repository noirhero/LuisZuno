// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct CameraPresetComponent : ISharedComponentData, IEquatable<CameraPresetComponent> {
    public readonly Camera preset;
    public readonly Transform myTransform;
    public readonly Vector3 defaultPos;

    public CameraPresetComponent(Camera inPreset) {
        preset = inPreset;
        myTransform = inPreset.transform;
        defaultPos = inPreset.transform.localPosition;
    }

    public bool Equals(CameraPresetComponent other) {
        return other.preset == preset;
    }

    public override int GetHashCode() {
        return (null == preset) ? 0 : preset.GetHashCode();
    }
}
