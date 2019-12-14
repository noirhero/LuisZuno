// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using GlobalDefine;

[UpdateAfter(typeof(MovementSystem))]
public class AutoMovementSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {
        Entities.WithAll<MovementComponent>(). ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref Translation playerPos) => {
            // initialize
            if (playerComp.currentAnim != AnimationType.Walk) {
                playerComp.currentAnim = AnimationType.Walk;
            }

            var moveComp = EntityManager.GetComponentData<MovementComponent>(playerEntity);

            var targetEntity = Entity.Null;
            var targetPos = float3.zero;
            Entities.WithNone<PlayerComponent>().ForEach((Entity entity) => {
                if (moveComp.targetEntityIndex == entity.Index) {
                    targetEntity = entity;
                    targetPos = EntityManager.GetComponentData<Translation>(entity).Value;
                    return;
                }
            });

            // DebugDraw
            Debug.DrawLine(new Vector2(targetPos.x, targetPos.y), new Vector2(playerPos.Value.x, playerPos.Value.y), Color.red);

            // arrived !
            var at = targetPos.x - playerPos.Value.x;
            if (0.5f >= math.abs(at)) {
                EntityManager.RemoveComponent<MovementComponent>(playerEntity);
                EntityManager.AddComponentData<IntelligenceComponent>(playerEntity, new IntelligenceComponent(targetEntity.Index));
                playerComp.currentAnim = AnimationType.Idle;    // MovementComponent를 삭제하면서 기본으로 돌림
                return;
            }

            if (0.0f > playerComp.playerDirection) {
                moveComp.value.x = -1.0f;
            }
            else {
                moveComp.value.x = 1.0f;
            }

            //var statusComp = EntityManager.GetComponentData<AvatarStatusComponent>(playerEntity);
            playerPos.Value.x += moveComp.value.x * Time.deltaTime;
        });
    }
}
