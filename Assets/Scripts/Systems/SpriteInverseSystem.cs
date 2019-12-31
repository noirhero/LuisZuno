// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpriteInverseSystem : JobComponentSystem {
    [BurstCompile]
    struct SpriteInverseSystemJob : IJobForEach<Rotation, PlayerComponent> {
        public void Execute(ref Rotation rotation, [ReadOnly] ref PlayerComponent playerComp) {
            rotation.Value = (0.0f < playerComp.playerDirection) ? quaternion.RotateY(math.radians(180.0f)) : quaternion.identity;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new SpriteInverseSystemJob();
        return job.Schedule(this, inputDependencies);
    }
}
