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
        Enabled = false;
    }
    
    private struct EffectSpawnSystemJob : IJobForEachWithEntity<EffectSpawnComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, ref EffectSpawnComponent effectComp, [ReadOnly] ref Translation posComp) {
            cmdBuf.RemoveComponent<EffectSpawnComponent>(index, entity);
            cmdBuf.AddComponent(index, entity, new LifeCycleComponent() {
                spawnEffect = Entity.Null,
                destroyEffect = Entity.Null,
                lifetime = 0.0f,
                duration = 0.0f,
            });

            var effectEntity = cmdBuf.Instantiate(index, effectComp.prefab);
            cmdBuf.SetComponent(index, effectEntity, posComp);
            cmdBuf.AddComponent(index, effectEntity, new LifeCycleComponent() {
                spawnEffect = Entity.Null,
                destroyEffect = Entity.Null,
                lifetime = effectComp.lifetime,
                duration = effectComp.duration,
            });
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
