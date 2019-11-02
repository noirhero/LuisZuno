// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class ReactiveSystem : ComponentSystem {
    private Entity _currentEntity = Entity.Null;
    private Entity _targetEntity = Entity.Null;
    private MovementComponent _currentMoveComp;
    private AvatarStatusComponent _currentStatusComp;
    private TargetComponent _currentTargetComp;
    private ReactiveComponent _currentReactiveComp;
    private ReactiveComponent _targetReactiveComp;


    private Entity GetTargetEntity(int targetIndex) {
        Entity targetEntity = Entity.Null;
        Entities.WithAll<ReactiveComponent>().ForEach((Entity e) => {
            if (targetIndex != int.MaxValue && targetIndex == e.Index) {
                targetEntity = e;
            }
        });
        return targetEntity;
    }


    private bool ShouldBePanic() {
        // 타겟이 플레이어인 경우 무시 (몬스터가 플레이어에게 패닉하지 않음)
        var targetReactiveComp = EntityManager.GetComponentData<ReactiveComponent>(_targetEntity);
        if (targetReactiveComp.type == EntityType.Player)
            return false;

        // Non-avatar
        if (EntityManager.HasComponent<NoneAvatarStatusComponent>(_targetEntity)) {
            var targetStatusComp = EntityManager.GetComponentData<NoneAvatarStatusComponent>(_targetEntity);
            if (targetStatusComp.madness > 0) {
                _currentStatusComp.madness += targetStatusComp.madness;     // todo : need some formula
                _currentStatusComp.InPanic = true;
                return true;
            }
        }
        // Avatar
        else if (EntityManager.HasComponent<AvatarStatusComponent>(_targetEntity)) {
            var targetStatusComp = EntityManager.GetComponentData<AvatarStatusComponent>(_targetEntity);
            if (targetStatusComp.madness > 0) {
                _currentStatusComp.madness += targetStatusComp.madness;     // todo : need some formula
                _currentStatusComp.InPanic = true;
                return true;
            }
        }
        return false;
    }


    private void FinishReaction() {
        _currentTargetComp.lastTargetIndex = _currentTargetComp.targetIndex;
        _currentReactiveComp.ReactionElapsedTime = 0.0f;
        _currentStatusComp.InPanic = false;
        ++_targetReactiveComp.ReactedCount;

        AdjustAllModifiedComponents();
        PendRewardItem();
    }


    private void AdjustAllModifiedComponents() {
        EntityManager.SetComponentData<TargetComponent>(_currentEntity, _currentTargetComp);
        EntityManager.SetComponentData<MovementComponent>(_currentEntity, _currentMoveComp);
        EntityManager.SetComponentData<AvatarStatusComponent>(_currentEntity, _currentStatusComp);
        EntityManager.SetComponentData<ReactiveComponent>(_currentEntity, _currentReactiveComp);
        EntityManager.SetComponentData<ReactiveComponent>(_targetEntity, _targetReactiveComp);
    }


    private void PendRewardItem() {
        if (false == EntityManager.HasComponent<DropComponent>(_targetEntity)) {
            return;
        }

        DropComponent dropComponent = EntityManager.GetComponentData<DropComponent>(_targetEntity);
        if (false == Utility.IsVaild(dropComponent.dropItemID)) {
            return;
        }
        
        Int64 dropItemID = dropComponent.dropItemID;
        EntityManager.RemoveComponent<DropComponent>(_targetEntity);

        if (EntityManager.HasComponent<PendingItemComponent>(_currentEntity)) {
            PendingItemComponent pendingItemComp = EntityManager.GetComponentData<PendingItemComponent>(_currentEntity);
            pendingItemComp.pendingItemID = dropItemID;
            EntityManager.SetComponentData<PendingItemComponent>(_currentEntity, pendingItemComp);
        }
        else {
            EntityManager.AddComponentData<PendingItemComponent>(_currentEntity, new PendingItemComponent() {
                pendingItemID = dropItemID,
            });
        }
    }


    protected override void OnUpdate() {
        Entities.WithAll<MovementComponent>().ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref TargetComponent targetComp) => {
            _targetEntity = GetTargetEntity(targetComp.targetIndex);
            if (_targetEntity.Equals(Entity.Null)) {
                return;
            }

            // get and keep all components we need
            _currentEntity = entity;
            _currentTargetComp = targetComp;
            _currentMoveComp = EntityManager.GetComponentData<MovementComponent>(_currentEntity);
            _currentStatusComp = EntityManager.GetComponentData<AvatarStatusComponent>(_currentEntity);
            _currentReactiveComp = reactiveComp;
            _targetReactiveComp = EntityManager.GetComponentData<ReactiveComponent>(_targetEntity);

            bool bMoving = math.FLT_MIN_NORMAL < math.lengthsq(_currentMoveComp.value);
            if (bMoving) {
                return;
            }

            if (_targetReactiveComp.type == EntityType.Wall) {
                _currentMoveComp.xValue *= -1.0f;
                FinishReaction();
                return;
            }

            // 타겟 reaction time을 아바타에게 넘김
            if (_targetReactiveComp.reactionTime > 0.0f) {
                _currentReactiveComp.reactionTime = _targetReactiveComp.reactionTime;
            }

            // Done
            if (reactiveComp.reactionTime <= reactiveComp.ReactionElapsedTime) {
                // 뇨뇨 끝
                if (_currentStatusComp.InPanic) {
                    FinishReaction();
                    return;
                }

                // 이성에 타격을 입었다면 패닉(1초로 임시 지정)
                if (ShouldBePanic()) {
                    _currentReactiveComp.ReactionElapsedTime -= 1.0f;
                }
                else {
                    FinishReaction();
                }
            }
            // Doing something
            else if (_currentTargetComp.lastTargetIndex != _currentTargetComp.targetIndex) {
                _currentReactiveComp.ReactionElapsedTime += Time.deltaTime;
            }

            AdjustAllModifiedComponents();
        });
    }
}
