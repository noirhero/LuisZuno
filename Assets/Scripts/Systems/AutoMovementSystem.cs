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

        Entities.With<PlayerComponent>.ForEach((Entity playerEntity, ref MovementComponent moveComp, ref Translation playerPos) => {

            var targetEntity = Entity.Null;

            // get target
            var targetIndex = moveComp.targetIndex;
            Entities.ForEach((Entity otherEntity, ref Translation entityPos) => {
                if (targetIndex == int.MaxValue /*|| lastTargetIndex == otherEntity.Index*/) {
                    return;
                }

                if (targetIndex == otherEntity.Index) {
                    targetEntity = otherEntity;
                    return;
                }
            });

            if (targetEntity.Equals(Entity.Null)) {
                return;
            }

            

            float3 targetPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;

            // DebugDraw
            Debug.DrawLine(new Vector2(targetPos.x, targetPos.y), new Vector2(playerPos.Value.x, playerPos.Value.y), Color.red);

            // arrived !
            var at = targetPos.x - playerPos.Value.x;
            if (0.5f >= math.abs(at)) {

                EntityManager.RemoveComponent<MovementComponent>(playerEntity);

                //EntityManager.AddComponent<>();

                return;
            }

            //var isHeadingForward = (moveComp.xValue < 0.0f && at < 0.0f) || (moveComp.xValue > 0.0f && at > 0.0f);
            //if (false == isHeadingForward) {
            //    return;
            //}

            if (moveComp.xValue < 0.0f) {
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
