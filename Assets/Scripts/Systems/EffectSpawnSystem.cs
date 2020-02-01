// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using GlobalDefine;

[UpdateAfter(typeof(AutoMovementSystem))]
public class EffectSpawnSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _cmdSystem;
    protected override void OnCreate() {
        _cmdSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var cmdBuf = _cmdSystem.CreateCommandBuffer().ToConcurrent();
        return Entities
            .WithName("EffectSpawnSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((Entity entity, int entityInQueryIndex, in EffectSpawnComponent effect, in Translation pos) => {
                cmdBuf.RemoveComponent<EffectSpawnComponent>(entityInQueryIndex, entity);
                Utility.SetLifeCycle(entityInQueryIndex, in entity, 0.0f, 
                    Entity.Null, Entity.Null, in cmdBuf);

                var effectEntity = cmdBuf.Instantiate(entityInQueryIndex, effect.prefab);
                cmdBuf.SetComponent(entityInQueryIndex, effectEntity, pos);
                Utility.SetLifeCycle(entityInQueryIndex, in effectEntity, effect.lifetime, 
                    Entity.Null, Entity.Null, in cmdBuf);
            })
            .Schedule(inputDependencies);
    }
}
