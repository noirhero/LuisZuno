// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline {
    ScriptableCullingParameters cullParam = new ScriptableCullingParameters() {
        isOrthographic = true
    };

    DrawingSettings opqaueDrawing = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"), new SortingSettings() {
        criteria = SortingCriteria.CommonOpaque
    }) {
        enableDynamicBatching = true,
        enableInstancing = true
    };
    FilteringSettings opaqueFiltering = new FilteringSettings(RenderQueueRange.opaque);

    DrawingSettings transparentDrawing = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"), new SortingSettings() {
        criteria = SortingCriteria.CommonTransparent
    }) {
        enableDynamicBatching = true,
        enableInstancing = true
    };
    FilteringSettings transparentFiltering = new FilteringSettings(RenderQueueRange.transparent);

    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        BeginFrameRendering(context, cameras);

        foreach (var camera in cameras) {
            BeginCameraRendering(camera);

            if (false == camera.TryGetCullingParameters(out cullParam)) {
                continue;
            }

#if UNITY_EDITOR
            if (CameraType.SceneView == camera.cameraType) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
#endif

            var cullResults = context.Cull(ref cullParam);

            context.SetupCameraProperties(camera);

            var cmdBuf = CommandBufferPool.Get("MainCamera");
            cmdBuf.ClearRenderTarget(true, true, camera.backgroundColor);
            context.ExecuteCommandBuffer(cmdBuf);
            cmdBuf.Clear();

            context.DrawRenderers(cullResults, ref opqaueDrawing, ref opaqueFiltering);
            context.DrawRenderers(cullResults, ref transparentDrawing, ref transparentFiltering);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);

            context.Submit();

            EndCameraRendering(context, camera);
        }

        EndFrameRendering(context, cameras);
    }
}
