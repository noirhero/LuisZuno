// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;

public class TargetingSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {
        Entities.WithAll<PlayerComponent>().ForEach((Entity playerEntity) => {
            if (EntityManager.HasComponent<MovementComponent>(playerEntity)) {
                return;
            }

            var lastNearestEntityIndex = int.MaxValue;
            var lastNearestDistance = float.PositiveInfinity;
            Entities.ForEach((Entity targetEntity, ref ReactiveComponent reactiveComp, ref Translation targetPos) => {
                var playerPos = EntityManager.GetComponentData<Translation>(playerEntity).Value;
                float xDistance = targetPos.Value.x - playerPos.x;

                PlayerComponent playerComp = EntityManager.GetComponentData<PlayerComponent>(playerEntity);
                bool isHeadingForward = (playerComp.playerDirection < 0.0f && xDistance < 0.0f) || (playerComp.playerDirection > 0.0f && xDistance > 0.0f);
                if (false == isHeadingForward) {
                    return;
                }

                float distance = Vector2.Distance(new Vector2(targetPos.Value.x, targetPos.Value.y), new Vector2(playerPos.x, playerPos.y));
                if (Mathf.Abs(distance) < lastNearestDistance) {
                    lastNearestEntityIndex = targetEntity.Index;
                    lastNearestDistance = Mathf.Abs(distance);
                }
            });

            if (lastNearestEntityIndex != int.MaxValue) {
                EntityManager.AddComponentData<MovementComponent>(playerEntity, new MovementComponent(lastNearestEntityIndex));
            }
        });

    }
}
