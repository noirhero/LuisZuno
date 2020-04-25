// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using GlobalDefine;

[UpdateAfter(typeof(GameSystem))]
public class LifeCycleSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;
    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var deltaTime = Time.DeltaTime;
        var cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent();
        var jobHandle = Entities
            .WithName("LifeCycleSystem")
            .ForEach((Entity entity, int entityInQueryIndex, ref LifeCycleComponent lifeComp, in Translation posComp) => {
                if (lifeComp.duration >= lifeComp.lifetime) {
                    if (Entity.Null != lifeComp.destroyEffect) {
                        Utility.SpawnEffect(entityInQueryIndex, lifeComp.destroyEffect, posComp.Value, in cmdBuf);
                    }
                    cmdBuf.RemoveComponent<LifeCycleComponent>(entityInQueryIndex, entity);
                    cmdBuf.DestroyEntity(entityInQueryIndex, entity);
                }
                else {
                    if (lifeComp.duration == 0.0f) {
                        if (Entity.Null != lifeComp.spawnEffect) {
                            Utility.SpawnEffect(entityInQueryIndex, lifeComp.spawnEffect, posComp.Value, in cmdBuf);
                        }
                    }
                    lifeComp.duration += deltaTime;
                }
            })
            .Schedule(inputDependencies);

        _cmdSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
