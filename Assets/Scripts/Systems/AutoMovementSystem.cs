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
        var desiredPos = new Translation();

        Entities.ForEach((Entity entity, ref TargetComponent targetComp, ref MovementComponent moveComp, ref Translation currentPos) => {

            Entity targetEntity = Entity.Null;

            // get target
            int targetIndex = targetComp.targetIndex;
            int lastTargetIndex = targetComp.lastTargetIndex;
            Entities.ForEach((Entity otherEntity, ref Translation entityPos) => {
                if (targetIndex == int.MaxValue || lastTargetIndex == otherEntity.Index) {
                    return;
                }

                if (targetIndex == otherEntity.Index) {
                    targetEntity = otherEntity;
                }
            });

            if (targetEntity.Equals(Entity.Null)) {
                return;
            }

            desiredPos = EntityManager.GetComponentData<Translation>(targetEntity);

            var at = desiredPos.Value.x - currentPos.Value.x;
            if (0.5f >= math.abs(at)) {
                moveComp.value = 0.0f;
                return;
            }

            bool isHeadingForward = (moveComp.xValue < 0.0f && at < 0.0f) || (moveComp.xValue > 0.0f && at > 0.0f);
            if (false == isHeadingForward) {
                return;
            }

            if (moveComp.xValue < 0.0f) {
                moveComp.value.x = -1.0f;
            }
            else {
                moveComp.value.x = 1.0f;
            }

            var statusComp = EntityManager.GetComponentData<AvatarStatusComponent>(entity);
            currentPos.Value.x += moveComp.value.x * deltaTime * statusComp.MoveSpeed;
        });
    }
}
