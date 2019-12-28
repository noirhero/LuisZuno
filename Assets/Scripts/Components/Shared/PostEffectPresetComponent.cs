// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine.Rendering.PostProcessing;

public struct PostEffectPresetComponent : ISharedComponentData, IEquatable<PostEffectPresetComponent> {
    public PostProcessVolume volume;

    public bool Equals(PostEffectPresetComponent other) {
        return other.volume == volume;
    }

    public override int GetHashCode() {
        return (null == volume) ? 0 : volume.GetHashCode();
    }
}
