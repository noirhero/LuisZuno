// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Mathematics;
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

[Serializable]
public struct CameraSyncComponent : IComponentData {
    public float3 syncPos;

    public CameraSyncComponent(float3 inPos) {
        syncPos = inPos;
    }
}

[Serializable]
public struct FadeInComponent : IComponentData {
    public float time;
    public float elapsedTime;

    public FadeInComponent(FadeInComponent rhs) {
        time = rhs.time;
        elapsedTime = 0.0f;
    }

    public FadeInComponent(float t) {
        time = t;
        elapsedTime = 0.0f;
    }
}

[Serializable]
public struct FadeOutComponent : IComponentData {
    public float time;
    public float elapsedTime;

    public FadeOutComponent(FadeOutComponent rhs) {
        time = rhs.time;
        elapsedTime = 0.0f;
    }

    public FadeOutComponent(float t) {
        time = t;
        elapsedTime = 0.0f;
    }
}
