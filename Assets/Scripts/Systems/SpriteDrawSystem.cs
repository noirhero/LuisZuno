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

        Entities.ForEach((SpriteMeshComponent meshComp, ref SpriteAnimComponent animComp, ref Rotation rotation, ref Translation pos) => {
            uv[0] = animComp.rect;
            mtrlPropBlock.SetVectorArray("_MainTex_UV", uv);
            mtrlPropBlock.SetTexture("_MainTex", meshComp.textures[animComp.index]);

            drawPos = pos.Value + meshComp.posOffsets[animComp.index];
            drawPos.z = pos.Value.y;

            Graphics.DrawMesh(
                meshComp.mesh,
                float4x4.TRS(drawPos, rotation.Value, meshComp.scales[animComp.index]),
                meshComp.material,
                0/*layer*/,
                Camera.main,
                0/*sub mesh index*/,
                mtrlPropBlock);
        });
    }

    protected override void OnDestroyManager() {
        base.OnDestroyManager();

        Entities.ForEach((SpriteMeshComponent meshComp) => {
            meshComp.nameHashes.Dispose();
            meshComp.lengths.Dispose();
            meshComp.frameRates.Dispose();
            meshComp.offsets.Dispose();
            meshComp.counts.Dispose();
            meshComp.scales.Dispose();
            meshComp.posOffsets.Dispose();
            meshComp.rects.Dispose();
        });
    }
}
