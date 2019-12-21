// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(AutoMovementSystem))]
public class EffectSpawnSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;

    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        Enabled = false;
    }

    [ExcludeComponent(typeof(EffectSpawnExistComponent))]
    private struct EffectSpawnSystemJob : IJobForEachWithEntity<EffectSpawnComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, ref EffectSpawnComponent effectComp, [ReadOnly] ref Translation posComp) {
            if (Entity.Null == effectComp.prefab) {
                return;
            }

            var effectEntity = cmdBuf.Instantiate(index, effectComp.prefab);
            cmdBuf.SetComponent(index, effectEntity, posComp);
            cmdBuf.AddComponent<EffectSpawnExistComponent>(index, entity);
            cmdBuf.AddComponent(index, effectEntity, new LifeCycleComponent() {
                spawnEffect = Entity.Null,
                destroyEffect = Entity.Null,
                lifetime = 0.4f,
                duration = 0.0f,
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
