// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using GlobalDefine;
using Unity.Collections;

[UpdateAfter(typeof(MovementSystem))]
public class AutoMovementSystem : ComponentSystem {
    private EntityQuery _nonePlayerQuery;
    protected override void OnCreate() {
        _nonePlayerQuery = GetEntityQuery(new EntityQueryDesc() {
            None = new ComponentType[] {
                typeof(PlayerComponent)
            }
        });
    }

    private void OnUpdate_Velocity() {
        Entities.ForEach((Entity entity, ref Translation posComp, ref VelocityComponent velComp) => {
            posComp.Value.x += velComp.velocity * Time.DeltaTime;
        });
     }


    protected override void OnUpdate() {
        OnUpdate_Velocity();

        Entities.ForEach((Entity playerEntity, ref PlayerComponent playerComp, ref Translation playerPos, ref MovementComponent moveComp) => {
            // initialize
            if (playerComp.currentAnim != AnimationType.Walk) {
                playerComp.currentAnim = AnimationType.Walk;
            }

            var targetEntity = Entity.Null;
            var targetPos = float3.zero;

            var nonePlayerEntities = _nonePlayerQuery.ToEntityArray(Allocator.TempJob);
            foreach (var entity in nonePlayerEntities) {
                if (moveComp.targetEntityIndex != entity.Index) {
                    continue;
                }

                targetEntity = entity;
                targetPos = EntityManager.GetComponentData<Translation>(entity).Value;
                break;
            }
            nonePlayerEntities.Dispose();

            // DebugDraw
            Debug.DrawLine(new Vector2(targetPos.x, targetPos.y), new Vector2(playerPos.Value.x, playerPos.Value.y), Color.red);

            // arrived !
            var at = targetPos.x - playerPos.Value.x;
            if (0.5f >= math.abs(at)) {
                // 스테이지 종료
                if (EntityManager.HasComponent<StageClearComponent>(targetEntity)) {
                    EntityManager.AddComponentData<GameClearComponent>(playerEntity, new GameClearComponent());
                }

                // 벽꿍
                if (EntityManager.HasComponent<TurningComponent>(targetEntity)) {
                    EntityManager.AddComponentData(playerEntity, new TargetingComponent());
                    EntityManager.RemoveComponent<MovementComponent>(playerEntity);
                    playerComp.playerDirection *= -1.0f;
                }
                else {
                    EntityManager.RemoveComponent<MovementComponent>(playerEntity);
                    EntityManager.AddComponentData(playerEntity, new IntelligenceComponent(targetEntity.Index));
                    playerComp.currentAnim = AnimationType.Idle;    // MovementComponent를 삭제하면서 기본으로 돌림
                }
                return;
            }

            if (0.0f > playerComp.playerDirection) {
                moveComp.value.x = -1.0f;
            }
            else {
                moveComp.value.x = 1.0f;
            }

            //var statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(playerEntity);
            playerPos.Value.x += moveComp.value.x * Time.DeltaTime;
        });
    }
}
