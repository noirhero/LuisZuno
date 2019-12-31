// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class LifeCycleSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;


    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        Enabled = false;
    }


    private struct EntityLifeCycleSystem : IJobForEachWithEntity<LifeCycleComponent, Translation> {
        public EntityCommandBuffer.Concurrent cmdBuf;
        public float deltaTime;

        public void Execute(Entity entity, int index, ref LifeCycleComponent lifeComp, [ReadOnly] ref Translation posComp) {
            if (-1.0f == lifeComp.duration) {
                return;
            }

            if (lifeComp.duration >= lifeComp.lifetime) {
                if (Entity.Null != lifeComp.destroyEffect) {
                    Entity effect = cmdBuf.CreateEntity(index);
                    cmdBuf.AddComponent(index, effect, new Translation() {
                        Value = posComp.Value,
                    });
                    cmdBuf.AddComponent(index, effect, new EffectSpawnComponent() {
                        prefab = lifeComp.destroyEffect,
                        lifetime = 0.4f,
                        duration = 0.0f,
                    });
                    lifeComp.destroyEffect = Entity.Null;
                }
                cmdBuf.RemoveComponent<LifeCycleComponent>(index, entity);
                cmdBuf.DestroyEntity(index, entity);
            }
            else {
                if (Entity.Null != lifeComp.spawnEffect) {
                    Entity effect = cmdBuf.CreateEntity(index);
                    cmdBuf.AddComponent(index, effect, new Translation() {
                        Value = posComp.Value,
                    });
                    cmdBuf.AddComponent(index, effect, new EffectSpawnComponent() {
                        prefab = lifeComp.spawnEffect,
                        lifetime = 0.4f,
                        duration = 0.0f,
                    });
                    lifeComp.spawnEffect = Entity.Null;
                }
                lifeComp.duration += deltaTime;
            }
        }
    }
    

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new EntityLifeCycleSystem() {
            cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent(),
            deltaTime = Time.deltaTime,
        };

        var handle = job.Schedule(this, inputDependencies);
        _cmdSystem.AddJobHandleForProducer(handle);
        return handle;
    }
}
