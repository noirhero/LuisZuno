// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using GlobalDefine;

[UpdateAfter(typeof(AutoMovementSystem))]
public class EffectSpawnSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;
    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent();
        var jobHandle = Entities
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, ref EffectSpawnComponent effect, in Translation pos) => {
                cmdBuf.RemoveComponent<EffectSpawnComponent>(entityInQueryIndex, entity);
                Utility.SetLifeCycle(entityInQueryIndex, ref cmdBuf, ref entity, 0.0f);

                var effectEntity = cmdBuf.Instantiate(entityInQueryIndex, effect.prefab);
                cmdBuf.SetComponent(entityInQueryIndex, effectEntity, pos);
                Utility.SetLifeCycle(entityInQueryIndex, ref cmdBuf, ref effectEntity, effect.lifetime);
            })
            .Schedule(inputDependencies);
        _cmdSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
