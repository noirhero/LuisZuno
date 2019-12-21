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
                EntityManager.AddComponentData<SearchingComponent>(playerEntity, new SearchingComponent(targetReactiveComp.searchingTime, targetReactiveComp.searchingAnim));
                playerComp.currentBehaviors |= BehaviorState.searching;
            }

            // Panic
            bool shouldPanic = (targetReactiveComp.panicTime > 0.0f) && (false == BehaviorState.HasState(playerComp, BehaviorState.panic));
            if (shouldPanic) {
                EntityManager.AddComponentData<PanicComponent>(playerEntity, new PanicComponent(targetReactiveComp.panicTime, targetReactiveComp.panicAnim));
                playerComp.currentBehaviors |= BehaviorState.panic;
            }

            // Pending Item
            bool shouldPendItem = (targetReactiveComp.itemID > 0) && (false == BehaviorState.HasState(playerComp, BehaviorState.pendingItem));
            if (shouldPendItem) {
                EntityManager.AddComponentData<PendingItemComponent>(playerEntity, new PendingItemComponent() {
                    pendingItemID = targetReactiveComp.itemID
                });
                playerComp.currentBehaviors |= BehaviorState.pendingItem;
            }

            // Madness
            bool shouldGetMadness = (targetReactiveComp.linearMadness > 0 || targetReactiveComp.madness > 0);
            if (shouldGetMadness) {
                // update
                if (EntityManager.HasComponent<MadnessComponent>(playerEntity)) {
                    var madnessComp = EntityManager.GetComponentData<MadnessComponent>(playerEntity);

                    if (targetReactiveComp.linearMadness > 0.0f) {
                        madnessComp.linearValue = targetReactiveComp.linearMadness;
                        madnessComp.linearDuration = targetReactiveComp.linearMadnessDuration;
                        madnessComp.linearTickTime = targetReactiveComp.linearMadnessTickTime;
                    }

                    if (targetReactiveComp.madness > 0.0f) {
                        // 이미 적용 중인 값이 있을 경우
                        if (madnessComp.value > 0.0f) {
                            madnessComp.valueForInterrupted = madnessComp.transitionStartValue + madnessComp.value;
                        }
                        madnessComp.value = targetReactiveComp.madness;
                    }
                }
                // create
                else {
                    EntityManager.AddComponentData<MadnessComponent>(playerEntity, new MadnessComponent() {
                        value = targetReactiveComp.madness,
                        duration = targetReactiveComp.madnessDuration,
                        linearValue = targetReactiveComp.linearMadness,
                        linearDuration = targetReactiveComp.linearMadnessDuration,
                        linearTickTime = targetReactiveComp.linearMadnessTickTime
                    });
                    playerComp.currentBehaviors |= BehaviorState.madness;
                }
            }

            if (playerComp.currentBehaviors > 0) {
                intelComp.isWaitingForOtherSystems = true;
            }
        });
    }
}
