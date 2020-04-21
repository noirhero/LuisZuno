// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;

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

                    if (playerStatus.status.search < targetStatus.search)
                        return;
                }

                var targetPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;
                // 높이 상수로 제한
                var yDistance = targetPos.y - playerPos.y;
                if (2.0f < Mathf.Abs(yDistance)) {
                    return;
                }

                // AutoMovementSystem에서 멈추게 되는 최소 거리 0.5f보다 멀리 있는 오브젝트를 타겟으로 지정
                var xDistance = targetPos.x - playerPos.x;
                if (0.5f > Mathf.Abs(xDistance)) {
                    return;
                }
                
                var isHeadingForward = (playerComp.playerDirection * xDistance > 0.0f) ? true : false;
                if (false == isHeadingForward) {
                    return;
                }
                
                if (Mathf.Abs(xDistance) < lastNearestDistance) {
                    lastNearestEntityIndex = targetEntity.Index;
                    lastNearestDistance = Mathf.Abs(xDistance);
                }
            });

            if (lastNearestEntityIndex != int.MaxValue) {
                EntityManager.RemoveComponent<TargetingComponent>(playerEntity);
                EntityManager.AddComponentData(playerEntity, new MovementComponent(lastNearestEntityIndex));
            }
        });
    }
}
