// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

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

        var postEffectEntity = World.Active.EntityManager.CreateEntity();
        World.Active.EntityManager.AddSharedComponentData(postEffectEntity, new PostEffectPresetComponent() {
            volume = postProcessVolume
        });
    }
}
