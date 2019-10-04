// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public class AutoMovementSystem : ComponentSystem {
    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;
        var desiredPos = new Translation();

        Entities.ForEach((Entity entity, ref TargetComponent targetComp, ref MovementComponent moveComp, ref Translation currentPos) => {
            // Initialize to know if it is changed
            desiredPos.Value.z = -5.0f;

            TargetComponent currentTarget = targetComp;
            Entities.ForEach((Entity targetEntity, ref Translation targetPos) => {
                if (currentTarget.targetIndex == targetEntity.Index) {
                    desiredPos = targetPos;
                }
            });

            // if it is not initialized
            if (-5.0f == desiredPos.Value.z)
                return;

            var at = desiredPos.Value.x - currentPos.Value.x;
            bool isHeadingForward = (moveComp.xValue < 0.0f && at < 0.0f) || (moveComp.xValue > 0.0f && at > 0.0f);
            if (false == isHeadingForward)
                return;

            if (0.5f >= math.abs(at)) {
                moveComp.value = 0.0f;
                return;
            }

            if (moveComp.xValue < 0.0f) {
                moveComp.value.x = -1.0f;
            }
            else {
                moveComp.value.x = 1.0f;
            }

            currentPos.Value.x += moveComp.value.x * deltaTime;
        });
    }
}
