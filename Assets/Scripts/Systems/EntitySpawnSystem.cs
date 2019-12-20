// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EntitySpawnSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;

    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        Enabled = false;
    }
    
    private struct EntitySpawnSystemJob : IJobForEachWithEntity<EntitySpawnComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, [ReadOnly] ref EntitySpawnComponent entityComp, [ReadOnly] ref Translation posComp) {
            for (int i=0; i<= entityComp.spawnNumber; i++) {
                var instantiateEntity = cmdBuf.Instantiate(index, entityComp.prefab);
                cmdBuf.SetComponent(index, instantiateEntity, posComp);
            }
            cmdBuf.RemoveComponent<EntitySpawnComponent>(index, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new EntitySpawnSystemJob() {
            cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent()
        };

        var handle = job.Schedule(this, inputDependencies);
        _cmdSystem.AddJobHandleForProducer(handle);

        return handle;
    }
}
