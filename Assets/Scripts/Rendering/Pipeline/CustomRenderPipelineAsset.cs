// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName ="Rendering/CustomPipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset {
    protected override RenderPipeline CreatePipeline() {
        return new CustomRenderPipeline();
    }
}
