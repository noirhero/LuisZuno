// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

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
                EntityManager.AddComponentData(playerEntity, new TargetingComponent());
                return;
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

            // Searching
            bool shouldSearch = (targetReactiveComp.searchingTime > 0.0f) && (false == BehaviorState.HasState(playerComp, BehaviorState.searching));
            if (shouldSearch) {
                EntityManager.AddComponentData(playerEntity, new SearchingComponent() {
                    searchingTime = targetReactiveComp.searchingTime,
                    searchingAnim = targetReactiveComp.searchingAnim,
                    elapsedSearchingTime = 0.0f
                });
                playerComp.currentBehaviors |= BehaviorState.searching;
            }

            // Panic
            bool shouldPanic = (targetReactiveComp.panicTime > 0.0f) && (false == BehaviorState.HasState(playerComp, BehaviorState.panic));
            if (shouldPanic) {
                EntityManager.AddComponentData(playerEntity, new PanicComponent() {
                    panicTime = targetReactiveComp.panicTime,
                    panicAnim = targetReactiveComp.panicAnim,
                    madness = targetReactiveComp.madness,
                    madnessDuration = targetReactiveComp.madnessDuration,
                    elapsedPanicTime = 0.0f,

                });
                playerComp.currentBehaviors |= BehaviorState.panic;
            }

            // Pending Item
            bool shouldPendItem = (targetReactiveComp.itemID > 0) && (false == BehaviorState.HasState(playerComp, BehaviorState.pendingItem));
            if (shouldPendItem) {
                EntityManager.AddComponentData(playerEntity, new PendingItemComponent() {
                    pendingItemID = targetReactiveComp.itemID
                });
                playerComp.currentBehaviors |= BehaviorState.pendingItem;
            }
            
            if (playerComp.currentBehaviors > 0) {
                intelComp.isWaitingForOtherSystems = true;
            }
        });
    }
}
