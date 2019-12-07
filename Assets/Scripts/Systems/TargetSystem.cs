﻿// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;

public class TargetingSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }


    protected override void OnUpdate() {
        Entities.With<PlayerComponent>.ForEach((Entity playerEntity) => {
            if (EntityManager.HasComponent<MovementComponent>(playerEntity)) {
                return;
            }

            var lastNearestEntityIndex = int.MaxValue;
            var lastNearestDistance = float.PositiveInfinity;
            Entities.ForEach((Entity targetEntity, ref ReactiveComponent reactiveComp, ref Translation targetPos) => {
                var playerPos = EntityManager.GetComponentData<Translation>(targetEntity).Value;
                float xDistance = playerPos.x - targetPos.Value.x;
                //bool isHeadingForward = (baseMoveComponent.xValue < 0.0f && xDistance < 0.0f) || (baseMoveComponent.xValue > 0.0f && xDistance > 0.0f);
                //if (false == isHeadingForward) {
                //    return;
                //}

                float distance = Vector2.Distance(new Vector2(targetPos.Value.x, targetPos.Value.y), new Vector2(playerPos.x, playerPos.y));
                if (Mathf.Abs(distance) < lastNearestDistance) {
                    lastNearestEntityIndex = targetEntity.Index;
                    lastNearestDistance = Mathf.Abs(distance);
                }
            });

            if (lastNearestEntityIndex > 0) {
                EntityManager.AddComponentData<MovementComponent>(playerEntity, new MovementComponent(lastNearestEntityIndex));
            }
        });

        /*
        var targetIndex = int.MaxValue;
        var targetDistance = float.PositiveInfinity;
        var compareLastTargetIndex = int.MinValue;
        var compareIndex = int.MaxValue;
        var comparePos = Vector2.zero;
        var compareType = EntityType.None;

        Entities.ForEach((Entity baseEntity, ref ReactiveComponent baseReactiveComp, ref Translation basePos, ref TargetComponent baseTargetComp, ref MovementComponent baseMoveComp) => {
            targetIndex = int.MaxValue;
            targetDistance = float.PositiveInfinity;
            compareLastTargetIndex = baseTargetComp.lastTargetIndex;
            compareIndex = baseEntity.Index;
            comparePos = new Vector2(basePos.Value.x, basePos.Value.y);
            compareType = baseReactiveComp.type;
            var baseMoveComponent = baseMoveComp;

            if (baseReactiveComp.ReactionElapsedTime > 0.0f) {
                return;
            }


            //// DebugDraw
            //var target = baseTargetComp;

            Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos, ref TargetComponent targetComp) => {
                if (compareLastTargetIndex == entity.Index) {
                    return;
                }

                if (compareIndex == entity.Index) {
                    return;
                }

                if (compareType == EntityType.None) {
                    return;
                }

                if (compareType == reactiveComp.type) {
                    return;
                }

                if (reactiveComp.type != EntityType.Wall && reactiveComp.ReactedCount >= reactiveComp.reactionLimitCount) {
                    return;
                }

                // If the entity is movable, check it is heading forward
                float xDistance = pos.Value.x - comparePos.x;
                bool isHeadingForward = (baseMoveComponent.xValue < 0.0f && xDistance < 0.0f) || (baseMoveComponent.xValue > 0.0f && xDistance > 0.0f);
                if (false == isHeadingForward) {
                    return;
                }

                // todo : check boundary or check look direction
                float distance = Vector2.Distance(comparePos, new Vector2(pos.Value.x, pos.Value.y));
                if (Mathf.Abs(distance) < targetDistance) {
                    targetIndex = entity.Index;
                    targetDistance = Mathf.Abs(distance);

                    //// DebugDraw
                    //if (target.targetIndex == targetIndex) {
                    //    if (compareType == EntityType.Player)
                    //        Debug.DrawLine(comparePos, new Vector2(pos.Value.x, pos.Value.y), Color.red);
                    //}
                }
            });

            if (baseTargetComp.targetIndex != targetIndex) {
                baseTargetComp.targetIndex = targetIndex;
                baseTargetComp.targetDistance = targetDistance;
            }
            

        });
        */
    }
}
