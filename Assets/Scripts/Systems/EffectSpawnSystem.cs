// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Collections;
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
    
    private struct EffectSpawnSystemJob : IJobForEachWithEntity<EffectSpawnComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, ref EffectSpawnComponent effectComp, [ReadOnly] ref Translation posComp) {
            cmdBuf.RemoveComponent<EffectSpawnComponent>(index, entity);
            Utility.SetLifeCycle(index, ref cmdBuf, ref entity, 0.0f);

            var effectEntity = cmdBuf.Instantiate(index, effectComp.prefab);
            cmdBuf.SetComponent(index, effectEntity, posComp);
            Utility.SetLifeCycle(index, ref cmdBuf, ref effectEntity, effectComp.lifetime);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new EffectSpawnSystemJob() {
            cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent()
        };
        var handle = job.Schedule(this, inputDependencies);
        _cmdSystem.AddJobHandleForProducer(handle);

        return handle;
    }
}
