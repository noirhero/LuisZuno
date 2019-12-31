// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct CameraPresetComponent : ISharedComponentData, IEquatable<CameraPresetComponent> {
    private readonly Camera _preset;
    public readonly Transform myTransform;
    public readonly Vector3 defaultPos;

    public CameraPresetComponent(Camera inPreset) {
        _preset = inPreset;
        myTransform = inPreset.transform;
        defaultPos = myTransform.localPosition;
    }

    public bool Equals(CameraPresetComponent other) {
        return other._preset == _preset;
    }

    public override int GetHashCode() {
        return (null == _preset) ? 0 : _preset.GetHashCode();
    }
}
