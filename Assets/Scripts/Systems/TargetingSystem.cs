// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public class TargetingSystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.WithAll<PlayerComponent, TargetingComponent>().ForEach((Entity playerEntity) => {
            if (EntityManager.HasComponent<MovementComponent>(playerEntity)) {
                return;
            }

            var lastNearestEntityIndex = int.MaxValue;
            var lastNearestDistance = float.PositiveInfinity;
            var playerComp = EntityManager.GetComponentData<PlayerComponent>(playerEntity);
            var playerPos = EntityManager.GetComponentData<Translation>(playerEntity).Value;
            var playerStatus = EntityManager.GetComponentData<PlayerStatusComponent>(playerEntity);

            Entities.WithNone<PlayerComponent>().ForEach((Entity targetEntity) => {
                if (false == EntityManager.HasComponent<ReactiveComponent>(targetEntity)) {
                    return;
                }

                // compare each status
                if (EntityManager.HasComponent<PropStatusComponent>(targetEntity)) {
                    var targetStatus = EntityManager.GetComponentData<PropStatusComponent>(targetEntity);

                    if (playerStatus.search < targetStatus.search)
                        return;
                }

                var targetPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;
                var xDistance = targetPos.x - playerPos.x;

                // AutoMovementSystem에서 멈추게 되는 최소 거리 0.5f보다 멀리 있는 오브젝트를 타겟으로 지정
                var isHeadingForward = (playerComp.playerDirection < 0.0f && xDistance < 0.5f) || (playerComp.playerDirection > 0.0f && xDistance > 0.5f);
                if (false == isHeadingForward) {
                    return;
                }

                var distance = Vector2.Distance(new Vector2(targetPos.x, targetPos.y), new Vector2(playerPos.x, playerPos.y));
                if (Mathf.Abs(distance) < lastNearestDistance) {
                    lastNearestEntityIndex = targetEntity.Index;
                    lastNearestDistance = Mathf.Abs(distance);
                }
            });

            if (lastNearestEntityIndex != int.MaxValue) {
                EntityManager.RemoveComponent<TargetingComponent>(playerEntity);
                EntityManager.AddComponentData(playerEntity, new MovementComponent(lastNearestEntityIndex));
            }
        });
    }
}
