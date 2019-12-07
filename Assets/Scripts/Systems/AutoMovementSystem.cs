// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public class AutoMovementSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        Entities.WithAll<PlayerComponent>().ForEach((Entity playerEntity, ref MovementComponent moveComp, ref Translation playerPos) => {

            var targetEntity = Entity.Null;

            // get target
            var targetIndex = moveComp.targetEntityIndex;
            Entities.ForEach((Entity entity, ref Translation entityPos) => {
                if (targetIndex == int.MaxValue /*|| lastTargetIndex == otherEntity.Index*/) {
                    return;
                }

                if (targetIndex == entity.Index) {
                    targetEntity = entity;
                    return;
                }
            });

            if (targetEntity.Equals(Entity.Null)) {
                return;
            }

            

            float3 targetPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;

            // DebugDraw
            //Debug.DrawLine(new Vector2(targetPos.x, targetPos.y), new Vector2(playerPos.Value.x, playerPos.Value.y), Color.red);

            // arrived !
            var at = targetPos.x - playerPos.Value.x;
            if (0.5f >= math.abs(at)) {

                EntityManager.RemoveComponent<MovementComponent>(playerEntity);

                //EntityManager.AddComponent<>();

                return;
            }

            PlayerComponent playerComp = EntityManager.GetComponentData<PlayerComponent>(playerEntity);
            if (playerComp.playerDirection < 0.0f) {
                moveComp.value.x = -1.0f;
            }
            else {
                moveComp.value.x = 1.0f;
            }

            //var statusComp = EntityManager.GetComponentData<AvatarStatusComponent>(playerEntity);
            playerPos.Value.x += moveComp.value.x * deltaTime;
        });







    }
}
