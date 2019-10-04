// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class TargetSystem : ComponentSystem {
    protected override void OnUpdate() {
        var chooseIndex = int.MaxValue;
        var compareIndex = int.MaxValue;
        var compareDist = float.PositiveInfinity;
        var comparePos = Vector2.zero;
        var compareType = EntityType.None;

        Entities.ForEach((Entity baseEntity, ref ReactiveComponent baseReactiveComp, ref Translation basePos, ref TargetComponent baseTargetComp) => {
            chooseIndex = int.MaxValue;
            compareIndex = baseEntity.Index;
            compareDist = float.PositiveInfinity;
            comparePos = new Vector2(basePos.Value.x, basePos.Value.y);
            compareType = baseReactiveComp.type;

            Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos, ref TargetComponent targetComp) => {
                if (compareIndex == entity.Index)
                    return;

                if (compareType == EntityType.Player && reactiveComp.type == EntityType.Player)
                    return;

                if (compareType != EntityType.Player && reactiveComp.type != EntityType.Player)
                    return;

                // todo : check boundary or check look direction
                float distance = Vector2.Distance(comparePos, new Vector2(pos.Value.x, pos.Value.y));
                if (Mathf.Abs(distance) < compareDist) {
                    compareDist = Mathf.Abs(distance);
                    chooseIndex = entity.Index;
                }
            });

            baseTargetComp.targetIndex = chooseIndex;
        });
    }
}
