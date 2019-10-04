// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class TargetSystem : ComponentSystem {
    protected override void OnUpdate() {

        int chooseIndex = int.MaxValue;
        int compareIndex = int.MaxValue;
        float compareDist = float.PositiveInfinity;
        Vector2 comparePos = Vector2.zero;

        Entities.ForEach((Entity baseEntity, ref ReactiveComponent baseReactiveComp, ref Translation basePos, ref TargetComponent baseTargetComp) => {
            chooseIndex = int.MaxValue;
            compareIndex = baseEntity.Index;
            compareDist = float.PositiveInfinity;
            comparePos = new Vector2(basePos.Value.x, basePos.Value.y);

            if (baseReactiveComp.type == EntityType.Player) {
                Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos, ref TargetComponent targetComp) => {
                    if (reactiveComp.type == EntityType.Player)
                        return;

                    if (compareIndex == entity.Index)
                        return;

                    float distance = Vector2.Distance(comparePos, new Vector2(pos.Value.x, pos.Value.y));
                    if (Mathf.Abs(distance) < compareDist) {
                        compareDist = Mathf.Abs(distance);
                        chooseIndex = entity.Index;                        
                    }
                });
            }
            else {
                Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos, ref TargetComponent targetComp) => {
                    if (reactiveComp.type != EntityType.Player)
                        return;

                    if (compareIndex == entity.Index)
                        return;

                    float distance = Vector2.Distance(comparePos, new Vector2(pos.Value.x, pos.Value.y));
                    if (Mathf.Abs(distance) < compareDist) {
                        compareDist = Mathf.Abs(distance);
                        chooseIndex = entity.Index;
                    }
                });
            }

            baseTargetComp.targetIndex = chooseIndex;
        });
    }
}
