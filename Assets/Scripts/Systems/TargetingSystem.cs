﻿// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public class TargetingSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {
        Entities.WithAll<PlayerComponent, TargetingComponent>().ForEach((Entity playerEntity) => {
            if (EntityManager.HasComponent<MovementComponent>(playerEntity)) {
                return;
            }

            var lastNearestEntityIndex = int.MaxValue;
            var lastNearestDistance = float.PositiveInfinity;
            var playerComp = EntityManager.GetComponentData<PlayerComponent>(playerEntity);
            var playerPos = EntityManager.GetComponentData<Translation>(playerEntity).Value;
            Entities.WithNone<PlayerComponent>().ForEach((Entity targetEntity) => {
                var targetPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;
                var xDistance = targetPos.x - playerPos.x;

                var isHeadingForward = (playerComp.playerDirection < 0.0f && xDistance < 0.0f) || (playerComp.playerDirection > 0.0f && xDistance > 0.0f);
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
                EntityManager.AddComponentData<MovementComponent>(playerEntity, new MovementComponent(lastNearestEntityIndex));                
            }
        });
    }
}
