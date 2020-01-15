// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using GlobalDefine;

public class LifeCycleSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;


    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    private struct EntityLifeCycleSystem : IJobForEachWithEntity<LifeCycleComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;
        public float deltaTime;

        public void Execute(Entity entity, int index, ref LifeCycleComponent lifeComp, [ReadOnly] ref Translation posComp) {
            if (lifeComp.duration >= lifeComp.lifetime) {
                if (Entity.Null != lifeComp.destroyEffect) {
                    Utility.SpawnEffect(index, ref cmdBuf, lifeComp.destroyEffect, posComp.Value);
                }
                cmdBuf.RemoveComponent<LifeCycleComponent>(index, entity);
                cmdBuf.DestroyEntity(index, entity);
            }
            else {
                if (lifeComp.duration == 0.0f) {
                    if (Entity.Null != lifeComp.spawnEffect) {
                        Utility.SpawnEffect(index, ref cmdBuf, lifeComp.spawnEffect, posComp.Value);
                    }
                }
                lifeComp.duration += deltaTime;
            }
        }
    }
    

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new EntityLifeCycleSystem() {
            cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent(),
            deltaTime = Time.DeltaTime,
        };

        var handle = job.Schedule(this, inputDependencies);
        _cmdSystem.AddJobHandleForProducer(handle);
        return handle;
    }
}
