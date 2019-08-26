// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class SpriteDrawSystem : ComponentSystem {
    protected override void OnUpdate() {
        var mtrlPropBlock = new MaterialPropertyBlock();
        var uv = new Vector4[1];
        var drawPos = new float3();

        Entities.ForEach((SpritePresetComponent presetComp, ref SpriteAnimComponent animComp, ref Rotation rotation, ref Translation pos) => {
            if (false == presetComp.preset.datas.TryGetValue(animComp.nameHash, out SpritePresetData presetData)) {
                return;
            }

            uv[0] = presetData.rects[animComp.frame];
            mtrlPropBlock.SetVectorArray("_MainTex_UV", uv);
            mtrlPropBlock.SetTexture("_MainTex", presetData.texture);

            drawPos = pos.Value + presetData.posOffset;
            drawPos.z = pos.Value.y;

            Graphics.DrawMesh(
                presetComp.preset.mesh,
                float4x4.TRS(drawPos, rotation.Value, presetData.scale),
                presetComp.preset.material,
                0/*layer*/,
                Camera.main,
                0/*sub mesh index*/,
                mtrlPropBlock);
        });
    }
}
