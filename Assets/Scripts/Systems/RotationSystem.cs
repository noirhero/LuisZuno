// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class RotationSystem : JobComponentSystem {
    [BurstCompile]
    struct RotationSystemJob : IJobForEach<Rotation, MovementComponent> {
        public void Execute(ref Rotation rotation, [ReadOnly] ref MovementComponent moveComp) {
            rotation.Value = (0.0f < moveComp.xValue) ? quaternion.RotateY(math.radians(180.0f)) : quaternion.identity;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new RotationSystemJob();
        return job.Schedule(this, inputDependencies);
    }
}