// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using System;
using System.Text;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class ReactiveSystem : ComponentSystem {
    private int[] _animNameHashes;
    private AnimationType _currentAnim = AnimationType.Idle;

    // caching anim information
    protected override void OnCreate() {
        int totalAnimCount = Enum.GetNames(typeof(AnimationType)).Length;
        _animNameHashes = new int[totalAnimCount];

        foreach (AnimationType type in Enum.GetValues(typeof(AnimationType))) {
            var nameHash = 0;
            foreach (var b in Encoding.ASCII.GetBytes(type.ToString())) {
                nameHash += b;
            }
            _animNameHashes[(int)type] = nameHash;
        }
    }

    protected override void OnUpdate() {
        var lastTargetIndex = int.MinValue;
        var currentTargetIndex = int.MaxValue;

        Entities.ForEach((ref ReactiveComponent baseReactiveComp, ref MovementComponent baseMoveComp, ref TargetComponent baseTargetComp, ref SpriteAnimComponent baseAnimComp) => {
            currentTargetIndex = baseTargetComp.targetIndex;
            lastTargetIndex = baseTargetComp.lastTargetIndex;

            Entity targetEntity = Entity.Null;
            Entities.WithAll<ReactiveComponent>().ForEach((Entity entity) => {
                if (currentTargetIndex != int.MaxValue && currentTargetIndex == entity.Index) {
                    targetEntity = entity;
                }
            });

            if (targetEntity.Equals(Entity.Null))
                return;

            
            ReactiveComponent targetReactiveComp = EntityManager.GetComponentData<ReactiveComponent>(targetEntity);

            bool bMoving = math.FLT_MIN_NORMAL < math.lengthsq(baseMoveComp.value);
            if (bMoving) {
                _currentAnim = AnimationType.Walk;
            }
            // you've been arrived at the target
            else {
                if (targetReactiveComp.type == EntityType.Wall) {
                    baseMoveComp.xValue *= -1.0f;
                    baseTargetComp.lastTargetIndex = currentTargetIndex;
                    return;
                }

                // Doing something
                if (targetReactiveComp.reactiveLength > targetReactiveComp.reactivingDuration) {
                    _currentAnim = AnimationType.NyoNyo;
                    targetReactiveComp.reactivingDuration += Time.deltaTime;
                }
                // Done
                else if (targetReactiveComp.reactiveLength <= targetReactiveComp.reactivingDuration) {
                    if (baseTargetComp.lastTargetIndex != currentTargetIndex) {
                        baseTargetComp.lastTargetIndex = currentTargetIndex;

                        targetReactiveComp.reactivingDuration = 0.0f;
                    }
                }
                else {
                    _currentAnim = AnimationType.Idle;
                }
            }

            EntityManager.SetComponentData<ReactiveComponent>(targetEntity, targetReactiveComp);


            if (baseAnimComp.nameHash != _animNameHashes[(int)_currentAnim])
                baseAnimComp.nameHash = _animNameHashes[(int)_currentAnim];
        });
    }
}
