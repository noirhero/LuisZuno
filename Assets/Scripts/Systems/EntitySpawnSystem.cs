// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using GlobalDefine;

public class EntitySpawnSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;


    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    
    private struct EntitySpawnSystemJob : IJobForEachWithEntity<EntitySpawnComponent, Translation, PlayerComponent> {
        public EntityCommandBuffer.Concurrent cmdBuf;

        public void Execute(Entity entity, int index, [ReadOnly] ref EntitySpawnComponent entityComp, [ReadOnly] ref Translation pos, ref PlayerComponent playerComp) {
            if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
                return;
            }

            var rand = new Random((uint)(entityComp.number));
            for (var i = 0; i < entityComp.number; ++i) {
                var instantiateEntity = cmdBuf.Instantiate(index, entityComp.prefab);
                var randPosOffset = new float3(rand.NextFloat(entityComp.posOffsetMin, entityComp.posOffsetMax), 0.0f, 0.0f);
                cmdBuf.SetComponent(index, instantiateEntity, new Translation() {
                    Value = pos.Value + randPosOffset,
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

                Utility.SetLifeCycle(index, ref cmdBuf, ref instantiateEntity, entityComp.lifetime, ref entityComp.spawnEffect, ref entityComp.destroyEffect);
            }
            
            cmdBuf.RemoveComponent<EntitySpawnComponent>(index, entity);

            playerComp.currentBehaviors ^= BehaviorState.spawning;
            cmdBuf.SetComponent<PlayerComponent>(index, entity, playerComp);
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
