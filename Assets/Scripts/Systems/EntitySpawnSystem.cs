// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

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
    

    private struct EntitySpawnSystemJob : IJobForEachWithEntity<EntitySpawnComponent, LifeCycleComponent> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, [ReadOnly] ref EntitySpawnComponent entityComp, ref LifeCycleComponent lifeComp) {
            var rand = new Random((uint)(entityComp.number));

            for (int i = 0; i < entityComp.number; ++i) {
                var instantiateEntity = cmdBuf.Instantiate(index, entityComp.prefab);
                cmdBuf.SetComponent(index, instantiateEntity, new Translation() {
                    Value = entityComp.spawnPosition,
                });
                
                var randForward = rand.NextBool();
                cmdBuf.SetComponent(index, instantiateEntity, new Rotation() {
                    Value = quaternion.RotateY(randForward ? math.radians(180.0f) : 0.0f),
                });

                var randDir = randForward ? 1.0f : -1.0f;
                var randVel = rand.NextFloat(entityComp.velocityMin, entityComp.velocityMax);
                cmdBuf.AddComponent(index, instantiateEntity, new VelocityComponent() {
                    velocity = randVel * randDir,
                });

                cmdBuf.AddComponent(index, instantiateEntity, new LifeCycleComponent() {
                    spawnEffect = lifeComp.spawnEffect,
                    destroyEffect = lifeComp.destroyEffect,
                    lifetime = lifeComp.lifetime,
                    duration = 0.0f,
                });
            }
            cmdBuf.RemoveComponent<EntitySpawnComponent>(index, entity);
            cmdBuf.RemoveComponent<LifeCycleComponent>(index, entity);
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
