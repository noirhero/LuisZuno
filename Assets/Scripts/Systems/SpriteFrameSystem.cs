// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

public class SpriteFrameSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var deltaTime = Time.DeltaTime;
        return Entities
            .WithName("SpriteFrameSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteStateComponent state) => {
                state.frame = (state.oldHash != state.hash) ? deltaTime : state.frame + deltaTime;
                state.oldHash = state.hash;
            })
            .Schedule(inputDependencies);
    }
}
