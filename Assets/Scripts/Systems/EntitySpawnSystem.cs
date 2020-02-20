// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

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

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent();
        var jobHandle = Entities
            .WithName("EntitySpawnSystem")
            .ForEach((Entity entity, int entityInQueryIndex, ref PlayerComponent playerComp, in EntitySpawnComponent entityComp, in Translation pos) => {

                if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
                    return;
                }

                var rand = new Random((uint)(entityComp.number));
                for (var i = 0; i < entityComp.number; ++i) {
                    var instantiateEntity = cmdBuf.Instantiate(entityInQueryIndex, entityComp.prefab);
                    var randPosOffset = new float3(rand.NextFloat(entityComp.posOffsetMin, entityComp.posOffsetMax), 0.0f, 0.0f);
                    cmdBuf.SetComponent(entityInQueryIndex, instantiateEntity, new Translation() {
                        Value = pos.Value + randPosOffset,
                    });

                    var randForward = rand.NextBool();
                    cmdBuf.SetComponent(entityInQueryIndex, instantiateEntity, new Rotation() {
                        Value = quaternion.RotateY(randForward ? math.radians(180.0f) : 0.0f),
                    });

                    var randDir = randForward ? 1.0f : -1.0f;
                    var randVel = rand.NextFloat(entityComp.velocityMin, entityComp.velocityMax);
                    cmdBuf.AddComponent(entityInQueryIndex, instantiateEntity, new VelocityComponent() {
                        velocity = randVel * randDir,
                    });

                    Utility.SetLifeCycle(entityInQueryIndex, in instantiateEntity, entityComp.lifetime,
                        in entityComp.spawnEffect, in entityComp.destroyEffect, in cmdBuf);
                }

                cmdBuf.RemoveComponent<EntitySpawnComponent>(entityInQueryIndex, entity);
                playerComp.currentBehaviors ^= BehaviorState.spawning;
            })
            .Schedule(inputDependencies);

        _cmdSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
