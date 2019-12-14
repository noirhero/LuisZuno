// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using GlobalDefine;

public class IntelligenceSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {

        Entities.ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref IntelligenceComponent intelComp) => {
            if (playerComp.currentBehaviors > 0)
                return;

            // all behaviours have done!
            if (intelComp.isWaitingForOtherSystems) {
                intelComp.isWaitingForOtherSystems = false;
                EntityManager.RemoveComponent<IntelligenceComponent>(playerEntity);
                EntityManager.AddComponentData<TargetingComponent>(playerEntity, new TargetingComponent());
            }

            // get target
            var targetEntity = Entity.Null;
            var targetIndex = intelComp.targetEntityIndex;
            Entities.ForEach((Entity entity, ref Translation entityPos) => {
                if (targetIndex == entity.Index) {
                    targetEntity = entity;
                    return;
                }
            });

            ReactiveComponent targetReactiveComp = EntityManager.GetComponentData<ReactiveComponent>(targetEntity);

            bool shouldSearch = (targetReactiveComp.searchingTime > 0.0f) && (false == BehaviorState.HasState(playerComp, BehaviorState.searching));
            bool shouldPanic = (targetReactiveComp.panicTime > 0.0f) && (false == BehaviorState.HasState(playerComp, BehaviorState.panic));

            if (shouldSearch) {
                EntityManager.AddComponentData<SearchingComponent>(playerEntity, new SearchingComponent(targetReactiveComp.searchingTime, targetReactiveComp.searchingAnim));
                playerComp.currentBehaviors |= BehaviorState.searching;
            }

            if (shouldPanic) {
                EntityManager.AddComponentData<PanicComponent>(playerEntity, new PanicComponent(targetReactiveComp.panicTime, targetReactiveComp.panicAnim));
                playerComp.currentBehaviors |= BehaviorState.panic;
            }

            if (playerComp.currentBehaviors > 0) {
                intelComp.isWaitingForOtherSystems = true;
            }
        });
    }
}
