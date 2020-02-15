// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using GlobalDefine;
using Unity.Collections;

public class NPCAutoMovementSystem : ComponentSystem {
    private EntityQuery _wallQuery;
    protected override void OnCreate() {
        _wallQuery = GetEntityQuery(new EntityQueryDesc() {
            Any = new ComponentType[] {
                typeof(TurningComponent)
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

        Entities.ForEach((Entity entity, ref NPCComponent npcComp, ref Translation npcPos) => {
            if(AnimationType.Idle == npcComp.currentAnim) {
                if (EntityManager.HasComponent<VelocityComponent>(entity)) {
                    EntityManager.RemoveComponent<VelocityComponent>(entity);
                }
            }
            else {
                // add
                if (false == EntityManager.HasComponent<VelocityComponent>(entity)) {
                    EntityManager.AddComponentData<VelocityComponent>(entity, new VelocityComponent());
                }

                // 벽꿍 검출
                var wallQuery = _wallQuery.ToEntityArray(Allocator.TempJob);
                foreach (var wallEntity in wallQuery) {
                    var targetPos = EntityManager.GetComponentData<Translation>(wallEntity).Value;
                    var at = targetPos.x - npcPos.Value.x;
                    if (npcComp.speed >= math.abs(at)) {
                        npcComp.npcDirection *= -1.0f;
                        break;
                    }
                }
                wallQuery.Dispose();

                // set
                var velocityComp = EntityManager.GetComponentData<VelocityComponent>(entity);
                velocityComp.velocity = npcComp.npcDirection * npcComp.speed;
                EntityManager.SetComponentData(entity, velocityComp);
            }           
        });
    }
}
