// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(EndFrameTRSToLocalToWorldSystem))]
public class SpriteTransformSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var transformAndReverseHandle = Entities
            .WithName("SpriteTransformSystem_AndReverse")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref LocalToWorld transform, in SpritePivotComponent pivot, in PlayerComponent player, in Translation pos, in Rotation rot, in NonUniformScale scale) => {
                var spriteInverseRot = quaternion.RotateY(0.0f < player.playerDirection ? math.radians(180.0f) : 0.0f);
                transform.Value = float4x4.TRS(pos.Value + pivot.Value, math.mul(spriteInverseRot, rot.Value), scale.Value);
            })
            .Schedule(inputDependencies);

        return Entities
            .WithName("SpriteTransformSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .WithNone<PlayerComponent>()
            .ForEach((ref LocalToWorld transform, in SpritePivotComponent pivot, in Translation pos, in Rotation rot, in NonUniformScale scale) => {
                transform.Value = float4x4.TRS(pos.Value + pivot.Value, rot.Value, scale.Value);
            })
            .Schedule(transformAndReverseHandle);
    }
}
