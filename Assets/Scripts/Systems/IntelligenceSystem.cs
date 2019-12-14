// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class IntelligenceSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {

        Entities.WithAll<PlayerComponent>().ForEach((Entity playerEntity, ref IntelligenceComponent intelComp) => {

            // all behaviours have done!
            if (intelComp.hasDoneSettingCopmonents) {
                if (false == EntityManager.HasComponent<BehaviorComponent>(playerEntity) && false == EntityManager.HasComponent<BehaviorCompleteComponent>(playerEntity)) {
                    EntityManager.RemoveComponent<IntelligenceComponent>(playerEntity);
                    EntityManager.AddComponentData<TargetingComponent>(playerEntity, new TargetingComponent());
                }
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
            //if (targetReactiveComp.SearchingTime > 0.0f) {
            //    EntityManager.AddComponentData<SearchingComponent>(playerEntity, new SearchingComponent(targetReactiveComp.SearchingTime));
            //}

            //if (targetReactiveComp.PanicTime > 0.0f) {
            //}

            //if (targetReactiveComp.Items.Length > 0) {
            //}

            //if (targetReactiveComp.Madness > 0.0f) {

            //}
        });
    }
}
