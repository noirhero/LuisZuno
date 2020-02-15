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
        var transformAndPlayerReverseHandle = Entities
            .WithName("PlayerSpriteTransformSystem_AndReverse")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref LocalToWorld transform, in SpritePivotComponent pivot, in PlayerComponent player, in Translation pos, in Rotation rot, in NonUniformScale scale) => {
                var spriteInverseRot = quaternion.RotateY(0.0f < player.playerDirection ? math.radians(180.0f) : 0.0f);
                var rotation = math.mul(spriteInverseRot, rot.Value);
                transform.Value = float4x4.TRS(pos.Value, rotation, scale.Value);

                var rotationApplyPivot = math.mul(rotation, pivot.Value);
                transform.Value.c3 += new float4(rotationApplyPivot, 0.0f);
            })
            .Schedule(inputDependencies);

        var transformAndNPCReverseHandle = Entities
            .WithName("NPCSpriteTransformSystem_AndReverse")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref LocalToWorld transform, in SpritePivotComponent pivot, in NPCComponent npc, in Translation pos, in Rotation rot, in NonUniformScale scale) => {
                var spriteInverseRot = quaternion.RotateY(0.0f < npc.npcDirection ? 0.0f : math.radians(180.0f));
                var rotation = math.mul(spriteInverseRot, rot.Value);
                transform.Value = float4x4.TRS(pos.Value, rotation, scale.Value);

                var rotationApplyPivot = math.mul(rotation, pivot.Value);
                transform.Value.c3 += new float4(rotationApplyPivot, 0.0f);
            })
            .Schedule(transformAndPlayerReverseHandle);

        return Entities
            .WithName("SpriteTransformSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .WithNone<PlayerComponent>()
            .WithNone<NPCComponent>()
            .ForEach((ref LocalToWorld transform, in SpritePivotComponent pivot, in Translation pos, in Rotation rot, in NonUniformScale scale) => {
                transform.Value = float4x4.TRS(pos.Value, rot.Value, scale.Value);
                var rotationApplyPivot = math.mul(rot.Value, pivot.Value);
                transform.Value.c3 += new float4(rotationApplyPivot, 0.0f);
            })
            .Schedule(transformAndNPCReverseHandle);
    }
}
