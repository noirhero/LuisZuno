// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using GlobalDefine;
using Unity.Collections;

public class NPCAutoMovementSystem : ComponentSystem {
    private EntityQuery _nonePlayerQuery;
    protected override void OnCreate() {
        _nonePlayerQuery = GetEntityQuery(new EntityQueryDesc() {
            None = new ComponentType[] {
                typeof(NPCComponent)
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

        Entities.ForEach((Entity entity, ref NPCComponent npcComp, ref Translation npcPos, ref NPCMovementComponent moveComp) => {
           
        });
    }
}
