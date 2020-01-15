// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using UnityEngine.Rendering.PostProcessing;

public class PostEffectProxy : MonoBehaviour {
    void Start() {
        var postProcessVolume = GetComponent<PostProcessVolume>();
        if (null == postProcessVolume) {
            Debug.LogError($"GameObject({this}) has not PostProcessVolume!!!!!");
            return;
        }

        var postEffectEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddSharedComponentData(postEffectEntity, new PostEffectPresetComponent() {
            volume = postProcessVolume
        });
    }
}
