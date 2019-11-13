// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class SpriteDrawSystem : ComponentSystem {
    private static readonly int _mainTexUv = Shader.PropertyToID("_MainTex_UV");
    private static readonly int _mainTex = Shader.PropertyToID("_MainTex");

    protected override void OnUpdate() {
        var propertyBlock = new MaterialPropertyBlock();
        var uv = new Vector4[1];
        float3 drawPos;

        Entities.ForEach((SpritePresetComponent presetComp, ref SpriteAnimComponent animComp, ref Rotation rotation, ref Translation pos) => {
            if (false == presetComp.preset.datas.TryGetValue(animComp.nameHash, out var presetData)) {
                return;
            }

            uv[0] = presetData.rects[animComp.frame];
            propertyBlock.SetVectorArray(_mainTexUv, uv);
            propertyBlock.SetTexture(_mainTex, presetData.texture);

            drawPos = pos.Value + presetData.posOffset;
            //drawPos.z = pos.Value.y;

            Graphics.DrawMesh(
                presetComp.preset.mesh,
                float4x4.TRS(drawPos, rotation.Value, presetData.scale),
                presetComp.preset.material,
                0 /*layer*/,
                Camera.main,
                0 /*sub mesh index*/,
                propertyBlock);
        });
    }
}
