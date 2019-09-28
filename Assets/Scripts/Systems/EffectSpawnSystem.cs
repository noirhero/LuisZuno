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
    }

    [ExcludeComponent(typeof(EffectSpawnExistComponent))]
    private struct EffectSpawnSystemJob : IJobForEachWithEntity<MovementComponent, Translation> {
        [ReadOnly] public Entity prefab;
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, [ReadOnly] ref MovementComponent moveComp, [ReadOnly] ref Translation posComp) {
            if (0.0f < math.abs(moveComp.value.x)) {
                return;
            }

            var effectEntity = cmdBuf.Instantiate(index, prefab);
            cmdBuf.SetComponent(index, effectEntity, posComp);
            cmdBuf.AddComponent<EffectSpawnExistComponent>(index, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new EffectSpawnSystemJob() {
            cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent()
        };

        var entities = EntityManager.GetAllEntities();
        foreach (var entity in entities.Where(entity =>
            true == EntityManager.HasComponent(entity, typeof(EffectSpawnComponent)))) {
            var effectSpawnComp = EntityManager.GetComponentData<EffectSpawnComponent>(entity);
            job.prefab = effectSpawnComp.prefab;
            break;
        }
        entities.Dispose();

        var handle = job.Schedule(this, inputDependencies);
        _cmdSystem.AddJobHandleForProducer(handle);

        return handle;
    }
}
