// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using GlobalDefine;

public class PostEffectProxy : MonoBehaviour {
    void Start() {
        var postProcessVolume = GetComponent<PostProcessVolume>();
        if (null == postProcessVolume) {
            Debug.LogError($"GameObject({this}) has not PostProcessVolume!!!!!");
            return;
        }

        var postEffectEntity = Utility.entityMng.CreateEntity();
        Utility.entityMng.AddSharedComponentData(postEffectEntity, new PostEffectPresetComponent() {
            volume = postProcessVolume
        });
    }
}
