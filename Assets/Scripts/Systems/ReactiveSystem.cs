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
    private bool bInPanic = false;

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


    private void FinishReaction(ref TargetComponent targetComp, ref ReactiveComponent targetReactiveComp) {
        targetComp.lastTargetIndex = targetComp.targetIndex;
        targetReactiveComp.reactivingDuration = 0.0f;
        bInPanic = false;
    }


    private bool ShouldBePanic(ref Entity targetEntity, ref AvatarStatusComponent baseStatusComp) {
        // 플레이어 대상으로 몬스터가 패닉하지는 않음
        var reactiveComp = EntityManager.GetComponentData<ReactiveComponent>(targetEntity);
        if (reactiveComp.type == EntityType.Player)
            return false;

        // Non-avatar
        if (EntityManager.HasComponent<NoneAvatarStatusComponent>(targetEntity)) {
            var targetStatusComp = EntityManager.GetComponentData<NoneAvatarStatusComponent>(targetEntity);

            if (targetStatusComp.madness > 0) {
                baseStatusComp.madness += targetStatusComp.madness;
                EntityManager.SetComponentData<NoneAvatarStatusComponent>(targetEntity, targetStatusComp);
                return true;
            }
        }
        // Avatar
        else if (EntityManager.HasComponent<AvatarStatusComponent>(targetEntity)) {
            var targetStatusComp = EntityManager.GetComponentData<AvatarStatusComponent>(targetEntity);

            if (targetStatusComp.madness > 0) {
                baseStatusComp.madness += targetStatusComp.madness;
                EntityManager.SetComponentData<AvatarStatusComponent>(targetEntity, targetStatusComp);
                return true;
            }
        }
        return false;
    }


    protected override void OnUpdate() {
        var lastTargetIndex = int.MinValue;
        var currentTargetIndex = int.MaxValue;

        Entities.ForEach((Entity e, ref ReactiveComponent baseReactiveComp, ref MovementComponent baseMoveComp, ref TargetComponent baseTargetComp, ref SpriteAnimComponent baseAnimComp, ref AvatarStatusComponent baseStatusComp) => {
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

                if (bInPanic) {
                    Debug.LogFormat(baseReactiveComp.type.ToString() + " Panic and moving / my index : " + e.Index);
                }
            }
            // you've been arrived at the target
            else {
                if (targetReactiveComp.type == EntityType.Wall) {
                    
                    baseMoveComp.xValue *= -1.0f;
                    FinishReaction(ref baseTargetComp, ref targetReactiveComp);
                    return;
                }

                // Doing something
                if (targetReactiveComp.reactiveLength > targetReactiveComp.reactivingDuration) {
                    if (false == bInPanic)
                        _currentAnim = AnimationType.SomethingDoIt;

                    targetReactiveComp.reactivingDuration += Time.deltaTime;
                }
                // Done
                else if ((targetReactiveComp.reactiveLength <= targetReactiveComp.reactivingDuration) && (baseTargetComp.lastTargetIndex != currentTargetIndex)) {
                    // 2번째 상호작용(패닉)이 끝남
                    if (bInPanic) {
                        FinishReaction(ref baseTargetComp, ref targetReactiveComp);
                    }
                    // 1번째 상호작용이 끝남
                    else {
                        // 타겟이 광기에 영향을 미친다면 패닉 상태로 전환
                        bInPanic = ShouldBePanic(ref targetEntity, ref baseStatusComp);

                        // 일단 패닉시간 1초로 하드코딩
                        if (bInPanic) {
                            _currentAnim = AnimationType.NyoNyo;
                            targetReactiveComp.reactivingDuration -= 1.0f;

                            Debug.LogFormat(baseReactiveComp.type.ToString() + " Panic start / my index : " + e.Index);
                        }
                        // 광기 카드를 받지 않으면 다음 타겟으로
                        else {
                            FinishReaction(ref baseTargetComp, ref targetReactiveComp);
                        }
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
