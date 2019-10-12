// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;

public class TargetSystem : ComponentSystem {
    protected override void OnUpdate() {
        var targetIndex = int.MaxValue;
        var targetDistance = float.PositiveInfinity;
        var targetReactiveLength = float.PositiveInfinity;
        var compareIndex = int.MaxValue;
        var comparePos = Vector2.zero;
        var compareType = EntityType.None;

        Entities.ForEach((Entity baseEntity, ref ReactiveComponent baseReactiveComp, ref Translation basePos, ref TargetComponent baseTargetComp, ref MovementComponent baseMoveComp) => {
            targetIndex = int.MaxValue;
            compareIndex = baseEntity.Index;
            targetDistance = float.PositiveInfinity;
            comparePos = new Vector2(basePos.Value.x, basePos.Value.y);
            compareType = baseReactiveComp.type;
            var baseMoveComponent = baseMoveComp;

            Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos, ref TargetComponent targetComp) => {
                if (compareIndex == entity.Index)
                    return;

                if (compareType == EntityType.None)
                    return;

                if (compareType == reactiveComp.type)
                    return;

                // If the entity is movable, check it is heading forward
                float xDistance = pos.Value.x - comparePos.x;
                bool isHeadingForward = (baseMoveComponent.xValue < 0.0f && xDistance < 0.0f) || (baseMoveComponent.xValue > 0.0f && xDistance > 0.0f);
                if (false == isHeadingForward)
                    return;

                // todo : check boundary or check look direction
                float distance = Vector2.Distance(comparePos, new Vector2(pos.Value.x, pos.Value.y));
                if (Mathf.Abs(distance) < targetDistance) {
                    targetIndex = entity.Index;
                    targetDistance = Mathf.Abs(distance);
                    targetReactiveLength = reactiveComp.reactiveLength;
                }
            });

            baseTargetComp.targetIndex = targetIndex;
            baseTargetComp.targetDistance = targetDistance;
        });
    }
}
